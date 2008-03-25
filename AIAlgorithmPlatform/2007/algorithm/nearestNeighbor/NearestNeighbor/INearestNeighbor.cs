using System;
using Position_Interface;

namespace NearestNeighbor
{
    public interface INearestNeighbor
    {
        void PreProcess(System.Collections.Generic.List<IPosition> pointList);
        void Insert(IPosition point);
        void Remove(IPosition point);
        
        IPosition ApproximateNearestNeighbor(IPosition targetPoint);
        IPosition NearestNeighbor(IPosition point);
    }
}
