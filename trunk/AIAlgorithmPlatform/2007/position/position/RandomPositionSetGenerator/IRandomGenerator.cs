using System;
using System.Collections.Generic;
using System.Text;
using Position_Implement;

namespace RandomPositionSetGenerator
{
   public interface IRandomGenerator
    {
        PositionSetEditSet getRandomPositionSet(int pointNum);
    }
}
