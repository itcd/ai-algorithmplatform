using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    public interface IPositionSet_ConnectedEdit:IPositionSet_Connected
    {
        IPosition_Connected_Edit GetPosition_Connected_Edit();
        void AddPosition_Connected(IPosition_Connected p);
        void RemovePosition_Connected(IPosition_Connected p);
        void Clear();
    }
}