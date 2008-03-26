using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using NearestNeighbor;
using Position_Implement;

namespace M2M
{
    public class M2M_NN : INearestNeighbor
    {
        #region code for algorithm demo
        //Preprocess
        public event dGetPositionSet GetPositionSetOfComparedPoint;
        public event dGetM2MStructure GetM2MStructure;

        //Query Process
        public event dGetPosition GetQueryPosition;
        public event dGetPartInSpecificLevel GetQueryPart;
        public event dSearchOnePartBeginAndGetSearchPartSequence SearchOnePartBeginAndGetSearchPartSequence;
        public event dGetPartInSpecificLevel GetSearchPart;
        public event dGetPosition GetComparedPoint;
        public event dGetPosition CurrentNearestPointChanged;
        public event dGetRectangle SearchBoundChanged;
        #endregion

        IM2MStructure m2mStructure = null;

        double set_pointInPartFactor = 33;

        public double TheDensityOfBottomLevel
        {
            get { return set_pointInPartFactor; }
            set { set_pointInPartFactor = value; }
        }

        public void PreProcess(List<IPosition> pointList)
        {
            PreProcess(new PositionSetEdit_ImplementByICollectionTemplate(pointList));
        }

        public void PreProcess(IPositionSet positionSet)
        {
            #region code for algorithm demo
            if (GetPositionSetOfComparedPoint != null)
            {
                GetPositionSetOfComparedPoint(positionSet);
            }
            #endregion

            //请在这里改变不同的m2mStructure实现。
            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();

            m2m_Creater_ForGeneralM2MStruture.SetPointInPartFactor(set_pointInPartFactor);

            m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(positionSet);

            m2mStructure.Preprocessing(positionSet);

            #region code for algorithm demo
            if (GetM2MStructure != null)
            {
                GetM2MStructure(m2mStructure);
            }
            #endregion
        }

        struct a
        {
            //int value;
        };

        public bool CanInsert(IPosition point)
        {
            object o = new a();

            return ((M2MStructure_General)m2mStructure).CanInsert(point);
        }

        public void Insert(IPosition point)
        {
            m2mStructure.Insert(point);
        }

        public void Remove(IPosition point)
        {
            m2mStructure.Remove_NotTestYet(point);
        }

        public IPosition ApproximateNearestNeighbor(IPosition targetPoint)
        {
            NearestNeighbor_ByM2MStructure NN = new NearestNeighbor_ByM2MStructure(m2mStructure);

            return NN.ApproximateNearestNeighbor(targetPoint);
        }

        public IPosition NearestNeighbor(IPosition targetPoint)
        {
            NearestNeighbor_ByM2MStructure NN = new NearestNeighbor_ByM2MStructure(m2mStructure);

            if (GetQueryPosition != null)
            {
                NN.GetQueryPosition += GetQueryPosition;
            }

            if(GetQueryPart != null)
            {
                NN.GetQueryPart += GetQueryPart;
            }

            if(SearchOnePartBeginAndGetSearchPartSequence != null)
            {
                NN.SearchOnePartBeginAndGetSearchPartSequence += SearchOnePartBeginAndGetSearchPartSequence;
            }

            if(GetSearchPart != null)
            {
                NN.GetSearchPart += GetSearchPart;
            }

            if(GetComparedPoint != null)
            {
                NN.GetComparedPoint += GetComparedPoint;
            }

            if(CurrentNearestPointChanged != null)
            {
                NN.CurrentNearestPointChanged += CurrentNearestPointChanged;
            }

            if(SearchBoundChanged != null)
            {
                NN.SearchBoundChanged += SearchBoundChanged;
            }

            return NN.NearestNeighbor(targetPoint);
        }

        public IPositionSet ApproximateKNearestNeighbor(IPosition targetPoint, int k)
        {
            NearestNeighbor_ByM2MStructure NN = new NearestNeighbor_ByM2MStructure(m2mStructure);

            return NN.ApproximateSearchKNearestNeighbor(targetPoint, k);
        }
    }
}
