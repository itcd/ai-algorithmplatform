using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Position_Interface;

namespace M2M
{
    public class M2MStructure_General : AM2MStructure, IM2MStructure
    {
        public M2MStructure_General(List<Level> LevelList)
        {
            this.LevelList = LevelList;
        }

        public List<Level> LevelList = null;

        override public ILevel GetLevel(int levelSequecne)
        {
            return LevelList[levelSequecne];
        }

        override public int GetLevelNum()
        {
            return LevelList.Count;
        }

        //得到形式上的子分块集合（比如形式上的子分块有可能是数据结构上的下下层后代分块集合）
        override public IPositionSet GetChildPositionSetByParentPart(int parentPartLevelSequence, IPart parentPart)
        {
            return parentPart.GetTrueChildPositionSet();
        }

        private int ThreadNum = 4;

        private int completeThreadNum;

        private IPositionSet positionSet;

        public void Preprocessing2(IPositionSet positionSet)
        {
            this.positionSet = positionSet;

            completeThreadNum = 0;

            ThreadPool.SetMaxThreads(4, 4);

            positionSet.InitToTraverseSet();
            for (int i = 0; i < ThreadNum; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
            }

            while (completeThreadNum < ThreadNum)
            {
                Thread.CurrentThread.Join(1);
            }
        }

        public void Preprocessing(IPositionSet positionSet)
        {
            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                Insert(positionSet.GetPosition());
            }
        }

        private void ThreadProc(Object o)
        {
            IPosition tempPosition;
            while (true)
            {
                Monitor.Enter(positionSet);
                if (positionSet.NextPosition())
                {
                    tempPosition = positionSet.GetPosition();
                }
                else
                {
                    Monitor.Exit(positionSet);
                    break;
                }
                Monitor.Exit(positionSet);

                Insert(tempPosition);
            }
            Interlocked.Increment(ref completeThreadNum);
        }

        public bool CanInsert(IPosition point)
        {
            float relativeX = LevelList[0].ConvertRealValueToRelativeValueX(point.GetX());
            float relativeY = LevelList[0].ConvertRealValueToRelativeValueY(point.GetY());

            if (relativeX >= 0 && relativeX < LevelList[0].GetGridWidth() && relativeY >= 0 && relativeY < LevelList[0].GetGridHeight())
            {
                return true;
            }
            return false;
        }

        public void Insert(IPosition point)
        {
            IPosition NewPosition = point;

            for (int levelSequrence = LevelList.Count - 1; levelSequrence >= 0; levelSequrence--)
            {
                IPart_Edit currentPart = LevelList[levelSequrence].GOCPartRefByPoint(point);

                Monitor.Enter(currentPart);
                currentPart.AddToSubPositionList(NewPosition);

                //更新不同层里面该点所在的part中点的数目
                currentPart.SubPointNumIncrease(1);
                Monitor.Exit(currentPart);

                if (currentPart.GetSubPositionNum() == 1)
                {
                    //如果part里面只有一个point,说明这个part也是新建的,它所属的宏观分块也必须把它包含.
                    NewPosition = currentPart;
                }
                else
                {
                    break;
                }
            }
        }

        public void Remove_NotTestYet(IPosition point)
        {
            //更新不同层里面该点所在的part中点的数目
            for (int levelSequrence = LevelList.Count - 2; levelSequrence >= 0; levelSequrence--)
            {
                ((IPart_Edit)LevelList[levelSequrence + 1].GetPartRefByPoint(point)).SubPointNumDecrease(1);
            }

            IPosition RemovePosition = point;

            for (int levelSequrence = LevelList.Count - 1; levelSequrence >= -1; levelSequrence--)
            {
                if (levelSequrence == -1)
                {
                    ((IPart_Edit)LevelList[0].GetPartRefByPoint(point)).RemoveFormSubPositionList(RemovePosition);
                    break;
                }

                IPart_Edit currentPart = (IPart_Edit)(LevelList[levelSequrence].GetPartRefByPoint(point));

                if (currentPart == null)
                {
                    throw new Exception("不存在该分块的指针!(指定删除的点是否已经存在?)");
                }

                currentPart.RemoveFormSubPositionList(RemovePosition);

                if (currentPart.GetSubPositionNum() == 0)
                {
                    //如果part里面一个点都没有,应该把该part从表格中删除.
                    LevelList[levelSequrence].RemovePartByPoint(point);
                    //同时也应该从包含该分块的宏观分块中删除该分块.
                    RemovePosition = currentPart;
                }
                else
                {
                    IPosition newDeputyPoint = null;
                    int levelSequrence2 = levelSequrence;
                    IPosition deputyPoint = LevelList[levelSequrence2].GetPartRefByPoint(point).GetRandomOneFormDescendantPoint();
                    if (point == deputyPoint)
                    {
                        newDeputyPoint = currentPart.GetRandomPointFormBottomLevel();
                    }

                    //如果该part里面超过一个点则可以结束删除工作,不过需要判断一下该part以及其所属宏观part的代表点是否是已删除点,如果是,需要更新.
                    for (levelSequrence2 = levelSequrence; levelSequrence2 >= 0; levelSequrence2--)
                    {
                        deputyPoint = LevelList[levelSequrence2].GetPartRefByPoint(point).GetRandomOneFormDescendantPoint();
                        if (point == deputyPoint)
                        {
                            ((IPart_Edit)(LevelList[levelSequrence2].GetPartRefByPoint(point))).SetDeputyPoint(newDeputyPoint);
                        }
                        else
                        {
                            //如果到了某层的代表点已经不是已删除点,则不需要继续向上查询.
                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}