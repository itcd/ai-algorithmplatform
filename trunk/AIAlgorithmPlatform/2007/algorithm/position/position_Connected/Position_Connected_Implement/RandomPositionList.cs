using System;
using System.Collections.Generic;
using System.Text;

using Position_Interface;
using M2M;
using RandomMakerAlgorithm;
using Position_Connected_Interface;
using Position_Implement;
using Configuration;

namespace Position_Connected_Implement
{
    [Serializable]
    public class RandomPositionList
    {
        public static List<IPosition_Connected_Edit> generateRandomFloatPositions(PositionSet_Connected_Config c)
        {
            return generateRandomFloatPositions(c.Width, c.Height, c.Bounded, c.TotalAmount, c.Amount1, c.Amount2, c.Amount3, c.Probability1, c.Probability2, c.Probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomIntPositions(PositionSet_Connected_Config c)
        {
            return generateRandomIntPositions(c.Width, c.Height, c.Bounded, c.TotalAmount, c.Amount1, c.Amount2, c.Amount3, c.Probability1, c.Probability2, c.Probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomFloatPositions_InFixedDistribution(PositionSet_Connected_Config c)
        {
            return generateRandomFloatPositions_InFixedDistribution(c.Width, c.Height, c.Bounded, c.TotalAmount, c.Amount1, c.Amount2, c.Amount3, c.Probability1, c.Probability2, c.Probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomIntPositions_InFixedDistribution(PositionSet_Connected_Config c)
        {
            return generateRandomIntPositions_InFixedDistribution(c.Width, c.Height, c.Bounded, c.TotalAmount, c.Amount1, c.Amount2, c.Amount3, c.Probability1, c.Probability2, c.Probability3);
        }

        public static List<IPosition_Connected_Edit> convertPositionSetToList(IPositionSet set, int width, int height, bool bounded)
        {
            List<IPosition_Connected_Edit> all = new List<IPosition_Connected_Edit>();
            IPosition position;
            float x, y;
            //为将地图范围以内的点坐标创建节点
            set.InitToTraverseSet();
            while (set.NextPosition())
            {
                position = set.GetPosition();
                x = position.GetX();
                y = position.GetY();
                if (!bounded || (x <= width - 1 && y <= height - 1))
                    all.Add(new Position_Connected_Edit(x, y));
            }
            return all;
        }

        public static List<IPosition_Connected_Edit> convertPositionSetToList_Integer(IPositionSet set, int width, int height, bool bounded)
        {
            List<IPosition_Connected_Edit> all = new List<IPosition_Connected_Edit>();
            IPosition position;
            int x, y;
            //将点坐标取整，为将地图范围以内的点坐标创建节点
            set.InitToTraverseSet();
            while (set.NextPosition())
            {
                position = set.GetPosition();
                x = (int)position.GetX();
                y = (int)position.GetY();
                if (!bounded || (x < width && y < height))
                    all.Add(new Position_Connected_Edit(x, y));
            }
            return all;
        }

        public static IPositionSet getRandomPositionSet_InFixedDistribution(int width, int height, int totalAmount)
        {
            //产生随机点集：
            RandomPositionSet_InFixedDistribution set = new RandomPositionSet_InFixedDistribution();
            set.DistributionStyle = distributionStyle.Uniform;
            set.MinBound = 0;
            set.MaxBound = width > height ? width : height;
            set.PointNum = totalAmount;
            new ConfiguratedByForm(set);
            set.Produce();
            return set;
        }

        public static List<IPosition_Connected_Edit> generateRandomPositionSet_Connected(List<IPosition_Connected_Edit> all, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            if (all.Count > 0)
            {
                IPositionSet_Connected positionSet = new PositionSet_Connected(all);
                IPositionSet nearSet;
                IPosition_Connected_Edit near_p;
                float distance;
                bool unconnected;
                IPositionSet_Connected_AdjacencyEdit adjSet;

                //存储计算好的点数量和概率以备重复使用
                int[] amount = new int[3];//获得周围amount个点来建立连接
                double[] probability = new double[3];//建立连接的概率为probability
                amount[0] = amount1;
                amount[1] = amount2;
                amount[2] = amount3;
                probability[0] = probability1;
                probability[1] = probability2;
                probability[2] = probability3;

                M2M_NN m2m = new M2M_NN();
                m2m.PreProcess(positionSet);
                //每个节点都随机地与周围的一些节点建立连接
                foreach (IPosition_Connected_Edit p in all)
                {
                    for (int i = 0; i < amount.Length; i++)
                    {
                        nearSet = m2m.ApproximateKNearestNeighbor(p, amount[i]);
                        nearSet.InitToTraverseSet();
                        while (nearSet.NextPosition())
                        {
                            if (RandomMaker.RapidBetween01() < probability[i])
                            {
                                near_p = (IPosition_Connected_Edit)nearSet.GetPosition();
                                if (near_p != p)
                                {
                                    unconnected = true;
                                    adjSet = p.GetAdjacencyPositionSetEdit();
                                    adjSet.InitToTraverseSet();
                                    while (adjSet.NextPosition())
                                    {
                                        if (adjSet.GetPosition_Connected_Edit() == near_p)
                                        {
                                            unconnected = false;
                                            break;
                                        }
                                    }
                                    if (unconnected)
                                    {
                                        distance = (float)Math.Sqrt((p.GetX() - near_p.GetX()) * (p.GetX() - near_p.GetX()) + (p.GetY() - near_p.GetY()) * (p.GetY() - near_p.GetY()));
                                        adjSet.AddAdjacency(near_p, distance);
                                        near_p.GetAdjacencyPositionSetEdit().AddAdjacency(p, distance);
                                    }
                                }
                            }
                        }//while
                    }//for
                }//foreach

                //清除没有连接的孤立节点
                int index = 0;
                while (index < all.Count)
                {
                    near_p = all[index];
                    adjSet = near_p.GetAdjacencyPositionSetEdit();
                    adjSet.InitToTraverseSet();
                    if (!adjSet.NextPosition())
                        all.Remove(near_p);
                    else
                        index++;
                }
            }
            return all;
        }

        public static List<IPosition_Connected_Edit> generateRandomFloatPositions(int width, int height, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            //生成随机节点集
            RandomPositionSet_Rectangle set = new RandomPositionSet_Rectangle(totalAmount, 0, width, 0, height);
            return generateRandomPositionSet_Connected(convertPositionSetToList(set, width, height, true), amount1, amount2, amount3, probability1, probability2, probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomIntPositions(int width, int height, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            //生成随机节点集
            RandomPositionSet_Rectangle set = new RandomPositionSet_Rectangle(totalAmount, 0, width, 0, height);
            return generateRandomPositionSet_Connected(convertPositionSetToList_Integer(set, width, height, true), amount1, amount2, amount3, probability1, probability2, probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomFloatPositions(int width, int height, bool bounded, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            //生成随机节点集
            RandomPositionSet_Rectangle set = new RandomPositionSet_Rectangle(totalAmount, 0, width, 0, height);
            return generateRandomPositionSet_Connected(convertPositionSetToList(set, width, height, bounded), amount1, amount2, amount3, probability1, probability2, probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomIntPositions(int width, int height, bool bounded, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            //生成随机节点集
            RandomPositionSet_Rectangle set = new RandomPositionSet_Rectangle(totalAmount, 0, width, 0, height);
            return generateRandomPositionSet_Connected(convertPositionSetToList_Integer(set, width, height, bounded), amount1, amount2, amount3, probability1, probability2, probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomFloatPositions_InFixedDistribution(int width, int height, bool bounded, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            return generateRandomPositionSet_Connected(convertPositionSetToList(getRandomPositionSet_InFixedDistribution(width, height, totalAmount), width, height, bounded), amount1, amount2, amount3, probability1, probability2, probability3);
        }

        public static List<IPosition_Connected_Edit> generateRandomIntPositions_InFixedDistribution(int width, int height, bool bounded, int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            return generateRandomPositionSet_Connected(convertPositionSetToList_Integer(getRandomPositionSet_InFixedDistribution(width, height, totalAmount), width, height, bounded), amount1, amount2, amount3, probability1, probability2, probability3);
        }
    }
}
