using System;
using System.Collections.Generic;
using System.Text;
using ConvexHullEngine;
using Position_Interface;
using QuickHullAlgorithm;
using WriteLineInGridEngine;
using BlockLineAlgorithm;
using Position_Implement;
using System.Diagnostics;

namespace M2M
{
    public class M2M_CD
    {
        #region code for algorithm demo
        public event dGetM2MStructure GetM2MStructure;
        public event dGetPositionSet GetPositionSetToGetConvexHull;

        public event dCollision GetCollision;
        public event dNoCollision GetNoCollision;
        public event dGetIntersectPart GetIntersectPart;
        
        #endregion

        IM2MStructure m2mStructure = null;

        int levelNum=3;
        public int NumOfLevel
        {
            get { return levelNum; }
            set { levelNum = value; }
        }

        
        public void PreProcess(PositionSetEditSet pointList)
        {
            #region code for algorithm demo
            if (GetPositionSetToGetConvexHull != null)
            {
                GetPositionSetToGetConvexHull(pointList);
            }
            #endregion

            //请在这里改变不同的m2mStructure实现。
            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();

            m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(pointList,levelNum);

            PositionSetEdit_ImplementByICollectionTemplate positionSet = new PositionSetEdit_ImplementByICollectionTemplate();

            

            ILevel bottomLevel = m2mStructure.GetLevel(m2mStructure.GetLevelNum() - 1);
            
            for (int i = 0; i < pointList.GetPositionSetNum(); i++)
            {
                IPosition start = new Position_Point();
                IPosition end = new Position_Point();
                IPosition first = new Position_Point();
              
                IPositionSet positionListTemp = pointList.GetNthPositionSet(i);
                
                positionListTemp.InitToTraverseSet();
                if (positionListTemp.NextPosition())
                {
                    start = new Position_Point(bottomLevel.ConvertRealValueToRelativeValueX(positionListTemp.GetPosition().GetX()),bottomLevel.ConvertRealValueToRelativeValueY(positionListTemp.GetPosition().GetY()));
                    first = start;
                    positionSet.AddPosition(start);
                }
                while (positionListTemp.NextPosition())
                {
                    end = new Position_Point(bottomLevel.ConvertRealValueToRelativeValueX(positionListTemp.GetPosition().GetX()), bottomLevel.ConvertRealValueToRelativeValueY(positionListTemp.GetPosition().GetY()));
                    IPositionSet positionSetTemp = writeLineInGridEngine.WriteLineInGrid(bottomLevel.GetGridWidth(), bottomLevel.GetGridHeight(), start, end);
                    if (positionSetTemp != null)
                    {
                        positionSetTemp.InitToTraverseSet();
                        while (positionSetTemp.NextPosition())
                            positionSet.AddPosition(positionSetTemp.GetPosition());
                    }
                    start = end;
                    positionSet.AddPosition(start);
                }
                IPositionSet positionSetTemp2 = writeLineInGridEngine.WriteLineInGrid(bottomLevel.GetGridWidth(), bottomLevel.GetGridHeight(), end, first);        
                if (positionSetTemp2 != null)
                {
                    positionSetTemp2.InitToTraverseSet();
                    while (positionSetTemp2.NextPosition())
                        positionSet.AddPosition(positionSetTemp2.GetPosition());
                }

            }

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

        //private IConvexHullEngine convexHullEngine = new QuickHull();

        private IWriteLineInGridEngine writeLineInGridEngine = new Line2Block();


        IPositionSetEdit tempIntersectPositionSet = new PositionSetEdit_ImplementByICollectionTemplate();

        public bool Collide(IPositionSet objPositionSet)
        {
            IPosition start = new Position_Point();
            IPosition end = new Position_Point();
            IPosition first = new Position_Point();
            ILevel currentLevel = new Level();
            
            for (int i = 1; i < m2mStructure.GetLevelNum(); i++)
            {
                currentLevel=m2mStructure.GetLevel(i);
                objPositionSet.InitToTraverseSet();
                //判断第一点所在的分块是否相交
                if (objPositionSet.NextPosition())
                {
                    start = new Position_Point(currentLevel.ConvertRealValueToRelativeValueX(objPositionSet.GetPosition().GetX()), currentLevel.ConvertRealValueToRelativeValueY(objPositionSet.GetPosition().GetY()));
                    first = start;
                    if (currentLevel.GetPartRefByPoint(start) != null)
                    {
                        if (i < m2mStructure.GetLevelNum() - 1) goto NextLevel;
                        else { tempIntersectPositionSet.AddPosition(new Position_Point(currentLevel.ConvertRelativeValueToPartSequenceX(start.GetX()), currentLevel.ConvertRelativeValueToPartSequenceY(start.GetY()))); }
                    }
                }


                while (objPositionSet.NextPosition())
                {
                    //判断每条边所在的分块是否相交
                    end = new Position_Point(currentLevel.ConvertRealValueToRelativeValueX(objPositionSet.GetPosition().GetX()), currentLevel.ConvertRealValueToRelativeValueY(objPositionSet.GetPosition().GetY()));
                    IPositionSet positionSetTemp = writeLineInGridEngine.WriteLineInGrid(currentLevel.GetGridWidth(), currentLevel.GetGridHeight(), start, end);

                    if (positionSetTemp != null)
                    {
                        positionSetTemp.InitToTraverseSet();
                        while (positionSetTemp.NextPosition())
                            if (currentLevel.GetPartRefByPoint(positionSetTemp.GetPosition()) != null)
                            {
                                if (i < m2mStructure.GetLevelNum() - 1) goto NextLevel;
                                else { tempIntersectPositionSet.AddPosition(new Position_Point(currentLevel.ConvertRelativeValueToPartSequenceX(positionSetTemp.GetPosition().GetX()), currentLevel.ConvertRelativeValueToPartSequenceY(positionSetTemp.GetPosition().GetY()))); }
                            }
                    }

                    //判断每条边的终点所在的分块是否相交
                    start = end;
                    if (currentLevel.GetPartRefByPoint(start) != null)
                    {
                        if (i < m2mStructure.GetLevelNum() - 1) goto NextLevel;
                        else { tempIntersectPositionSet.AddPosition(new Position_Point(currentLevel.ConvertRelativeValueToPartSequenceX(start.GetX()), currentLevel.ConvertRelativeValueToPartSequenceY(start.GetY()))); }
                    }
                }

                //判断最后一条边所在的分块是否相交
                IPositionSet positionSetTemp2 = writeLineInGridEngine.WriteLineInGrid(currentLevel.GetGridWidth(), currentLevel.GetGridHeight(), end, first);
                if (positionSetTemp2 != null)
                {
                    positionSetTemp2.InitToTraverseSet();
                    while (positionSetTemp2.NextPosition())
                        if (currentLevel.GetPartRefByPoint(positionSetTemp2.GetPosition()) != null)
                        {
                            if (i < m2mStructure.GetLevelNum() - 1) goto NextLevel;
                            else { tempIntersectPositionSet.AddPosition(new Position_Point(currentLevel.ConvertRelativeValueToPartSequenceX(positionSetTemp2.GetPosition().GetX()), currentLevel.ConvertRelativeValueToPartSequenceY(positionSetTemp2.GetPosition().GetY()))); }
                        }
                }

                if (i == m2mStructure.GetLevelNum() - 1&&tempIntersectPositionSet.GetNum() > 0) goto NextLevel;

                #region code for algorithm demo
                if (GetNoCollision != null)
                {
                    GetNoCollision(objPositionSet);
                    Debug.WriteLine(i);
                }
                #endregion

                return false;//free of collision

            NextLevel: ;
            }

            #region code for algorithm demo
            if (GetCollision != null)
            {
                GetCollision(objPositionSet);
            }
            if (GetIntersectPart != null)
            {
                GetIntersectPart(currentLevel, tempIntersectPositionSet);
            }
            #endregion
            tempIntersectPositionSet.Clear();

            return true;

        }


        public IPositionSet CollisionDetection(PositionSetEditSet bgPositionSet,IPositionSetEdit objPositionSet)
        {
            PreProcess(bgPositionSet);
            float delta = 0;
            while (true)
            {
                
                IPositionSetEdit obj = new PositionSetEdit_ImplementByICollectionTemplate();
                objPositionSet.InitToTraverseSet();
                while (objPositionSet.NextPosition())
                {
                    obj.AddPosition(new Position_Point(objPositionSet.GetPosition().GetX(), objPositionSet.GetPosition().GetY() - delta));
                }
                Collide(obj);
                delta += 0.2f;
            }
            return null;
            //return QueryConvexHull();
        }


    }
}