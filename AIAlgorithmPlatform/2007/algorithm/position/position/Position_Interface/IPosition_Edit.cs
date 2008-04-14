using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Interface
{
    public interface IPosition_Edit : IPosition
	{
        void SetX(float v);
        void SetY(float v);
	}
}
