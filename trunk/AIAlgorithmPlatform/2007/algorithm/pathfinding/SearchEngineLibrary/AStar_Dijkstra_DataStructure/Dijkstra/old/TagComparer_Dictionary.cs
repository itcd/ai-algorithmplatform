using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace SearchEngineLibrary
{
    //用于比较两个包含Label的IPosition_Connected对象
    [Serializable]
    public class TagComparer_Dictionary : IComparer<IPosition_Connected>
    {
        Dictionary<IPosition_Connected, Tag> dict;
        Tag l1, l2;

        public TagComparer_Dictionary(Dictionary<IPosition_Connected, Tag> dictionary)
        {
            this.dict = dictionary;
        }

        public int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            if (dict.TryGetValue(p1, out l1) && dict.TryGetValue(p2, out l2))
            {
                if (l1.g < l2.g)
                    return -1;
                if (l1.g > l2.g)
                    return 1;
            }
            return 0;
        }
    }
}
