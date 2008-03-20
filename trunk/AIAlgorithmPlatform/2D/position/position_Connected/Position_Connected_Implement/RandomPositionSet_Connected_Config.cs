using System;
using System.Collections.Generic;
using System.Text;
using Position_Connected_Interface;
using System.ComponentModel;

namespace Position_Connected_Implement
{
    public enum mapStyle
    {
        RandomFloatPositions_InFixedDistribution,
        RandomFloatPositions,
        RandomIntPositions_InFixedDistribution,
        RandomIntegerPositions,
        RandomMaze,
        RandomMaze_duplex,
        RandomMaze3D
    }

    public class RandomPositionSet_Connected_Config
    {
        PositionSet_Connected_Config config = new PositionSet_Connected_Config();
        mapStyle style = mapStyle.RandomFloatPositions_InFixedDistribution;

        public IPositionSet_ConnectedEdit Produce()
        {
            if (style == mapStyle.RandomMaze_duplex)
            {
                PositionMap positionMap = new PositionMap(config.Width, config.Height);
                positionMap.GenerateRandomMaze();
                return positionMap.GetPositionSet_ConnectedEdit();
            }
            else
            {
                List<IPosition_Connected_Edit> list = null;
                switch (style)
                {
                    case mapStyle.RandomFloatPositions_InFixedDistribution:
                        list = RandomPositionList.generateRandomFloatPositions_InFixedDistribution(config);
                        break;
                    case mapStyle.RandomFloatPositions:
                        list = RandomPositionList.generateRandomFloatPositions(config);
                        break;
                    case mapStyle.RandomIntPositions_InFixedDistribution:
                        list = RandomPositionList.generateRandomIntPositions_InFixedDistribution(config);
                        break;
                    case mapStyle.RandomIntegerPositions:
                        list = RandomPositionList.generateRandomIntPositions(config);
                        break;
                }
                IPositionSet_ConnectedEdit pSet = new PositionSet_ConnectedEdit(list);
                return pSet;
            }
        }

        [CategoryAttribute("Appearance")]
        public mapStyle MapStyle
        {
            get { return style; }
            set { style = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Width
        {
            get { return config.Width; }
            set { config.Width = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Height
        {
            get { return config.Height; }
            set { config.Height = value; }
        }

        [CategoryAttribute("Appearance")]
        public int TotalAmount
        {
            get { return config.TotalAmount; }
            set { config.TotalAmount = value; }
        }

        [CategoryAttribute("Appearance")]
        public bool Bounded
        {
            get { return config.Bounded; }
            set { config.Bounded = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount1
        {
            get { return config.Amount1; }
            set { config.Amount1 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount2
        {
            get { return config.Amount2; }
            set { config.Amount2 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount3
        {
            get { return config.Amount3; }
            set { config.Amount3 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability1
        {
            get { return config.Probability1; }
            set { config.Probability1 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability2
        {
            get { return config.Probability2; }
            set { config.Probability2 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability3
        {
            get { return config.Probability3; }
            set { config.Probability3 = value; }
        }
    }
}
