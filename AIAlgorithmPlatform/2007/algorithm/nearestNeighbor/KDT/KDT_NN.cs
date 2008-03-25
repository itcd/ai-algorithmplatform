using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Position_Interface;
using NearestNeighbor;

namespace KDT
{
    public class KDT_NN : INearestNeighbor
    {
        KDTree kdt;
        #region NearestNeighbor ≥…‘±

        public void PreProcess(List<IPosition> pointList)
        {
            ArrayList al = new ArrayList();
            al.AddRange(pointList.ToArray());
            kdt = KDTree.CreateKDTree(al);
        }

        public void Insert(IPosition point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Remove(IPosition point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IPosition ApproximateNearestNeighbor(IPosition targetPoint)
        {
            KD2DPoint pp = new KD2DPoint(targetPoint);
            double dis;
            KD2DPoint np = (KD2DPoint)kdt.NearestNeighbour(pp, out dis);
            return np;

        }

        public IPosition NearestNeighbor(IPosition point)
        {
            KD2DPoint pp = new KD2DPoint(point);
            double dis;
            KD2DPoint np = (KD2DPoint)kdt.NearestNeighbour(pp, out dis);
            return np;
        }

        #endregion
    }
}
