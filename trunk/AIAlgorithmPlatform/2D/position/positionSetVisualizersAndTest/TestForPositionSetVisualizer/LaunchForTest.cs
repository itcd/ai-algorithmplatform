using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using PositionSetVisualizers;
using Position_Implement;

namespace TestForPositionSetVisualizer
{
    class LaunchForTest
    {
        static void Main(string[] args)
        {
            IPositionSet ps = new RandomPositionSet_Square(100, -1000, 1000);            
            PositionSetVisualizer.TestShowVisualizer(ps);
        }
    }
}
