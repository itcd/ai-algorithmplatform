using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;
using Troschuetz;
using RandomMakerAlgorithm;
using Position_Interface;
using Configuration;
using RandomPositionSetGenerator;

namespace Position_Implement
{
    [Serializable]
    public class RandomPositionSet : APositionSetEdit_PositionSetEdit, IPositionSetEdit
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet()
        {
        }

        public RandomPositionSet(int n, double scale, Distribution xGenerator, Distribution yGenerator)
        {
            List<IPosition> aryPositionSet = new List<IPosition>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(scale * xGenerator.NextDouble());
                float y = (float)(scale * yGenerator.NextDouble());
                aryPositionSet.Add(new Position_Point(x, y));
            }

            positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
        }
    }

    [Serializable]
    public class RandomPositionSet_Square : APositionSetEdit_PositionSetEdit, IPositionSetEdit
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_Square()
        {
        }

        public RandomPositionSet_Square(int n, float min, float max)
        {
            List<IPosition> aryPositionSet = new List<IPosition>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(min + r.NextDouble() * (max - min));
                float y = (float)(min + r.NextDouble() * (max - min));
                aryPositionSet.Add(new Position_Point(x, y));
            }

            positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
        }
    }

    [Serializable]
    public class RandomPositionSet_Rectangle : APositionSetEdit_PositionSetEdit, IPositionSetEdit
    {
        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_Rectangle()
        {
        }

        public RandomPositionSet_Rectangle(int n, float xmin, float xmax, float ymin, float ymax)
        {
            List<IPosition> aryPositionSet = new List<IPosition>(n);
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                float x = (float)(xmin + r.NextDouble() * (xmax - xmin));
                float y = (float)(ymin + r.NextDouble() * (ymax - ymin));
                aryPositionSet.Add(new Position_Point(x, y));
            }

            positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(aryPositionSet);
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



    public class RandomPositionSet_InFixedDistribution : APositionSetEdit_PositionSetEdit, IPositionSetEdit
    {
        int pointNum;
        IRandomGenerator RandomGen;

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
                RandomGen = new LaplaceGen(minMu, maxMu, pointNum);
            }
            else if (dStyle == distributionStyle.ClusterLaplaceDistribution)
                RandomGen = new ClusterLaplaceGen(minMu, maxMu, pointNum);
            else if (dStyle == distributionStyle.GaussianDistribution)
                RandomGen = new GaussianGen(minMu, maxMu, pointNum);
            else if (dStyle == distributionStyle.ClusterGaussianDistribution)
                RandomGen = new ClusterGaussianGen(minMu, maxMu, pointNum);
            else
                return;
            new ConfiguratedByForm(this.RandomGen);
        }

        /// <summary>
        /// 利用ConfigurationForm来初始化参数
        /// </summary>
        public RandomPositionSet_InFixedDistribution()
        {
        }
              
        public void ConfiguratedByGUI()
        {
            new ConfiguratedByForm(this);
        }

     

   

        public RandomPositionSet_InFixedDistribution(int pointNum, distributionStyle dStyle)
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

        public IPositionSetEdit Produce()
        {
            //int seed = (int)DateTime.Now.Ticks;

            //int clusterPointNum = 10;
            PositionSetEditSet positionSetEditSet = new PositionSetEditSet();

            if (dStyle == distributionStyle.Uniform)
            {
                positionSetEditSet.AddPositionSet(new RandomPositionSet_Square(pointNum, minBound, maxBound));
            }
            else //if (dStyle == distributionStyle.LaplaceDistribution || dStyle == distributionStyle.ClusterLaplaceDistribution)
            {


                positionSetEditSet = RandomGen.getRandomPositionSet(pointNum);
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

            positionSetEdit = positionSetEditSet;

            return this;
        }
    }

    public class GetRandomPositionFromPositionSetRectangle
    {
        float minX = 0;
        float minY = 0;
        float maxX = 0;
        float maxY = 0;

        public GetRandomPositionFromPositionSetRectangle(IPositionSet positionSet)
        {
            positionSet.InitToTraverseSet();

            if (positionSet.NextPosition())
            {
                minX = positionSet.GetPosition().GetX();
                minY = positionSet.GetPosition().GetY();
                maxX = positionSet.GetPosition().GetX();
                maxY = positionSet.GetPosition().GetY();
            }

            while (positionSet.NextPosition())
            {
                if (minX > positionSet.GetPosition().GetX())
                {
                    minX = positionSet.GetPosition().GetX();
                }
                else if (maxX < positionSet.GetPosition().GetX())
                {
                    maxX = positionSet.GetPosition().GetX();
                }

                if (minY > positionSet.GetPosition().GetY())
                {
                    minY = positionSet.GetPosition().GetY();
                }
                else if (maxY < positionSet.GetPosition().GetY())
                {
                    maxY = positionSet.GetPosition().GetY();
                }
            }
        }

        public IPosition Get()
        {
            return new Position_Point(RandomMaker.RapidBetween(minX, maxX),
                RandomMaker.RapidBetween(minY, maxY));
        }
    }
}