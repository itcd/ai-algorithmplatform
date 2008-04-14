using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    public interface IPosition_Connected_Edit : IPosition_Connected, IPosition_Edit
    {
        IPositionSet_Connected_AdjacencyEdit GetAdjacencyPositionSetEdit();
    }
}
