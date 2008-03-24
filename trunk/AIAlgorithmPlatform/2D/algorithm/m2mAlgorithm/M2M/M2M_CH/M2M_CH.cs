using System;
using System.Collections.Generic;
using System.Text;
using ConvexHullEngine;
using Position_Interface;
using QuickHullAlgorithm;
using WriteLineInGridEngine;
using BlockLineAlgorithm;
using Position_Implement;

namespace M2M
{
    public class M2M_CH : IConvexHullEngine
    {
        #region code for algorithm demo
        public event dGetM2MStructure GetM2MStructure;
        public event dGetPositionSet GetPositionSetToGetConvexHull;
        public event dGetPositionSetInSpecificLevel GetChildPositionSetInSpecificLevelOfConvexHull;
        public event dGetPositionSetInSpecificLevel GetConvexHullPositionSetInSpecificLevel;
        public event dGetPositionSetInSpecificLevel GetLinePositionSetInSpecificLevel;
        public event dGetPositionSet GetRealConvexHull;
        public event dGetPositionSetInSpecificLevel GetRepresentativeHullInSpecificLevel;
        #endregion

        IM2MStructure m2mStructure = null;

        double set_pointInPartFactor = 333;

        public double TheDensityOfBottomLevel
        {
            get { return set_pointInPartFactor; }
            set { set_pointInPartFactor = value; }
        }

        public void PreProcess(List<IPosition> pointList)
        {
            PositionSetEdit_ImplementByICollectionTemplate positionSet = new PositionSetEdit_ImplementByICollectionTemplate(pointList);

            PreProcess(positionSet);
        }

        public void PreProcess(IPositionSet positionSet)
        {
            #region code for algorithm demo
            if (GetPositionSetToGetConvexHull != null)
            {
                GetPositionSetToGetConvexHull(positionSet);
            }
            #endregion

            //请在这里改变不同的m2mStructure实现。
            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();

            m2m_Creater_ForGeneralM2MStruture.SetPointInPartFactor(set_pointInPartFactor);

            m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(positionSet);

            m2mStructure.Preprocessing(positionSet);

            #region code for algorithm demo
            if (GetM2MStructure != null)
            {
                GetM2MStructure(m2mStructure);
            }
            #endregion
        }

        public void Insert(IPosition point)
        {
            m2mStructure.Insert(point);
        }
        public void Remove(IPosition point)
        {
            m2mStructure.Remove_NotTestYet(point);
        }

        private IConvexHullEngine convexHullEngine = new QuickHull();

        private IWriteLineInGridEngine writeLineInGridEngine = new BlockLine();

        private IPositionSet childPositionSetOfPart;

        List<IPosition> tempChildPositionList = new List<IPosition>();

        public IPositionSet ConvexHull(IPositionSet positionSet)
        {
            PreProcess(positionSet);
            return QueryConvexHull();
        }

        #region code for algorithm demo
        IPositionSetEdit linePositionSetInSpecificLevel;
        IPositionSetEdit representativeHull;
        #endregion

        public IPositionSet QueryConvexHull()
        {
            #region code for algorithm demo
            if (GetChildPositionSetInSpecificLevelOfConvexHull != null)
            {
                GetChildPositionSetInSpecificLevelOfConvexHull(m2mStructure.GetLevel(1), 1, m2mStructure.GetChildPositionSetByParentPart(0, m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0)));
            }
            #endregion

            IPositionSet PositionSetInConvexHull = convexHullEngine.ConvexHull(
                m2mStructure.GetChildPositionSetByParentPart(0, m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0)));

