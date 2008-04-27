using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;
using Troschuetz;
//using M2M.Position.RandomGenerator;
using M2M.Position.Implement;
using Configuration;
using Position_Implement;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;

namespace RandomPositionSetGenerator3D
{
    class LaplaceGen3D: IRandomGenerator3D
    {
        int clusterPointNum=1;
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

        double Z_Alpha = 4;

        public double Z_Alpha1
        {
            get { return Z_Alpha; }
            set { Z_Alpha = value; }
        }

        float minMu;
        float maxMu;
        int pointNum;
        PositionSet3D positionSet3D = new PositionSet3D();

        public LaplaceGen3D(float minMu, float maxMu, int pointNum)
        {
            this.maxMu = maxMu;
            this.minMu = minMu;
            this.pointNum = pointNum;
        }

        #region IRandomGenerator ≥…‘±

        public PositionSet3D getRandomPositionSet(int pointNum)
        {
            unchecked
            {
                int seed = (int) DateTime.Now.Ticks;
                this.pointNum = pointNum;
                LaplaceDistribution distributionX = new LaplaceDistribution(new StandardGenerator(seed++));
                distributionX.Alpha = X_Alpha;

                LaplaceDistribution distributionY = new LaplaceDistribution(new StandardGenerator(seed++));
                distributionY.Alpha = Y_Alpha;

                LaplaceDistribution distributionZ = new LaplaceDistribution(new StandardGenerator(seed++));
                distributionZ.Alpha = Z_Alpha;

                Random r = new Random();
                for (int i = 0; i < clusterPointNum; i++)
                {
                    distributionX.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    distributionY.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    distributionZ.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    RandomPositionSet3D randomPositionSet =
                        new RandomPositionSet3D((int) (pointNum/clusterPointNum), 1000, distributionX, distributionY,distributionZ);
                    positionSet3D = (PositionSet3D)randomPositionSet;
                }
            }
            return positionSet3D;
        }

        #endregion
    }
}
