using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    using Real = System.Double;
    using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
    using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;
    using PositionSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition>;
    using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition_Connected>;

    using IPosition3DSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition3D>;
    using Position3DSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition3D>;
}
