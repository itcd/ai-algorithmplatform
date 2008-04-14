using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position
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