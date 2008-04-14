using System;
using System.Collections.Generic;
using System.Text;

using Position_Interface;
using PositionSetViewer;
using DataStructure;
using Position_Connected_Interface;

namespace PositionSetViewer
{
    public partial class PainterDialog
    {
        #region PositionSet_Connected

        public void DrawPositionSet_Connected(IPositionSet_Connected pSet)
        {
            layers.Add(new Layer_PositionSet_Connected(pSet));

            IfNotHoldingShowFormInShowMode();
        }

        #endregion
    }
}
