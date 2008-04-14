using System;
using System.Collections.Generic;
using System.Text;
using M2M.Position;
using Troschuetz.Random;
using Troschuetz;
//using M2M.Position.RandomGenerator;
using Configuration;
using RandomPositionSetGenerator;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;
using IPosition3DSet = System.Collections.Generic.List<M2M.Position.IPosition3D>;
//using IPosition3DSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition3D>;

namespace Position_Implement
{
    [Serializable]
    public class RandomPositionSet3D : PositionSet3D//APositionSetEdit_PositionSetEdit, 
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet3D()
        {
        }

        public RandomPositionSet3D(int n, double scale, Distribution xGenerator, Distribution yGenerator, Distribution zGenerator)
        {
            //List<IPosition3D> aryPositionSet = new List<IPosition3D>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(scale * xGenerator.NextDouble());
                float y = (float)(scale * yGenerator.NextDouble());
                float z = (float)(scale * zGenerator.NextDouble());
                this.Add(new Position3D(x, y, z));
            }

            //positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
        }
    }

    [Serializable]
    public class RandomPositionSet_Square3D : PositionSet3D//APositionSetEdit_PositionSetEdit,
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_Square3D()
        {
        }

        public RandomPositionSet_Square3D(int n, float min, float max)
        {
            //List<IPosition> aryPositionSet = new List<IPosition>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(min + r.NextDouble() * (max - min));
                float y = (float)(min + r.NextDouble() * (max - min));
                float z = (float)(min + r.NextDouble() * (max - min));
                this.Add(new Position3D(x, y, z));
            }

            //positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
        }
    }

    [Serializable]
    public class RandomPositionSet_Rectangle3D : PositionSet3D //: APositionSetEdit_PositionSetEdit,
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_Rectangle3D()
        {
        }

        public RandomPositionSet_Rectangle3D(int n, float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
        {
            List<IPosition3D> aryPositionSet = new List<IPosition3D>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(xmin + r.NextDouble() * (xmax - xmin));
                float y = (float)(ymin + r.NextDouble() * (ymax - ymin));
                float z = (float)(zmin + r.NextDouble() * (zmax - zmin));
                this.Add(new Position3D(x, y, z));
            }

            //positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
        }
    }

    public enum distributionStyle
    {
        Uniform,
        GaussianDistribution,
        LaplaceDistribution,
        ClusterGaussianDistribution,
        ClusterLaplaceDistribution
    };



    public class RandomPositionSet_InFixedDistribution3D : PositionSet3D  //: APositionSetEdit_PositionSetEdit,
    {
        int pointNum;
        IRandomGenerator3D RandomGen;

        public int PointNum
        {
            get { return pointNum; }
            set { pointNum = value; }
        }

        distributionStyle dStyle;
        public distributionStyle DistributionStyle
        {
            get { return dStyle; }
            set { dStyle = value;
            ChangeGenernator();
            }
        }

        private void ChangeGenernator()
        {
            if (dStyle == distributionStyle.Uniform)
                return;

            else if (dStyle == distributionStyle.LaplaceDistribution)
            {
                RandomGen = new LaplaceGen3D(minMu, maxMu, pointNum);
            }
            else if (dStyle == distributionStyle.ClusterLaplaceDistribution)
                RandomGen = new ClusterLaplaceGen3D(minMu, maxMu, pointNum);
            else if (dStyle == distributionStyle.GaussianDistribution)
                RandomGen = new GaussianGen3D(minMu, maxMu, pointNum);
            else if (dStyle == distributionStyle.ClusterGaussianDistribution)
                RandomGen = new ClusterGaussianGen3D(minMu, maxMu, pointNum);
            else
                return;
            new ConfiguratedByForm(this.RandomGen);
        }

        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_InFixedDistribution3D()
        {
        }
              
        public void ConfiguratedByGUI()
        {
            new ConfiguratedByForm(this);
        }

     

   

        public RandomPositionSet_InFixedDistribution3D(int pointNum, distributionStyle dStyle)
        {
            this.pointNum = pointNum;
            this.DistributionStyle = dStyle;
            Produce();
        }

        float minMu = 0;

        public float MinMu
        {
            get { return minMu; }
            set { minMu = value; }
        }
        float maxMu = 80;

        public float MaxMu
        {
            get { return maxMu; }
            set { maxMu = value; }
        }

        float minBound = 0;

        public float MinBound
        {
            get { return minBound; }
            set { minBound = value; }
        }
        float maxBound = 1000;

        public float MaxBound
        {
            get { return maxBound; }
            set { maxBound = value; }
        }

        public PositionSet3D Produce()
        {
            //int seed = (int)DateTime.Now.Ticks;

            //int clusterPointNum = 10;
            PositionSet3D positionSet3D = new PositionSet3D();

            if (dStyle == distributionStyle.Uniform)
            {
                positionSet3D = new RandomPositionSet_Square3D(pointNum, minBound, maxBound);
            }
            else //if (dStyle == distributionStyle.LaplaceDistribution || dStyle == distributionStyle.ClusterLaplaceDistribution)
            {


                positionSet3D = RandomGen.getRandomPositionSet(pointNum);
                //LaplaceDistribution distributionX = new LaplaceDistribution(new StandardGenerator(seed++));
                //distributionX.Alpha = 4;

                //LaplaceDistribution distributionY = new LaplaceDistribution(new StandardGenerator(seed++));
                //distributionY.Alpha = 4;

                //for (int i = 0; i < clusterPointNum; i++)
                //{
                //    distributionX.Mu = RandomMaker.RapidBetween(minMu, maxMu);
                //    distributionY.Mu = RandomMaker.RapidBetween(minMu, maxMu);
                //    RandomPositionSet randomPositionSet = new RandomPositionSet((int)(pointNum / clusterPointNum), 1000, distributionX, distributionY);
                //    positionSetEditSet.AddPositionSet(randomPositionSet);
                //}
            }
            //else if (dStyle == distributionStyle.GaussianDistribution || dStyle == distributionStyle.ClusterGaussianDistribution)
            //{
            //    //if (dStyle == distributionStyle.GaussianDistribution)
            //    //{
            //    //    clusterPointNum = 1;
            //    //}

            //    //NormalDistribution distributionX = new NormalDistribution(new StandardGenerator(seed++));
            //    //distributionX.Sigma = 8;

            //    //NormalDistribution distributionY = new NormalDistribution(new StandardGenerator(seed++));
            //    //distributionY.Sigma = 8;

            //    //for (int i = 0; i < clusterPointNum; i++)
            //    //{
            //    //    distributionX.Mu = RandomMaker.RapidBetween(minMu, maxMu);
            //    //    distributionY.Mu = RandomMaker.RapidBetween(minMu, maxMu);
            //    //    RandomPositionSet randomPositionSet = new RandomPositionSet((int)(pointNum / clusterPointNum), 1000, distributionX, distributionY);
            //    //    positionSetEditSet.AddPositionSet(randomPositionSet);
            //    //}
            //}

            positionSet3D = positionSet3D;

            return this;
        }
    }

    public class GetRandomPosition3DFromPositionSetRectangle3D
    {
        double minX = 0;
        double minY = 0;
        double minZ = 0;
        double maxX = 0;
        double maxY = 0;
        double maxZ = 0;

        public GetRandomPosition3DFromPositionSetRectangle3D(IPosition3DSet positionSet)
        {
            IPosition3D[] positionArray = positionSet.ToArray();

            if (positionArray.Length!=0)
            {
                minX = positionArray[0].GetX();
                minY = positionArray[0].GetY();
                minZ = positionArray[0].GetZ();
                maxX = positionArray[0].GetX();
                maxY = positionArray[0].GetY();
                maxZ = positionArray[0].GetZ();
            }
            int i = 1;

            while (i < positionArray.Length)
            {
                if (minX > positionArray[i].GetX())
                {
                    minX = positionArray[i].GetX();
                }
                else if (maxX < positionArray[i].GetX())
                {
                    maxX = positionArray[i].GetX();
                }

                if (minY > positionArray[i].GetY())
                {
                    minY = positionArray[i].GetY();
                }
                else if (maxY < positionArray[i].GetY())
                {
                    maxY = positionArray[i].GetY();
                }
                if (minZ > positionArray[i].GetZ())
                {
                    minZ = positionArray[i].GetZ();
                }
                else if (maxZ < positionArray[i].GetZ())
                {
                    maxZ = positionArray[i].GetZ();
                }
                i++;
            }
        }

        public IPosition3D Get()
        {
            Random ra = new Random();
            return new Position3D(minX + (float)(ra.NextDouble() * (maxX - minX)),
                minY + (float)(ra.NextDouble() * (maxY - minY)), minZ + (float)(ra.NextDouble() * (maxZ - minZ)));
        }
    }
}