using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace M2M
{
    //M2M_PreProcessor设计只用于预处理M2MStructure_General这种M2M数据结构
    public class M2MSCreater_ForGeneralM2MStruture
    {
        //最底层的最大点密度
        double set_pointInPartFactor = 33;

        //上一层分块包含下一层分块数目的开平方
        int set_UnitNumInGridLength = 2;

        //如果地图尺寸太大，或者分块太细可能出现问题，应该调整bias使得更大
        const float set_Bias = 1.00001f;

        float MapWidth;
        float MapHeight;

        float MaxGridLength;
        int EffectivelevelNum;

        public Type PartType = typeof(Part);

        List<QueryTable> QueryTableList = new List<QueryTable>();

        List<Level> LevelList = new List<Level>();

        Position_Point RelativelyPoint = new Position_Point();

        Level GetLevel(int num)
        {
            return LevelList[num];
        }

        int GetLevelNum()
        {
            return LevelList.Count;
        }

        public void Init(float leftestX, float lowestY, float MapWidth, float MapHeight, float MaxGridLength, int UnitNumInLength, int effectivelevelNum)
        {
            this.MapWidth = MapWidth;
            this.MapHeight = MapHeight;

            this.MaxGridLength = MaxGridLength;
            this.set_UnitNumInGridLength = UnitNumInLength;
            this.EffectivelevelNum = effectivelevelNum;

            RelativelyPoint.SetX(leftestX);
            RelativelyPoint.SetY(lowestY);

            InitMemory();
        }

        private void InitMemory()
        {
            LevelList.Clear();

            int MaxGridUnitNumInWidth = (int)Math.Ceiling(MapWidth / MaxGridLength);
            int MaxGridUnitNumInHeight = (int)Math.Ceiling(MapHeight / MaxGridLength);


            //创建level0
            Level level0 = new Level();
            level0.SetUnitNumInWidth(1);
            level0.SetUnitNumInHeight(1);
            level0.SetGridWidth(MaxGridUnitNumInWidth * MaxGridLength);
            level0.SetGridHeight(MaxGridUnitNumInHeight * MaxGridLength);
            level0.SetRelativelyPoint(RelativelyPoint);
            LevelList.Add(level0);

            //创建level1
            Level level1 = new Level();
            level1.SetUnitNumInWidth(MaxGridUnitNumInWidth);
            level1.SetUnitNumInHeight(MaxGridUnitNumInHeight);
            level1.SetGridWidth(MaxGridLength);
            level1.SetGridHeight(MaxGridLength);
            level1.SetRelativelyPoint(RelativelyPoint);
            LevelList.Add(level1);

            for (int levelSequence = 1; levelSequence < EffectivelevelNum; levelSequence++)
            {
                Level level = new Level();
                level.SetUnitNumInWidth(set_UnitNumInGridLength * LevelList[levelSequence].GetUnitNumInWidth());
                level.SetUnitNumInHeight(set_UnitNumInGridLength * LevelList[levelSequence].GetUnitNumInHeight());
                //level.SetGridWidth(LevelList[levelSequence].GetGridWidth() / set_UnitNumInGridLength + float.Epsilon + set_Bias);
                //level.SetGridHeight(LevelList[levelSequence].GetGridHeight() / set_UnitNumInGridLength + float.Epsilon + set_Bias);
                level.SetGridWidth(LevelList[0].GetGridWidth() / level.GetUnitNumInWidth());
                level.SetGridHeight(LevelList[0].GetGridHeight() / level.GetUnitNumInHeight());
                level.SetRelativelyPoint(RelativelyPoint);
                LevelList.Add(level);
            }

            foreach (Level level in LevelList)
            {
                level.QueryTable = new QueryTableByArray(PartType);
                level.AllocateMemory();
            }
        }

        public M2MStructure_General Create()
        {
            return new M2MStructure_General(LevelList);
        }

        public M2MStructure_General CreateAutomatically(IPositionSet positionSet)
        {
            int PointNum = 0;

            float leftestX = 0;
            float rightestX = 0;
            float lowestY = 0;
            float highestY = 0;

            positionSet.InitToTraverseSet();
            if (positionSet.NextPosition())
            {
                leftestX = positionSet.GetPosition().GetX();
                rightestX = positionSet.GetPosition().GetX();
                lowestY = positionSet.GetPosition().GetY();
                highestY = positionSet.GetPosition().GetY();

                PointNum++;
            }

            while (positionSet.NextPosition())
            {
                if (positionSet.GetPosition().GetX() > rightestX)
                {
                    rightestX = positionSet.GetPosition().GetX();
                }
                else if (positionSet.GetPosition().GetX() < leftestX)
                {
                    leftestX = positionSet.GetPosition().GetX();
                }

                if (positionSet.GetPosition().GetY() > highestY)
                {
                    highestY = positionSet.GetPosition().GetY();
                }
                else if (positionSet.GetPosition().GetY() < lowestY)
                {
                    lowestY = positionSet.GetPosition().GetY();
                }

                PointNum++;
            }

            if (PointNum == 0)
            {
                throw new Exception("PointNum == 0");
            }

            float mapWidth = (rightestX - leftestX) * set_Bias;
            float mapHeight = (highestY - lowestY) * set_Bias;
            
            int MicPartNumInMacPart = set_UnitNumInGridLength * set_UnitNumInGridLength;

            float maxGridLength;
            
            if (mapWidth > mapHeight)
            {
                maxGridLength = mapHeight / set_UnitNumInGridLength + float.Epsilon;
            }
            else
            {
                maxGridLength = mapWidth / set_UnitNumInGridLength + float.Epsilon;
            }

            Init(leftestX, lowestY, mapWidth, mapHeight, maxGridLength,
                set_UnitNumInGridLength, CalculateEffectiveLevelNum(PointNum, MicPartNumInMacPart));

            return Create();
        }

        public M2MStructure_General CreateAutomatically(IPositionSet positionSet,int level)
        {
            int PointNum = 0;

            float leftestX = 0;
            float rightestX = 0;
            float lowestY = 0;
            float highestY = 0;

            positionSet.InitToTraverseSet();
            if (positionSet.NextPosition())
            {
                leftestX = positionSet.GetPosition().GetX();
                rightestX = positionSet.GetPosition().GetX();
                lowestY = positionSet.GetPosition().GetY();
                highestY = positionSet.GetPosition().GetY();

                PointNum++;
            }

            while (positionSet.NextPosition())
            {
                if (positionSet.GetPosition().GetX() > rightestX)
                {
                    rightestX = positionSet.GetPosition().GetX();
                }
                else if (positionSet.GetPosition().GetX() < leftestX)
                {
                    leftestX = positionSet.GetPosition().GetX();
                }

                if (positionSet.GetPosition().GetY() > highestY)
                {
                    highestY = positionSet.GetPosition().GetY();
                }
                else if (positionSet.GetPosition().GetY() < lowestY)
                {
                    lowestY = positionSet.GetPosition().GetY();
                }

                PointNum++;
            }

            if (PointNum == 0)
            {
                throw new Exception("PointNum == 0");
            }

            float mapWidth = (rightestX - leftestX) * set_Bias;
            float mapHeight = (highestY - lowestY) * set_Bias;

            int MicPartNumInMacPart = set_UnitNumInGridLength * set_UnitNumInGridLength;

            float maxGridLength;

            if (mapWidth > mapHeight)
            {
                maxGridLength = mapHeight / set_UnitNumInGridLength + float.Epsilon;
            }
            else
            {
                maxGridLength = mapWidth / set_UnitNumInGridLength + float.Epsilon;
            }

            Init(leftestX, lowestY, mapWidth, mapHeight, maxGridLength,
                set_UnitNumInGridLength, level);

            return Create();
        }        


        public void SetPointInPartFactor(double factor)
        {
            set_pointInPartFactor = factor;
        }

        public void SetUnitNumInGridLength(int num)
        {
            set_UnitNumInGridLength = num;
        }

        private int CalculateEffectiveLevelNum(int PointNum, int MicPartNumInMacPart)
        {
            return (int)Math.Ceiling(Math.Log(PointNum / set_pointInPartFactor, MicPartNumInMacPart));
        }
    }
}