            for (int i = 1; i < m2mStructure.GetLevelNum(); i++)
            {
                ILevel currentLevel = m2mStructure.GetLevel(i);

                #region code for algorithm demo
                if (GetConvexHullPositionSetInSpecificLevel != null)
                {
                    GetConvexHullPositionSetInSpecificLevel(currentLevel, i, PositionSetInConvexHull);
                }
                #endregion

                tempChildPositionList = new List<IPosition>();

                tempChildPositionList.Capacity = 4 * PositionSetInConvexHull.GetNum();

                PositionSetInConvexHull.InitToTraverseSet();

                IPosition hullStartPoint = null;
                IPosition temp1 = null;
                IPosition temp2 = null;

                if (PositionSetInConvexHull.NextPosition())
                {
                    temp1 = PositionSetInConvexHull.GetPosition();
                    hullStartPoint = temp1;
                    AddChildPositionSetToList(i, temp1);

                    #region code for algorithm demo
                    if (GetLinePositionSetInSpecificLevel != null)
                    {
                        linePositionSetInSpecificLevel = new PositionSetEdit_ImplementByICollectionTemplate();
                        linePositionSetInSpecificLevel.AddPosition(new Position_Point(temp1.GetX(), temp1.GetY()));
                    }
                    #endregion
                }

                #region code for algorithm demo
                if (GetRepresentativeHullInSpecificLevel != null)
                {
                    representativeHull = new PositionSetEdit_ImplementByICollectionTemplate();
                }
                #endregion
                
                bool isWaitingLastPoint = true;

                //遍历该层凸包的每个分块，以确定下一层的候选分块
                while (true)
                {
                    if (isWaitingLastPoint)
                    {
                        if (PositionSetInConvexHull.NextPosition() == false)
                        {
                            isWaitingLastPoint = false;
                            temp2 = hullStartPoint;
                        }
                        else
                        {
                            temp2 = PositionSetInConvexHull.GetPosition();
                            AddChildPositionSetToList(i, temp2);                            
                        }
                    }
                    else
                    {
                        break;
                    }

                    IPosition start = new Position_Point(currentLevel.ConvertRealValueToRelativeValueX(((IPart)temp1).GetRandomOneFormDescendantPoint().GetX()), currentLevel.ConvertRealValueToRelativeValueY(((IPart)temp1).GetRandomOneFormDescendantPoint().GetY()));
                    IPosition end = new Position_Point(currentLevel.ConvertRealValueToRelativeValueX(((IPart)temp2).GetRandomOneFormDescendantPoint().GetX()), currentLevel.ConvertRealValueToRelativeValueY(((IPart)temp2).GetRandomOneFormDescendantPoint().GetY()));

                    IPositionSet tempPositionSet = writeLineInGridEngine.WriteLineInGrid(currentLevel.GetGridWidth(), currentLevel.GetGridHeight(), start, end);

                    #region code for algorithm demo
                    if (GetRepresentativeHullInSpecificLevel != null)
                    {
                        representativeHull.AddPosition(start);
                        representativeHull.AddPosition(end);
                        GetRepresentativeHullInSpecificLevel(m2mStructure.GetLevel(i), i, representativeHull);
                    }
                    #endregion

                    tempPositionSet.InitToTraverseSet();

                    while (tempPositionSet.NextPosition())
                    {
                        IPosition tempPosition = tempPositionSet.GetPosition();

                        IPart tempPart = currentLevel.GetPartRefByPartIndex(currentLevel.ConvertRelativeValueToPartSequenceX(tempPosition.GetX()), 
                            currentLevel.ConvertRelativeValueToPartSequenceY(tempPosition.GetY()));

                        #region code for algorithm demo
                        if (GetLinePositionSetInSpecificLevel != null)
                        {
                            linePositionSetInSpecificLevel.AddPosition(new Position_Point(currentLevel.ConvertRelativeValueToPartSequenceX(tempPosition.GetX())
                                , currentLevel.ConvertRelativeValueToPartSequenceY(tempPosition.GetY())));
                        }
                        #endregion

                        if (tempPart != null)
                        {
                            AddChildPositionSetToList(i, tempPart);
                        }
                    }
                                        
                    #region code for algorithm demo
                    if (GetLinePositionSetInSpecificLevel != null)
                    {
                        if (isWaitingLastPoint)
                        {
                            linePositionSetInSpecificLevel.AddPosition(new Position_Point(temp2.GetX(), temp2.GetY()));
                        }
                    }
                    #endregion

                    #region code for algorithm demo
                    if (GetLinePositionSetInSpecificLevel != null)
                    {
                        GetLinePositionSetInSpecificLevel(m2mStructure.GetLevel(i), i, linePositionSetInSpecificLevel);
                    }
                    #endregion

                    temp1 = temp2;
                }

                IPositionSet ChildPositionSetInConvexHull = new PositionSetEdit_ImplementByICollectionTemplate(tempChildPositionList);

                #region code for algorithm demo
                if (GetChildPositionSetInSpecificLevelOfConvexHull != null)
                {
                    if (i < m2mStructure.GetLevelNum() - 1)
                    {
                        GetChildPositionSetInSpecificLevelOfConvexHull(m2mStructure.GetLevel(i + 1), i+1, ChildPositionSetInConvexHull);
                    }
                }
                #endregion

                PositionSetInConvexHull =
                    convexHullEngine.ConvexHull(ChildPositionSetInConvexHull);
            }

            #region code for algorithm demo
            if (GetRealConvexHull != null)
            {
                GetRealConvexHull(PositionSetInConvexHull);
            }
            #endregion

            return PositionSetInConvexHull;
        }

        private void AddChildPositionSetToList(int parentPartLevelSequence, IPosition parentPart)
        {
            childPositionSetOfPart = m2mStructure.GetChildPositionSetByParentPart(parentPartLevelSequence, (IPart)parentPart);
            
            //得到凸包上的点的子分块集合
            childPositionSetOfPart.InitToTraverseSet();
            while (childPositionSetOfPart.NextPosition())
            {
                tempChildPositionList.Add(childPositionSetOfPart.GetPosition());
            }
        }
    }
}