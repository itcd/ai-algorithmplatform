using System;
using System.Collections.Generic;
using Position_Interface;
using Position_Implement;

namespace M2M
{
    class NearestNeighbor_ByM2MStructure
    {
        #region code for algorithm demo
        public event dGetPosition GetQueryPosition;
        public event dGetPartInSpecificLevel GetQueryPart;
        public event dSearchOnePartBeginAndGetSearchPartSequence SearchOnePartBeginAndGetSearchPartSequence;
        public event dGetPartInSpecificLevel GetSearchPart;
        public event dGetPosition GetComparedPoint;
        public event dGetPosition CurrentNearestPointChanged;
        public event dGetRectangle SearchBoundChanged;
        #endregion

        int set_MaxCheckPointNum = 15;

        //////////这个类对象以后要改成接口指针////////////
        IM2MStructure m2mStructure = null;

        public NearestNeighbor_ByM2MStructure(IM2MStructure m2mS)
        {
            Init(m2mS);
        }

        void Init(IM2MStructure m2mS)
        {
            m2mStructure = m2mS;
        }

        private int SearchTheTargetPart2(IPosition targetPoint, ref IPart targetPart)
        {
            int levelSequence;

            int BottonlevelSequence = m2mStructure.GetLevelNum() - 1;

            IPart currentPart = m2mStructure.GetLevel(1).GetPartRefByPoint(targetPoint);

            if (currentPart == null)
            {
                targetPart = null;

                return 1;
            }

            for (levelSequence = 2; levelSequence < BottonlevelSequence; levelSequence++)
            {
                currentPart = m2mStructure.GetLevel(levelSequence).GetPartRefByPoint(targetPoint);

                if ((currentPart == null) || (currentPart.GetBottomLevelPointNum() <= set_MaxCheckPointNum))
                {
                    targetPart = currentPart;
                    return levelSequence;
                }
            }

            if (targetPart == null)
            {
                targetPart = m2mStructure.GetLevel(BottonlevelSequence).GetPartRefByPoint(targetPoint);

                return BottonlevelSequence;
            }

            throw new Exception("不应该到达这里!");

            return 1;
        }

        public IPositionSet ApproximateSearchKNearestNeighbor2(IPosition targetPoint, int k)
        {
            int levelSequence;

            int BottonlevelSequence = m2mStructure.GetLevelNum() - 1;

            IPart currentPart = m2mStructure.GetLevel(1).GetPartRefByPoint(targetPoint);

            if (currentPart == null)
            {
                return m2mStructure.GetBottonLevelPositionSetByAncestorPart(m2mStructure.GetLevel(0).GetPartRefByPoint(targetPoint), 0);
            }

            for (levelSequence = 2; levelSequence < BottonlevelSequence; levelSequence++)
            {
                ILevel currentLevel = m2mStructure.GetLevel(levelSequence);

                currentPart = currentLevel.GetPartRefByPoint(targetPoint);

                if ((currentPart == null) || (currentPart.GetBottomLevelPointNum() <= k))
                {
                    return m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentPart, levelSequence);
                }
            }

            currentPart = m2mStructure.GetLevel(BottonlevelSequence).GetPartRefByPoint(targetPoint);

