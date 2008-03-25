using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace M2M
{
    public interface IPart_Multi : IPart_Edit
    {
        IPart_Connected CreateSubPart();

        IEnumerable<IPart_Connected> GetSubPartSet();
        void SetSubPartSet(List<IPart_Connected> SubPartList);
    }

    public interface IPart_Connected : IPart_Edit, IPosition_Connected_Edit
    {
        IPart_Connected GetParentPart();
        void SetParentPart(IPart_Connected parentPart);
        IPartSet_Connected_Adjacency GetAdjacencyPartSet();
    }

    public interface IPartSet_Connected : IPositionSet_Connected
    {
        IPart_Connected GetPart_Connected();
    }

    public interface IPartSet_Connected_Adjacency : IPositionSet_Connected_AdjacencyEdit, IPartSet_Connected
    {
    }
}