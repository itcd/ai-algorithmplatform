using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPosition=M2M.Position.IPosition;
using IPosition_Connected=M2M.Position.IPosition_Connected;
using IPosition3D=M2M.Position.IPosition3D;

namespace M2M.Position
{
    /// The declarations below are intent on copying to other source file's head as needed.

    using Real = System.Double;

    using IPositionSet = System.Collections.Generic.ICollection<IPosition>;
    using PositionSet = System.Collections.Generic.List<IPosition>;

    using IPosition_ConnectedSet = System.Collections.Generic.ICollection<IPosition_Connected>;
    using Position_ConnectedSet = System.Collections.Generic.List<IPosition_Connected>;

    using IPosition3DSet = System.Collections.Generic.ICollection<IPosition3D>;
    using Position3DSet = System.Collections.Generic.List<IPosition3D>;
}
