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
    class ClusterGaussianGen : IRandomGenerator
    {
        int clusterPointNum = 10;

        public int ClusterPointNum
        {
            get { return clusterPointNum; }
            set { clusterPointNum = value; }
        }
        double X_Sigma = 8;

        public double X_Sigma1
        {
            get { return X_Sigma; }
            set { X_Sigma = value; }
        }
        double Y_Sigma = 8;

        public double Y_Sigma1
        {
            get { return Y_Sigma; }
            set { Y_Sigma = value; }
        }
        float minMu;
        float maxMu;
        int pointNum;
        PositionSetEditSet positionSetEditSet = new PositionSetEditSet();

        public ClusterGaussianGen(float minMu, float maxMu, int pointNum)
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
                NormalDistribution distributionX = new NormalDistribution(new StandardGenerator(seed++));
                distributionX.Sigma = X_Sigma;

                NormalDistribution distributionY = new NormalDistribution(new StandardGenerator(seed++));
                distributionY.Sigma = Y_Sigma;

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
