using System;
using System.Collections.Generic;
using System.Text;

namespace M2M
{
    /*
    public class M2M_NN1 : INearestNeighbor
    {
        const float bias = 0.01f;

        float MapWidth;
        float MapHeight;

        float MaxGridLength;
        int UnitNumInGridLength;
        int levelNum;

        List<Level> LevelList = new List<Level>();

        Level GetLevel(int num)
        {
            return LevelList[num];
        }

        int GetLevelNum()
        {
            return LevelList.Count;
        }

        List<IPosition> root = new List<IPosition>();

        public void Init(float MapWidth, float MapHeight, float MaxGridLength, int UnitNumInLength, int levelNum)
        {
            this.MapWidth = MapWidth;
            this.MapHeight = MapHeight;

            this.MaxGridLength = MaxGridLength;
            this.UnitNumInGridLength = UnitNumInLength;
            this.levelNum = levelNum;
        }

        private void InitMemory()
        {
            Level level0 = new Level();

            level0.SetUnitNumInWidth((int)Math.Ceiling(MapWidth / MaxGridLength));
            level0.SetUnitNumInHeight((int)Math.Ceiling(MapHeight / MaxGridLength));

            level0.SetGridWidth(MaxGridLength);
            level0.SetGridHeight(MaxGridLength);

            level0.AllocateMemory();

            LevelList.Clear();
            LevelList.Add(level0);

            for (int levelSequence = 1; levelSequence < levelNum; levelSequence++)
            {
                Level level = new Level();

                level.SetUnitNumInWidth(UnitNumInGridLength * LevelList[levelSequence - 1].GetUnitNumInWidth());
                level.SetUnitNumInHeight(UnitNumInGridLength * LevelList[levelSequence - 1].GetUnitNumInHeight());

                level.SetGridWidth(LevelList[levelSequence - 1].GetGridWidth() / UnitNumInGridLength);
                level.SetGridHeight(LevelList[levelSequence - 1].GetGridHeight() / UnitNumInGridLength);

                level.AllocateMemory();

                LevelList.Add(level);
            }

            ////////////为其它成员分配空间/////////////
            SearchEverPartList.Capacity = 25;
            WaitToSearchPartList.Capacity = 25;
        }

        public void PreProcess(List<IPosition> pointList)
        {
            int PointNum = pointList.Count;

            float MaxLargeX = float.MinValue;
            float MaxLargeY = float.MinValue;

            foreach (IPosition point in pointList)
            {
                if (point.GetX() > MaxLargeX)
                {
                    MaxLargeX = point.GetX();
                }

                if (point.GetY() > MaxLargeY)
                {
                    MaxLargeY = point.GetY();
                }
            }

            //float mapWidth = MaxLargeX + 0.0001f;
            //float mapHeight = MaxLargeY + 0.0001f;

            float mapWidth = MaxLargeX;
            float mapHeight = MaxLargeY;

            UnitNumInGridLength = 5;
            int MicPartNumInMacPart = UnitNumInGridLength * UnitNumInGridLength;

            MaxGridLength = mapWidth / UnitNumInGridLength;

            if (MaxGridLength > mapHeight / UnitNumInGridLength)
            {
                MaxGridLength = mapHeight / UnitNumInGridLength;
            }

            MaxGridLength += bias;
            
            levelNum = (int)Math.Ceiling(Math.Log(PointNum * 1.7, MicPartNumInMacPart));
            //levelNum = 4;

            MapWidth = mapWidth;
            MapHeight = mapHeight;

            InitMemory();

            foreach (IPosition point in pointList)
            {
                Insert(point);
            }
        }

        public void Insert(IPosition point)
        {
            IPosition NewPosition = point;

            for (int levelSequrence = LevelList.Count - 1; levelSequrence >= -1; levelSequrence--)
            {
                if (levelSequrence == -1)
                {
                    root.Add(NewPosition);
                    break;
                }

                Part currentPart = LevelList[levelSequrence].GOCPartRefByPoint(point);

                currentPart.AddToSubPositionList(NewPosition);
                
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

            //更新不同层里面该点所在的part中点的数目
            for (int levelSequrence = LevelList.Count - 1; levelSequrence >= 0; levelSequrence--)
            {
                ((Part)LevelList[levelSequrence].GetPartRefByPoint(point)).SubPointNumIncrease(1);
            }
        }

        public void Remove(IPosition point)
        {
            //更新不同层里面该点所在的part中点的数目
            for (int levelSequrence = LevelList.Count - 1; levelSequrence >= 0; levelSequrence--)
            {
                ((Part)LevelList[levelSequrence].GetPartRefByPoint(point)).SubPointNumDecrease(1);
            }

            IPosition RemovePosition = point;

            for (int levelSequrence = LevelList.Count; levelSequrence >= -1; levelSequrence--)
            {
                if (levelSequrence == -1)
                {
                    root.Remove(RemovePosition);
                    break;
                }

                Part currentPart = (Part)LevelList[levelSequrence].GetPartRefByPoint(point);

                if(currentPart == null)
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
                    //如果该part里面超过一个点则可以结束删除工作,不过需要判断一下该part以及其所属宏观part的代表点是否是已删除点,如果是,需要更新.
                    for (int levelSequrence2 = levelSequrence; levelSequrence2 >= 0; levelSequrence2++)
                    {
                        IPosition deputyPoint = LevelList[levelSequrence2].GetPartRefByPoint(point).GetRandomOneFormDescendantPoint();
                        if (point == deputyPoint)
                        {
                            deputyPoint = currentPart.GetRandomPositionFormSubPositionList();
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

        private Level SearchTheTargetPart(IPosition targetPoint, ref Part targetPart)
        {
            int levelSequence = 0;

            if (LevelList[0].GetPartRefByPoint(targetPoint) == null)
            {
                //这种情况还没有处理
                //throw new Exception("这种情况还没有处理!");
                
                targetPart = LevelList[0].GetPartRefByPoint(targetPoint);

                levelSequence = 0;
            }
            else
            {
                for (levelSequence = 1; levelSequence < LevelList.Count; levelSequence++)
                {
                    //如果该分块不存在,说明该分块里面没有其它点.
                    if (LevelList[levelSequence].GetPartRefByPoint(targetPoint) == null)
                    {
                        targetPart = LevelList[levelSequence - 1].GetPartRefByPoint(targetPoint);

                        break;
                    }
                }

                levelSequence--;

                if (targetPart == null)
                {
                    targetPart = LevelList[LevelList.Count - 1].GetPartRefByPoint(targetPoint);

                    levelSequence = LevelList.Count - 1;
                }
            }

            return LevelList[levelSequence];
        }

        public IPosition ApproximateNearestNeighbor(IPosition targetPoint)
        {
            Part targetPart = null;

            SearchTheTargetPart(targetPoint, ref targetPart);

            TravelThePointInPart travelThePointInPart = new TravelThePointInPart();

            travelThePointInPart.InitToGetTheNearestPointInPart(targetPart,targetPoint);

            return travelThePointInPart.GetTheNearestPointInPart();
        }

        //精确搜索最近邻每次都会用到.
        List<Part> SearchEverPartList = new List<Part>();
        List<Part> WaitToSearchPartList = new List<Part>();

        public IPosition NearestNeighbor2(IPosition point)
        {
            //初始化变量
            SearchEverPartList.Clear();
            IPosition CurrentNearestPoint;
            
            float CurrentNearestDistanceSquare = float.MaxValue;
            TravelThePointInPart travelThePointInPart = new TravelThePointInPart();


            Part targetPart = null;
            Level targetPartLevel = SearchTheTargetPart(point, ref targetPart);
            
            travelThePointInPart.InitToGetTheNearestPointInPart(targetPart, point);
            CurrentNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
            CurrentNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();
            SearchEverPartList.Add(targetPart);

            while (true)
            {
                DetermineWaitToSearchPartList(point, CurrentNearestDistanceSquare, targetPartLevel);

                //(未实现)
                //把WaitToSearchPartList每个分块按该分块的代表点到考察点的距离做一次冒泡,距离越小的越早被选中.
                //如果最小距离比当前最小距离上限小,应该更新最小距离上限,并更新最小距离点,并重新画出搜索正方形,重新开始搜索.

                foreach (Part currentCheckedPart in WaitToSearchPartList)
                {
                    travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, point);

                    IPosition CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
                    float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();
                    SearchEverPartList.Add(currentCheckedPart);

                    //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并重新画出搜索正方形,重新开始搜索.
                    if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
                    {
                        CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
                        CurrentNearestPoint = CurrentPartNearestPoint;
                        continue;
                    }
                }

                break;
            }

            return CurrentNearestPoint;
        }

        private Level SearchTheTargetPart2(IPosition targetPoint, ref Part targetPart)
        {
            int maxCheckPointNum = 18;

            int levelSequence = 0;

            for (levelSequence = 0; levelSequence < LevelList.Count; levelSequence++)
            {
                Part currentPart = LevelList[levelSequence].GetPartRefByPoint(targetPoint);

                if ((levelSequence == 0) && (currentPart == null))
                {
                    targetPart = null;

                    return LevelList[0];
                }

                if ((currentPart == null) || (currentPart.GetSubPointNum() <= maxCheckPointNum))
                {
                    targetPart = currentPart;

                    return LevelList[levelSequence];
                }
            }

            if (targetPart == null)
            {
                targetPart = LevelList[LevelList.Count - 1].GetPartRefByPoint(targetPoint);

                return LevelList[LevelList.Count - 1];
            }

            throw new Exception("不应该到达这里!");

            return LevelList[0];
        }

        enum HorizontalSearchPriority { left, right }
        enum VerticalSearchPriority { top, bottom }

        int leftSearchLine, rightSearchLine, bottomSearchLine, topSearchLine;

        //点集搜索边界
        int upperBound, lowerBound, leftBound, rightBound;

        public IPosition NearestNeighbor(IPosition targetPoint)
        {
            //初始化变量
            IPosition CurrentNearestPoint = null;

            float CurrentNearestDistanceSquare = float.MaxValue;
            float CurrentNearestDistance = float.MaxValue;

            TravelThePointInPart travelThePointInPart = new TravelThePointInPart();

            Part targetPart = null;
            Level targetPartLevel = SearchTheTargetPart2(targetPoint, ref targetPart);

            HorizontalSearchPriority horizontalSearchPriority;
            VerticalSearchPriority verticalSearchPriority;

            int horizontalForewardSearchValue;
            int horizontalAfterwardSearchValue;
            int verticalForewardSearchValue;
            int verticalAfterwardSearchValue;

            int targetPartX = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX());
            int targetPartY = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY());

            //计算搜索框

            //如果点在分块的左半部分
            if (targetPoint.GetX() <= targetPartLevel.ConvertPartSequenceXToRealityValue(Convert.ToInt32(targetPartX)) + targetPartLevel.GetGridWidth() / 2)
            {
                leftSearchLine = targetPartX - 1;
                rightSearchLine = targetPartX;
                horizontalSearchPriority = HorizontalSearchPriority.right;
            }
            else
            {
                leftSearchLine = targetPartX;
                rightSearchLine = targetPartX + 1;
                horizontalSearchPriority = HorizontalSearchPriority.left;
            }

            //如果点在分块的下半部分
            if (targetPoint.GetY() <= targetPartLevel.ConvertPartSequenceYToRealityValue(Convert.ToInt32(targetPartY)) + targetPartLevel.GetGridHeight() / 2)
            {
                bottomSearchLine = targetPartY - 1;
                topSearchLine = targetPartY;
                verticalSearchPriority = VerticalSearchPriority.top;
            }
            else
            {
                bottomSearchLine = targetPartY;
                topSearchLine = targetPartY + 1;
                verticalSearchPriority = VerticalSearchPriority.bottom;
            }

            //把点集边界作为搜索边界
            upperBound = targetPartLevel.GetUnitNumInHeight() - 1;
            lowerBound = 0;
            leftBound = 0;
            rightBound = targetPartLevel.GetUnitNumInWidth() - 1;
                              
            while (true)
            {
                //判断搜索线是否在地图边界以内.
                //ISTheSearchLineInsideMapAndUpdateTheSearchLine(targetPartLevel);

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    verticalForewardSearchValue = topSearchLine;
                    verticalAfterwardSearchValue = bottomSearchLine;
                }
                else
                {
                    verticalForewardSearchValue = bottomSearchLine;
                    verticalAfterwardSearchValue = topSearchLine;
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    horizontalForewardSearchValue = rightSearchLine;
                    horizontalAfterwardSearchValue = leftSearchLine;
                }
                else
                {
                    horizontalForewardSearchValue = leftSearchLine;
                    horizontalAfterwardSearchValue = rightSearchLine;
                }

                //if ((verticalForewardSearchValue < point.GetY() + CurrentNearestDistance) && (verticalForewardSearchValue > point.GetY() - CurrentNearestDistance))

                bool isNeedToSearch = true;

                ///////////////////////垂直优先////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    for (int j = leftSearchLine + 1; j <= rightSearchLine - 1; j++)
                    {
                        if (ISTheCoordinateInsideSearchBound(j, verticalForewardSearchValue))
                        {
                            Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalForewardSearchValue);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////水平优先////////////////////////
                isNeedToSearch = true;

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    for (int i = bottomSearchLine + 1; i <= topSearchLine - 1; i++)
                    {
                        if (ISTheCoordinateInsideSearchBound(horizontalForewardSearchValue, i))
                        {
                            Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, i);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////垂直滞后////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    for (int j = leftSearchLine + 1; j <= rightSearchLine - 1; j++)
                    {
                        if (ISTheCoordinateInsideSearchBound(j, verticalAfterwardSearchValue))
                        {
                            Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalAfterwardSearchValue);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////水平滞后////////////////////////
                isNeedToSearch = true;

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    for (int i = bottomSearchLine + 1; i <= topSearchLine - 1; i++)
                    {
                        if (ISTheCoordinateInsideSearchBound(horizontalAfterwardSearchValue, i))
                        {
                            Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, i);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////优先角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, verticalForewardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                    }
                }

                ///////////////////////垂直优先水平滞后角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, verticalForewardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);                    }
                }

                ///////////////////////垂直滞后水平优先角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, verticalAfterwardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);                    
                    }
                }

                ///////////////////////垂直滞后水平滞后角落////////////////////////

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBound)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBound)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, verticalAfterwardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, ref CurrentNearestPoint, ref CurrentNearestDistanceSquare, ref CurrentNearestDistance, travelThePointInPart, targetPartLevel, currentCheckedPart);
                    }
                }

                //更新搜索边界
                leftSearchLine -= 1;
                rightSearchLine += 1;
                bottomSearchLine -= 1;
                topSearchLine += 1;

                if ((leftSearchLine < leftBound) && (rightSearchLine > rightBound) && (topSearchLine > upperBound) && (bottomSearchLine < lowerBound))
                {
                    break;
                }
            }

            return CurrentNearestPoint;
        }

        private bool ISTheCoordinateInsideSearchBound(int x, int y)
        {
            return (x <= rightBound && x >= leftBound && y <= upperBound && y >= lowerBound);
        }

        private bool ISThePartInsideSearchBound(Part currentCheckedPart)
        {
            return ((currentCheckedPart.GetX()) <= rightBound) && ((currentCheckedPart.GetX()) >= leftBound)
                                        && ((currentCheckedPart.GetY()) <= upperBound) && ((currentCheckedPart.GetX()) >= lowerBound);
        }

        //private void ISTheSearchLineInsideMapAndUpdateTheSearchLine(Level targetPartLevel)
        //{
        //    //判断搜索线是否在地图边界以内.
        //    if (topSearchLine > targetPartLevel.GetUnitNumInHeight() - 1)
        //    {
        //        topSearchLine = targetPartLevel.GetUnitNumInHeight() - 1;
        //    }

        //    if (bottomSearchLine < 0)
        //    {
        //        bottomSearchLine = 0;
        //    }

        //    if (rightSearchLine > targetPartLevel.GetUnitNumInWidth() - 1)
        //    {
        //        rightSearchLine = targetPartLevel.GetUnitNumInWidth() - 1;
        //    }

        //    if (leftSearchLine < 0)
        //    {
        //        leftSearchLine = 0;
        //    }
        //}

        private void SearchNearestPointInOnePart(IPosition targetPoint, ref IPosition CurrentNearestPoint, ref float CurrentNearestDistanceSquare, ref float CurrentNearestDistance, TravelThePointInPart travelThePointInPart, Level targetPartLevel, Part currentCheckedPart)
        {
            travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, targetPoint);

            IPosition CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
            float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();

            //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并更新最大搜索边界
            if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
            {
                CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
                CurrentNearestPoint = CurrentPartNearestPoint;

                CurrentNearestDistance = (float)Math.Sqrt((double)CurrentNearestDistanceSquare);

                //更新搜索边界
                upperBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() + CurrentNearestDistance);
                lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() - CurrentNearestDistance);
                leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() - CurrentNearestDistance);
                rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() + CurrentNearestDistance);

                //判断搜索边界是否在地图边界以内
                if (upperBound > targetPartLevel.GetUnitNumInHeight() - 1)
                {
                    upperBound = targetPartLevel.GetUnitNumInHeight() - 1;
                }

                if (lowerBound < 0)
                {
                    lowerBound = 0;
                }

                if (rightBound > targetPartLevel.GetUnitNumInWidth() - 1)
                {
                    rightBound = targetPartLevel.GetUnitNumInWidth() - 1;
                }

                if (leftBound < 0)
                {
                    leftBound = 0;
                }
            }
        }

        private static void SearchNearestPointOnOneSide(IPosition targetPoint, ref IPosition CurrentNearestPoint, ref float CurrentNearestDistanceSquare, ref float CurrentNearestDistance, TravelThePointInPart travelThePointInPart, Level targetPartLevel, int leftSearchLine, int rightSearchLine, ref int upperBound, ref int lowerBound, ref int leftBound, ref int rightBound)
        {
            //for (int j = leftSearchLine + 1; j <= rightSearchLine - 1; j++)
            //{
            //    Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalForewardSearchValue);
            //    if (currentCheckedPart != null)
            //    {
            //        travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, targetPoint);

            //        Position CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
            //        float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();

            //        //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并更新最大搜索边界
            //        if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
            //        {
            //            CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
            //            CurrentNearestPoint = CurrentPartNearestPoint;

            //            CurrentNearestDistance = (float)Math.Sqrt((double)CurrentNearestDistanceSquare);

            //            //更新搜索边界
            //            upperBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() + CurrentNearestDistance);
            //            lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() - CurrentNearestDistance);
            //            leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() - CurrentNearestDistance);
            //            rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() + CurrentNearestDistance);
            //        }
            //    }
            //}         
        }

        private void DetermineWaitToSearchPartList(IPosition point, float CurrentNearestDistanceSquare, Level targetPartLevel)
        {
            float CurrentNearestDistance = (float)Math.Sqrt((double)CurrentNearestDistanceSquare);

            int upperBound = targetPartLevel.ConvertRealityValueToPartSequenceY(point.GetY() + CurrentNearestDistance);
            int lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(point.GetY() - CurrentNearestDistance);
            int leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(point.GetX() - CurrentNearestDistance);
            int rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(point.GetX() + CurrentNearestDistance);

            WaitToSearchPartList.Clear();

            for (int i = lowerBound; i <= upperBound; i++)
            {
                for (int j = leftBound; j <= rightBound; j++)
                {
                    if (!targetPartLevel.IndexIsExceedPartRange(j, i))
                    {
                        Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, i);
                        if ((currentCheckedPart != null) && (!SearchEverPartList.Contains(currentCheckedPart)))
                        {
                            WaitToSearchPartList.Add(currentCheckedPart);
                        }
                    }
                }
            }
        }
        
        class TravelThePointInPart
        {
            Part currentPart;
            IPosition comparePoint;
            IPosition nearestPointToComparePoint;
            float nearestDistanceSquare;

            public float GetNearestDistanceSquare()
            {
                return nearestDistanceSquare;
            }

            float CalculateTheDistanceFromComparePoint(IPosition point)
            {
                float dx = point.GetX() - comparePoint.GetX();
                float dy = point.GetY() - comparePoint.GetY();

                return dx * dx + dy * dy;
            }

            public void InitToGetTheNearestPointInPart(Part part,IPosition point)
            {
                currentPart = part;
                comparePoint = point;
                nearestDistanceSquare = float.MaxValue;
            }

            void CompareToThePoint(IPosition point)
            {
                float currentDistance = CalculateTheDistanceFromComparePoint(point);
                if (nearestDistanceSquare > currentDistance)
                {
                    nearestPointToComparePoint = point;
                    nearestDistanceSquare = currentDistance;
                }
            }

            public IPosition GetTheNearestPointInPart()
            {
                //currentPart.OnGetOnePoint = this.CompareToThePoint;

                currentPart.OnGetOnePoint += CompareToThePoint;

                currentPart.TravelAllPointInPart();

                currentPart.OnGetOnePoint -= CompareToThePoint;

                return nearestPointToComparePoint;
            }
        }        
    }
    */
}