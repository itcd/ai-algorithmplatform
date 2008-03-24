using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Interface
{
    public interface IPositionSetEdit:IPositionSet
    {
        void AddPosition(IPosition p);
        void RemovePosition(IPosition p);
        void Clear();
    }
}
