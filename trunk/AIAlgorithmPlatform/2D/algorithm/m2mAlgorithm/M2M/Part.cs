using System;
using System.Collections.Generic;
using System.Threading;
using Position_Interface;
using Position_Implement;

namespace M2M
{
    public class Part : IPart_Edit
    {
        public virtual IPart_Edit Create()
        {
            return new Part();
        }

        int x = 0;
        int y = 0;
        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return y;
        }
        public void SetX(float x)
        {
            this.x = (int)x;
        }
        public void SetY(float y)
        {
            this.y = (int)y;
        }
        public void SetX(int x)
        {
            this.x = x;
        }
        public void SetY(int y)
        {
            this.y = y;
        }

        int subPointNum = 0;
        public int GetBottomLevelPointNum()
        {
            return subPointNum;
        }

        public void SubPointNumIncrease(int num)
        {
            subPointNum += num;
        }

        public void SubPointNumDecrease(int num)
        {
            subPointNum -= num;
        }

        public int GetSubPositionNum()
        {
            return subPositionList.Count;
        }

        protected List<IPosition> subPositionList = new List<IPosition>();

        public void AddToSubPositionList(IPosition position)
        {
            subPositionList.Add(position);
        }

        public void RemoveFormSubPositionList(IPosition position)
        {
            subPositionList.Remove(position);
        }

        public IPosition GetRandomPointFormBottomLevel()
        {
            IPosition temp = subPositionList[0];
            if (temp is IPart)
            {
                return temp = ((IPart)temp).GetRandomPointFormBottomLevel();
            }
            else if (temp is IPosition)
            {
                return temp;
            }
            else
            {
                return temp;
            }
        }

        IPosition deputyPoint;
        public IPosition GetRandomOneFormDescendantPoint()
        {
            return deputyPoint;
        }
        public void SetDeputyPoint(IPosition deputyPoint)
        {
            this.deputyPoint = deputyPoint;
        }

        /// <summary>
        /// 得到真实的下一层中，属于该分块的分块集合
        /// </summary>
        /// <returns></returns>
        public virtual IPositionSet GetTrueChildPositionSet()
        {
            return new PositionSetEdit_ImplementByICollectionTemplate(subPositionList);
        }


        //public delegate void dGetPoint(IPosition point);
        //public event dGetPoint OnGetOnePoint;

        //public void TravelAllPointInPart()
        //{
        //    if (OnGetOnePoint == null)
        //    {
        //        throw new Exception("没有响应OnGetOnePoint事件而调用TravelAllPointInPart函数是没有意义的");
        //    }

        //    foreach (IPosition currentPosition in subPositionList)
        //    {
        //        if (currentPosition is Part)
        //        {
        //            Part currentPart = (Part)currentPosition;
                    
        //            //如果Part里面仅仅有一个点,则该点一定是其代表点,该分块不必向下继续遍历.
        //            if (currentPart.GetSubPointNum() == 1)
        //            {
        //                OnGetOnePoint(currentPart.GetRandomOneFormDescendantPoint());
        //            }
        //            else
        //            {
        //                currentPart.OnGetOnePoint = OnGetOnePoint;
        //                currentPart.TravelAllPointInPart();
        //                currentPart.OnGetOnePoint = null;
        //            }
        //        }
        //        //如果是position
        //        else
        //        {
        //            OnGetOnePoint(currentPosition);
        //        }
        //    }
        //}
    }

    class PositionSet_ChildPosition : APositionSet<IPosition>, IPositionSet
    {
        int x1, x2, y1, y2;

        int i, j;

        ILevel currentLevel;

        public PositionSet_ChildPosition(int parentPartSequence, IPart parentPart, ILevel currentLevel)
        {
            y1 = (int)parentPart.GetY() * currentLevel.GetUnitNumInHeight();
            y2 = y1 + currentLevel.GetUnitNumInHeight();
            x1 = (int)parentPart.GetX() * currentLevel.GetUnitNumInWidth();
            x2 = x1 + currentLevel.GetUnitNumInWidth();

            this.currentLevel = currentLevel;
        }


        override public void InitToTraverseSet()
        {
            i = y1;
            j = x1 - 1;
        }

        override public bool NextPosition()
        {
            j++;

            if (j < x2)
            {
                return true;
            }
            else
            {
                j = x1;
                i++;
                if (i < y2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        override public IPosition GetPosition()
        {
            return currentLevel.GetPartRefByPartIndex(j, i);
        }
    }
}