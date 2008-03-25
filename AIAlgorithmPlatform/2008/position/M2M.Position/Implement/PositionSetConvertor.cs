using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;

namespace M2M.Position.Implement
{
    public static class PositionSetConvertor
    {
        public static void CopyTo(ICollection<IPosition_Connected> src, ICollection<IPosition> dest)
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
