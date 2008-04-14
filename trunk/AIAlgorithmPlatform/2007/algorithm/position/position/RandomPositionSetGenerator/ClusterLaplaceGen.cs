using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;
using Troschuetz;
using RandomMakerAlgorithm;
using Position_Interface;
using Configuration;
using Position_Implement;

namespace RandomPositionSetGenerator
{
    class ClusterLaplaceGen:IRandomGenerator
    {
        int clusterPointNum = 10;

        public int ClusterPointNum
        {
            get { return clusterPointNum; }
            set { clusterPointNum = value; }
        }
        double X_Alpha = 4;

        public double X_Alpha1
        {
            get { return X_Alpha; }
            set { X_Alpha = value; }
        }
        double Y_Alpha = 4;

        public double Y_Alpha1
        {
            get { return Y_Alpha; }
            set { Y_Alpha = value; }
        }
        float minMu;
        float maxMu;
        int pointNum;
        PositionSetEditSet positionSetEditSet = new PositionSetEditSet();

        public ClusterLaplaceGen(float minMu, float maxMu, int pointNum)
        {
            this.maxMu = maxMu;
            this.minMu = minMu;
            this.pointNum = pointNum;
        }

        #region IRandomGenerator ≥…‘±

        public PositionSetEditSet getRandomPositionSet(int pointNum)
        {
            unchecked
            {
                int seed = (int) DateTime.Now.Ticks;
                this.pointNum = pointNum;
                LaplaceDistribution distributionX = new LaplaceDistribution(new StandardGenerator(seed++));
                distributionX.Alpha = X_Alpha;

                LaplaceDistribution distributionY = new LaplaceDistribution(new StandardGenerator(seed++));
                distributionY.Alpha = Y_Alpha;

                for (int i = 0; i < clusterPointNum; i++)
                {
                    distributionX.Mu = RandomMaker.RapidBetween(minMu, maxMu);
                    distributionY.Mu = RandomMaker.RapidBetween(minMu, maxMu);
                    RandomPositionSet randomPositionSet =
                        new RandomPositionSet((int) (pointNum/clusterPointNum), 1000, distributionX, distributionY);
                    positionSetEditSet.AddPositionSet(randomPositionSet);
                }
            }
            return positionSetEditSet;
        }

        #endregion
    }
}
