using System;
using System.Collections.Generic;
using System.Text;
using Position_Implement;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Implement.Position3D>;

namespace RandomPositionSetGenerator
{
   public interface IRandomGenerator3D
    {
        PositionSet3D getRandomPositionSet(int pointNum);
    }
}