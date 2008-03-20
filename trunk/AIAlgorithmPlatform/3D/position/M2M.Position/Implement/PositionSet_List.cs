using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;

namespace M2M.Position.Implement
{
    public class PositionSet_List : AbstractPositionSet
    {
        private List<IPosition> list;

        public PositionSet_List()
        {
            list = new List<IPosition>();
        }

        public PositionSet_List(int size)
        {
            list = new List<IPosition>(size);
        }

        public override bool Contains(IPosition item)
        {
            return list.Contains(item);
        }

        public override void CopyTo(IPosition[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override void Add(IPosition item)
        {
        
        }
    }
}
