using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Position_Connected_Implement
{
    public class PositionSet_Connected_Config
    {
        int width = 1000;
        int height = 1000;
        int totalAmount = 1000;
        int amount1 = 2;
        int amount2 = 6;
        int amount3 = 16;
        double probability1 = 0.7;
        double probability2 = 0.3;
        double probability3 = 0.1;
        bool bounded = false;

        [CategoryAttribute("Appearance")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        [CategoryAttribute("Appearance")]
        public int TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }

        [CategoryAttribute("Appearance")]
        public bool Bounded
        {
            get { return bounded; }
            set { bounded = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount1
        {
            get { return amount1; }
            set { amount1 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount2
        {
            get { return amount2; }
            set { amount2 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public int Amount3
        {
            get { return amount3; }
            set { amount3 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability1
        {
            get { return probability1; }
            set { probability1 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability2
        {
            get { return probability2; }
            set { probability2 = value; }
        }

        [CategoryAttribute("Connecting Probability")]
        public double Probability3
        {
            get { return probability3; }
            set { probability3 = value; }
        }
    }
}
