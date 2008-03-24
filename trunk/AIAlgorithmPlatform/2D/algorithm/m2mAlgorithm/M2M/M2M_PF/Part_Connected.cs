using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace M2M
{
    class Part_Connected : Part, IPart_Connected
    {
        IPart_Connected parentPart = null;

        IPartSet_Connected_Adjacency partSet_Connected_Adjacency = new PartSet_Connected_Adjacency();

        Object o = null;

        public void SetSubPositionList(List<IPosition> subPositionList)
        {
            this.subPositionList = subPositionList;
        }
                
        #region IPart_Connected member
        public override IPart_Edit Create()
        {
            return new Part_Connected();
        }

        public IPart_Connected GetParentPart()
        {
            return parentPart;
        }

        public void SetParentPart(IPart_Connected parentPart)
        {
            this.parentPart = parentPart;
        }

        public IPartSet_Connected_Adjacency GetAdjacencyPartSet()
        {
            return partSet_Connected_Adjacency;
        }
        #endregion

        #region IPosition_Connected member
        public Object GetAttachment()
        {
            return o;
        }

        public void SetAttachment(Object o)
        {
            this.o = o;
        }

        public IPositionSet_Connected_Adjacency GetAdjacencyPositionSet()
        {
            return partSet_Connected_Adjacency;
        }        
        #endregion

        #region IPosition_Connected_Edit member
        public IPositionSet_Connected_AdjacencyEdit GetAdjacencyPositionSetEdit()
        {
            return partSet_Connected_Adjacency;
        }
        #endregion
    }
}