            return m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentPart, BottonlevelSequence);
        }

        public IPositionSet ApproximateSearchKNearestNeighbor3(IPosition targetPoint, int k)
        {
            int levelSequence;

            int BottonlevelSequence = m2mStructure.GetLevelNum() - 1;

            IPart currentPart;

            for (levelSequence = 0; levelSequence < BottonlevelSequence; levelSequence++)
            {
                ILevel currentLevel = m2mStructure.GetLevel(levelSequence);

                currentPart = currentLevel.GetPartRefByPoint(targetPoint);

                if ((currentPart == null) || (currentPart.GetBottomLevelPointNum() <= k))
                {
                    return m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentPart, levelSequence);
                }
            }

            currentPart = m2mStructure.GetLevel(BottonlevelSequence).GetPartRefByPoint(targetPoint);

            return m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentPart, BottonlevelSequence);
        }

        public IPositionSet ApproximateSearchKNearestNeighbor(IPosition targetPoint, int k)
        {
            KNearestPointSList = new SortedList<float, IPosition>();

            KNearestPointSList.Capacity = k;

            IPart targetPart = null;
            int targetPartLevelSequence = SearchTheTargetPart(targetPoint, ref targetPart);

            GetOnePointAndDetermineNearestPointIsChanged = SearchKNearestPointInComparedPointSet;

            DiffuseFromOriginByPartInSpecialLevel(targetPoint, targetPartLevelSequence);

            return new PositionSetEdit_ImplementByICollectionTemplate(KNearestPointSList.Values);
        }

        private int SearchTheTargetPart(IPosition targetPoint, ref IPart targetPart)
        {
            int levelSequence;

            int BottonlevelSequence = m2mStructure.GetLevelNum() - 1;

            IPart currentPart = m2mStructure.GetLevel(1).GetPartRefByPoint(targetPoint);

            if (currentPart == null)
            {
                targetPart = null;

                return 1;
            }

            for (levelSequence = 2; levelSequence < BottonlevelSequence; levelSequence++)
            {
                //currentPart = m2mStructure.GetLevel(levelSequence).GetPartRefByPoint(targetPoint);

                //if ((currentPart == null) || (currentPart.GetSubPointNum() <= set_MaxCheckPointNum))
                //{
                //    targetPart = currentPart;
                //    return levelSequence;
                //}

                ILevel currentLevel = m2mStructure.GetLevel(levelSequence);

                int pX = currentLevel.ConvertRealValueToPartSequenceX(targetPoint.GetX());
                int pY = currentLevel.ConvertRealValueToPartSequenceY(targetPoint.GetY());

                currentPart = currentLevel.GetPartRefByPartIndex(pX, pY);
                
                int totalNum = 0;

                if (currentPart != null)
                {
                    totalNum += 4 * currentPart.GetBottomLevelPointNum();
                }

                IPart tempPart = null;

                tempPart = currentLevel.GetPartRefByPartIndex(pX + 1, pY);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX - 1, pY);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX, pY + 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX, pY - 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX + 1, pY + 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX + 1, pY - 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX - 1, pY + 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                tempPart = currentLevel.GetPartRefByPartIndex(pX - 1, pY - 1);
                if (tempPart != null)
                {
                    totalNum += tempPart.GetBottomLevelPointNum();
                }

                if (totalNum <= set_MaxCheckPointNum)
                {
                    targetPart = currentPart;
                    return levelSequence;
                }
            }

            if (targetPart == null)
            {
                targetPart = m2mStructure.GetLevel(BottonlevelSequence).GetPartRefByPoint(targetPoint);

                return BottonlevelSequence;
            }

            throw new Exception("不应该到达这里!");

            return 1;
        }

        class TravelThePointInPart
        {
            int AncestorLevelSequence;
            IM2MStructure M2MS;

            IPart currentPart;
            IPosition comparePoint;
            IPosition nearestPointToComparePoint;
            float nearestDistanceSquare;

            float upperBound;
            float lowerBound;
            float leftBound;
            float rightBound;

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

            public void InitToGetTheNearestPointInPart(IPart part, int AncestorLevelSequence, IM2MStructure M2MS, IPosition point)
            {
                this.AncestorLevelSequence = AncestorLevelSequence;
                this.M2MS = M2MS;

                currentPart = part;
                comparePoint = point;
                nearestDistanceSquare = float.MaxValue;

                upperBound = float.MaxValue;
                lowerBound = 0;
                leftBound = 0;
                rightBound = float.MaxValue;
            }

            void CompareToThePoint(IPosition point)
            {
                ////如果点位于边界矩形的左上
                //if(point.GetX() < )

                float currentDistance = CalculateTheDistanceFromComparePoint(point);
                if (nearestDistanceSquare > currentDistance)
                {
                    nearestPointToComparePoint = point;
                    nearestDistanceSquare = currentDistance;
                }
            }

            public IPosition GetTheNearestPointInPart()
            {
                IPositionSet BottonLevelPositionSet =
                    M2MS.GetBottonLevelPositionSetByAncestorPart(currentPart, AncestorLevelSequence);

                BottonLevelPositionSet.InitToTraverseSet();
                while (BottonLevelPositionSet.NextPosition())
                {
                    CompareToThePoint(BottonLevelPositionSet.GetPosition());
                }

                //currentPart.OnGetOnePoint = this.CompareToThePoint;

                //((Part)currentPart).OnGetOnePoint += CompareToThePoint;

                //((Part)currentPart).TravelAllPointInPart();

                //((Part)currentPart).OnGetOnePoint -= CompareToThePoint;

                return nearestPointToComparePoint;
            }
        }

        public IPosition ApproximateNearestNeighbor(IPosition targetPoint)
        {
            if (m2mStructure == null)
            {
                throw new Exception("NearestNeighbor_ByM2MStructure 类未被调用Init函数，或者初始化值为空");
            }

            IPart targetPart = null;

            int targetPartLevelSequence = SearchTheTargetPart(targetPoint, ref targetPart);

            TravelThePointInPart travelThePointInPart = new TravelThePointInPart();

            travelThePointInPart.InitToGetTheNearestPointInPart(targetPart, targetPartLevelSequence, m2mStructure, targetPoint);

            return travelThePointInPart.GetTheNearestPointInPart();
        }

        #region code for algorithm demo        
        int SearchPartSequence;
        #endregion
        public IPosition NearestNeighbor(IPosition targetPoint)
        {
            IPart targetPart = null;
            int targetPartLevelSequence = SearchTheTargetPart(targetPoint, ref targetPart);

            #region code for algorithm demo
            if (GetQueryPosition != null)
            {               
                GetQueryPosition(targetPoint);
            }            

            if (SearchOnePartBeginAndGetSearchPartSequence != null)
            {
                SearchPartSequence = 0;
            }
            #endregion            

            GetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird = SearchNearestPointInComparedPointSetAndShrinkSearchBound;
            //GetOnePointAndDetermineNearestPointIsChanged = SearchNearestPointInComparedPointSet;
            DiffuseFromOriginByPartInSpecialLevel(targetPoint, targetPartLevelSequence);

            return CurrentNearestPoint;
        }

        enum HorizontalSearchPriority { left, right }
        enum VerticalSearchPriority { top, bottom }

        //包围搜索线
        int leftSearchLine, rightSearchLine, bottomSearchLine, topSearchLine;

        //真实边界
        float upperBound, lowerBound, leftBound, rightBound;

        //网格上的边界
        int upperBoundInGrid, lowerBoundInGrid, leftBoundInGrid, rightBoundInGrid;

        //最近点相关变量
        IPosition CurrentNearestPoint;
        float CurrentNearestDistanceSquare;
        float CurrentNearestDistance;

        private void DiffuseFromOriginByPartInSpecialLevel(IPosition targetPoint, int targetPartLevelSequence)
        {
            CurrentNearestPoint = null;

            CurrentNearestDistanceSquare = float.MaxValue;
            CurrentNearestDistance = float.MaxValue;

            ILevel targetPartLevel = m2mStructure.GetLevel(targetPartLevelSequence);

            HorizontalSearchPriority horizontalSearchPriority;
            VerticalSearchPriority verticalSearchPriority;

            int targetPartX = targetPartLevel.ConvertRealValueToPartSequenceX(targetPoint.GetX());
            int targetPartY = targetPartLevel.ConvertRealValueToPartSequenceY(targetPoint.GetY());

            #region code for algorithm demo
            if (GetQueryPart != null)
            {
                IPart_Edit temp = m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0).Create();
                temp.SetX(targetPartX);
                temp.SetY(targetPartY);
                GetQueryPart(m2mStructure.GetLevel(targetPartLevelSequence), targetPartLevelSequence, temp);
            }
            #endregion

            //计算搜索框

            //如果点在分块的左半部分
            if (targetPoint.GetX() <= targetPartLevel.ConvertPartSequenceXToRealValue(targetPartX) + targetPartLevel.GetGridWidth() / 2)
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
            if (targetPoint.GetY() <= targetPartLevel.ConvertPartSequenceYToRealValue(targetPartY) + targetPartLevel.GetGridHeight() / 2)
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

            //初始化边界
            upperBound = float.MaxValue;
            lowerBound = float.MinValue;
            leftBound = float.MinValue;
            rightBound = float.MaxValue;

            //把点集边界作为搜索边界
            upperBoundInGrid = targetPartLevel.GetUnitNumInHeight() - 1;
            lowerBoundInGrid = 0;
            leftBoundInGrid = 0;
            rightBoundInGrid = targetPartLevel.GetUnitNumInWidth() - 1;

            SquareDiffuseFromSearchLineToSearchBoundInGrid(targetPoint, targetPartLevelSequence, targetPartLevel, horizontalSearchPriority, verticalSearchPriority);
        }

        private void SquareDiffuseFromSearchLineToSearchBoundInGrid(IPosition targetPoint, int targetPartLevelSequence, ILevel targetPartLevel, HorizontalSearchPriority horizontalSearchPriority, VerticalSearchPriority verticalSearchPriority)
        {
            while (true)
            {
                int horizontalForewardSearchValue;
                int horizontalAfterwardSearchValue;
                int verticalForewardSearchValue;
                int verticalAfterwardSearchValue;

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

                bool isNeedToSearch = true;

                ///////////////////////垂直优先////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBoundInGrid)
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
                            IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalForewardSearchValue);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////水平优先////////////////////////
                isNeedToSearch = true;

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBoundInGrid)
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
                            IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, i);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////垂直滞后////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBoundInGrid)
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
                            IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalAfterwardSearchValue);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////水平滞后////////////////////////
                isNeedToSearch = true;

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBoundInGrid)
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
                            IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, i);
                            if (currentCheckedPart != null)
                            {
                                SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                            }
                        }
                    }
                }

                ///////////////////////优先角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, verticalForewardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                    }
                }

                ///////////////////////垂直优先水平滞后角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalForewardSearchValue > upperBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalForewardSearchValue < lowerBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, verticalForewardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                    }
                }

                ///////////////////////垂直滞后水平优先角落////////////////////////
                isNeedToSearch = true;

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalForewardSearchValue > rightBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalForewardSearchValue < leftBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalForewardSearchValue, verticalAfterwardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                    }
                }

                ///////////////////////垂直滞后水平滞后角落////////////////////////

                if (verticalSearchPriority == VerticalSearchPriority.top)
                {
                    if (verticalAfterwardSearchValue < lowerBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (verticalAfterwardSearchValue > upperBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (horizontalSearchPriority == HorizontalSearchPriority.right)
                {
                    if (horizontalAfterwardSearchValue < leftBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }
                else
                {
                    if (horizontalAfterwardSearchValue > rightBoundInGrid)
                    {
                        isNeedToSearch = false;
                    }
                }

                if (isNeedToSearch)
                {
                    IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(horizontalAfterwardSearchValue, verticalAfterwardSearchValue);
                    if (currentCheckedPart != null)
                    {
                        SearchNearestPointInOnePart(targetPoint, targetPartLevelSequence, currentCheckedPart);
                    }
                }

                //更新搜索边界
                leftSearchLine -= 1;
                rightSearchLine += 1;
                bottomSearchLine -= 1;
                topSearchLine += 1;

                //如果搜索线包围格子搜索边界则搜索结束
                if ((leftSearchLine < leftBoundInGrid) && (rightSearchLine > rightBoundInGrid) && (topSearchLine > upperBoundInGrid) && (bottomSearchLine < lowerBoundInGrid))
                {
                    break;
                }
            }
        }

        private bool ISTheCoordinateInsideSearchBound(int x, int y)
        {
            return (x <= rightBoundInGrid && x >= leftBoundInGrid && y <= upperBoundInGrid && y >= lowerBoundInGrid);
        }

        //private bool ISThePartInsideSearchBound(IPart currentCheckedPart)
        //{
        //    return ((currentCheckedPart.GetX()) <= rightBound) && ((currentCheckedPart.GetX()) >= leftBound)
        //                                && ((currentCheckedPart.GetY()) <= upperBoundInGrid) && ((currentCheckedPart.GetX()) >= lowerBound);
        //}

        private void SearchNearestPointInOnePart(IPosition targetPoint, int targetPartLevelSequence, IPart currentCheckedPart)
        {
            bool NearestPointIsChange = false;
            bool SearchBoundIsChange = false;

            ILevel targetPartLevel = m2mStructure.GetLevel(targetPartLevelSequence);

            #region code for algorithm demo
            if (SearchOnePartBeginAndGetSearchPartSequence != null)
            {
                SearchOnePartBeginAndGetSearchPartSequence(SearchPartSequence);                
            }

            if (GetSearchPart != null)
            {
                GetSearchPart(targetPartLevel, targetPartLevelSequence, currentCheckedPart);
            }
            #endregion

            //travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, targetPartLevelSequence, m2mStructure, targetPoint);

            //IPosition CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
            //float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();

            IPositionSet BottonLevelPositionSet =
                    m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentCheckedPart, targetPartLevelSequence);

            BottonLevelPositionSet.InitToTraverseSet();
            while (BottonLevelPositionSet.NextPosition())
            {
                IPosition comparePoint = BottonLevelPositionSet.GetPosition();

                if (GetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird != null)
                {
                    if (GetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird(targetPoint, comparePoint) == true)
                    {
                        SearchBoundIsChange = true;
                    }
                }

                if (GetOnePointAndDetermineNearestPointIsChanged != null)
                {
                    if (GetOnePointAndDetermineNearestPointIsChanged(targetPoint, comparePoint) == true)
                    {
                        NearestPointIsChange = true;
                    }
                }
            }

            if (GetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird != null)
            {
                if (SearchBoundIsChange)
                {
                    UpdateSearchBoundInGrid(targetPartLevel);
                }
            }

            if (GetOnePointAndDetermineNearestPointIsChanged != null)
            {
                if (NearestPointIsChange)
                {
                    ShrinkSearchBound(targetPoint);

                    UpdateSearchBoundInGrid(targetPartLevel);
                }
            }

            #region code for algorithm demo
            SearchPartSequence++;
            #endregion
        }

        //通过真实搜索边界更新格子搜索边界
        private void UpdateSearchBoundInGrid(ILevel targetPartLevel)
        {
            //更新格子搜索边界
            upperBoundInGrid = targetPartLevel.ConvertRealValueToPartSequenceY(upperBound);
            lowerBoundInGrid = targetPartLevel.ConvertRealValueToPartSequenceY(lowerBound);
            leftBoundInGrid = targetPartLevel.ConvertRealValueToPartSequenceX(leftBound);
            rightBoundInGrid = targetPartLevel.ConvertRealValueToPartSequenceX(rightBound);

            //判断搜索边界是否在地图边界以内
            if (upperBoundInGrid > targetPartLevel.GetUnitNumInHeight() - 1)
            {
                upperBoundInGrid = targetPartLevel.GetUnitNumInHeight() - 1;
            }

            if (lowerBoundInGrid < 0)
            {
                lowerBoundInGrid = 0;
            }

            if (rightBoundInGrid > targetPartLevel.GetUnitNumInWidth() - 1)
            {
                rightBoundInGrid = targetPartLevel.GetUnitNumInWidth() - 1;
            }

            if (leftBoundInGrid < 0)
            {
                leftBoundInGrid = 0;
            }
        }

        private void ShrinkSearchBound(IPosition targetPoint)
        {
            //更新最近距离以计算搜索边界
            CurrentNearestDistance = (float)Math.Sqrt(CurrentNearestDistanceSquare);

            //更新搜索边界
            upperBound = targetPoint.GetY() + CurrentNearestDistance;
            lowerBound = targetPoint.GetY() - CurrentNearestDistance;
            leftBound = targetPoint.GetX() - CurrentNearestDistance;
            rightBound = targetPoint.GetX() + CurrentNearestDistance;

            #region code for algorithm demo
            if (CurrentNearestPointChanged != null)
            {
                CurrentNearestPointChanged(CurrentNearestPoint);
            }

            if (SearchBoundChanged != null)
            {
                SearchBoundChanged(upperBound, lowerBound, leftBound, rightBound);
            }
            #endregion
        }

        delegate bool dGetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird(IPosition targetPoint, IPosition comparePoint);
        event dGetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird GetOnePointAndShrinkSearchBoundAndDetermineIfShinkSearchBoundInGird;
        private bool SearchNearestPointInComparedPointSetAndShrinkSearchBound(IPosition targetPoint, IPosition comparePoint)
        {
            if ((comparePoint.GetX() > leftBound) && (comparePoint.GetX() < rightBound) &&
                (comparePoint.GetY() < upperBound) && (comparePoint.GetY() > lowerBound))
            {
                #region code for algorithm demo
                if (GetComparedPoint != null)
                {
                    GetComparedPoint(comparePoint);
                }
                #endregion

                float dx = targetPoint.GetX() - comparePoint.GetX();
                float dy = targetPoint.GetY() - comparePoint.GetY();

                float currentDistanceSquare = dx * dx + dy * dy;

                if (currentDistanceSquare < CurrentNearestDistanceSquare)
                {
                    CurrentNearestDistanceSquare = currentDistanceSquare;
                    CurrentNearestPoint = comparePoint;

                    //遍历一个格子里面的点的时候,遇到比当前最近点更近的点，则缩小搜索边界
                    ShrinkSearchBound(targetPoint);
                    return true;
                }
            }
            return false;
        }

        delegate bool dGetOnePointAndDetermineNearestPointIsChanged(IPosition targetPoint, IPosition comparePoint);
        event dGetOnePointAndDetermineNearestPointIsChanged GetOnePointAndDetermineNearestPointIsChanged;
        private bool SearchNearestPointInComparedPointSet(IPosition targetPoint, IPosition comparePoint)
        {
            if ((comparePoint.GetX() > leftBound) && (comparePoint.GetX() < rightBound) &&
                (comparePoint.GetY() < upperBound) && (comparePoint.GetY() > lowerBound))
            {
                #region code for algorithm demo
                if (GetComparedPoint != null)
                {
                    GetComparedPoint(comparePoint);
                }
                #endregion

                float dx = targetPoint.GetX() - comparePoint.GetX();
                float dy = targetPoint.GetY() - comparePoint.GetY();

                float currentDistanceSquare = dx * dx + dy * dy;

                if (currentDistanceSquare < CurrentNearestDistanceSquare)
                {
                    CurrentNearestDistanceSquare = currentDistanceSquare;
                    CurrentNearestPoint = comparePoint;
                    return true;

                    //遍历一个格子里面的点的时候，并不把搜索边界缩小
                }
            }
            return false;
        }        

        SortedList<float, IPosition> KNearestPointSList;
        private bool SearchKNearestPointInComparedPointSet(IPosition targetPoint, IPosition comparePoint)
        {
            if ((comparePoint.GetX() > leftBound) && (comparePoint.GetX() < rightBound) &&
                (comparePoint.GetY() < upperBound) && (comparePoint.GetY() > lowerBound))
            {
                float dx = targetPoint.GetX() - comparePoint.GetX();
                float dy = targetPoint.GetY() - comparePoint.GetY();

                float currentDistanceSquare = dx * dx + dy * dy;

                if (KNearestPointSList.Count >= KNearestPointSList.Capacity)
                {
                    KNearestPointSList.RemoveAt(KNearestPointSList.Count - 1);

                    if (currentDistanceSquare < CurrentNearestDistanceSquare)
                    {
                        while (KNearestPointSList.ContainsKey(currentDistanceSquare))
                        {
                            currentDistanceSquare += 0.001f;
                        }
                        KNearestPointSList.Add(currentDistanceSquare, comparePoint);

                        CurrentNearestDistanceSquare = KNearestPointSList.Keys[KNearestPointSList.Count - 1];
                        CurrentNearestPoint = KNearestPointSList.Values[KNearestPointSList.Count - 1];
                        return true;
                        //遍历一个格子里面的点的时候，并不把搜索边界缩小
                    }
                }
                else
                {
                    while (KNearestPointSList.ContainsKey(currentDistanceSquare))
                    {
                        currentDistanceSquare += 0.001f;
                    }
                    KNearestPointSList.Add(currentDistanceSquare, comparePoint);

                    CurrentNearestDistanceSquare = KNearestPointSList.Keys[KNearestPointSList.Count - 1];
                    CurrentNearestPoint = KNearestPointSList.Values[KNearestPointSList.Count - 1];
                    return false;
                    //遍历一个格子里面的点的时候，并不把搜索边界缩小
                }
            }
            return false;
        }

        //private void SearchNearestPointInOnePart_Old(IPosition targetPoint, int targetPartLevelSequence, IPart currentCheckedPart)
        //{
        //    ILevel targetPartLevel = m2mStructure.GetLevel(targetPartLevelSequence);

        //    travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, targetPartLevelSequence, m2mStructure, targetPoint);

        //    IPosition CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
        //    float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();

        //    //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并更新最大搜索边界
        //    if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
        //    {
        //        CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
        //        CurrentNearestPoint = CurrentPartNearestPoint;

        //        CurrentNearestDistance = (float)Math.Sqrt(CurrentNearestDistanceSquare);

        //        //更新搜索边界
        //        upperBoundInGrid = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() + CurrentNearestDistance);
        //        lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() - CurrentNearestDistance);
        //        leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() - CurrentNearestDistance);
        //        rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() + CurrentNearestDistance);

        //        //判断搜索边界是否在地图边界以内
        //        if (upperBoundInGrid > targetPartLevel.GetUnitNumInHeight() - 1)
        //        {
        //            upperBoundInGrid = targetPartLevel.GetUnitNumInHeight() - 1;
        //        }

        //        if (lowerBound < 0)
        //        {
        //            lowerBound = 0;
        //        }

        //        if (rightBound > targetPartLevel.GetUnitNumInWidth() - 1)
        //        {
        //            rightBound = targetPartLevel.GetUnitNumInWidth() - 1;
        //        }

        //        if (leftBound < 0)
        //        {
        //            leftBound = 0;
        //        }
        //    }
        //}

        //private static void SearchNearestPointOnOneSide(IPosition targetPoint, ref IPosition CurrentNearestPoint, ref float CurrentNearestDistanceSquare, ref float CurrentNearestDistance, TravelThePointInPart travelThePointInPart, ILevel targetPartLevel, int leftSearchLine, int rightSearchLine, ref int upperBoundInGrid, ref int lowerBound, ref int leftBound, ref int rightBound)
        //{
        //    for (int j = leftSearchLine + 1; j <= rightSearchLine - 1; j++)
        //    {
        //        Part currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, verticalForewardSearchValue);
        //        if (currentCheckedPart != null)
        //        {
        //            travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, targetPoint);

        //            Position CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
        //            float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();

        //            //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并更新最大搜索边界
        //            if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
        //            {
        //                CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
        //                CurrentNearestPoint = CurrentPartNearestPoint;

        //                CurrentNearestDistance = (float)Math.Sqrt((double)CurrentNearestDistanceSquare);

        //                //更新搜索边界
        //                upperBoundInGrid = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() + CurrentNearestDistance);
        //                lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(targetPoint.GetY() - CurrentNearestDistance);
        //                leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() - CurrentNearestDistance);
        //                rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(targetPoint.GetX() + CurrentNearestDistance);
        //            }
        //        }
        //    }
        //}

        //private void DetermineWaitToSearchPartList(IPosition point, float CurrentNearestDistanceSquare, ILevel targetPartLevel)
        //{
        //    float CurrentNearestDistance = (float)Math.Sqrt(CurrentNearestDistanceSquare);

        //    int upperBoundInGrid = targetPartLevel.ConvertRealityValueToPartSequenceY(point.GetY() + CurrentNearestDistance);
        //    int lowerBound = targetPartLevel.ConvertRealityValueToPartSequenceY(point.GetY() - CurrentNearestDistance);
        //    int leftBound = targetPartLevel.ConvertRealityValueToPartSequenceX(point.GetX() - CurrentNearestDistance);
        //    int rightBound = targetPartLevel.ConvertRealityValueToPartSequenceX(point.GetX() + CurrentNearestDistance);

        //    WaitToSearchPartList.Clear();

        //    for (int i = lowerBound; i <= upperBoundInGrid; i++)
        //    {
        //        for (int j = leftBound; j <= rightBound; j++)
        //        {
        //            if (!targetPartLevel.IndexIsExceedPartRange(j, i))
        //            {
        //                IPart currentCheckedPart = targetPartLevel.GetPartRefByPartIndex(j, i);
        //                if ((currentCheckedPart != null) && (!SearchEverPartList.Contains(currentCheckedPart)))
        //                {
        //                    WaitToSearchPartList.Add(currentCheckedPart);
        //                }
        //            }
        //        }
        //    }
        //}

        
//public IPosition NearestNeighbor2(IPosition point)
        //{
        //    //初始化变量
        //    SearchEverPartList.Clear();
        //    IPosition CurrentNearestPoint;

        //    float CurrentNearestDistanceSquare = float.MaxValue;
        //    TravelThePointInPart travelThePointInPart = new TravelThePointInPart();


        //    Part targetPart = null;
        //    Level targetPartLevel = SearchTheTargetPart(point, ref targetPart);

        //    travelThePointInPart.InitToGetTheNearestPointInPart(targetPart, point);
        //    CurrentNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
        //    CurrentNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();
        //    SearchEverPartList.Add(targetPart);

        //    while (true)
        //    {
        //        DetermineWaitToSearchPartList(point, CurrentNearestDistanceSquare, targetPartLevel);

        //        //(未实现)
        //        //把WaitToSearchPartList每个分块按该分块的代表点到考察点的距离做一次冒泡,距离越小的越早被选中.
        //        //如果最小距离比当前最小距离上限小,应该更新最小距离上限,并更新最小距离点,并重新画出搜索正方形,重新开始搜索.

        //        foreach (Part currentCheckedPart in WaitToSearchPartList)
        //        {
        //            travelThePointInPart.InitToGetTheNearestPointInPart(currentCheckedPart, point);

        //            IPosition CurrentPartNearestPoint = travelThePointInPart.GetTheNearestPointInPart();
        //            float CurrentPartNearestDistanceSquare = travelThePointInPart.GetNearestDistanceSquare();
        //            SearchEverPartList.Add(currentCheckedPart);

        //            //如果在该分块里面找到的点的距离比原来的最短距离上限小,则更新最小距离上限,更新最小距离点,并重新画出搜索正方形,重新开始搜索.
        //            if (CurrentPartNearestDistanceSquare < CurrentNearestDistanceSquare)
        //            {
        //                CurrentNearestDistanceSquare = CurrentPartNearestDistanceSquare;
        //                CurrentNearestPoint = CurrentPartNearestPoint;
        //                continue;
        //            }
        //        }

        //        break;
        //    }

        //    return CurrentNearestPoint;
        //}
    }
}
