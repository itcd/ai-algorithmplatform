using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.Implement
{
    public static class PositionSetConvertor
    {
        public static void CopyTo(IPosition_ConnectedSet src, IPositionSet dest)
        {
            if (src != null && dest != null)
            {
                dest.Clear();
                foreach (IPosition_Connected p in src)
                {
                    dest.Add(p);
                }
            }
        }
    }
}
