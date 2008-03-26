using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace SearchEngineLibrary
{
    /// <summary>
    /// 用于比较两个包含AStarTag的IPosition_Connected对象
    /// </summary>
    /// 
    [Serializable]
    public class AStarTagComparer : IComparer<IPosition_Connected>
    {
        public int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            Tag t1 = (Tag)p1.GetAttachment(), t2 = (Tag)p2.GetAttachment();
            if (t1 != null && t2 != null)
            {
                if (t1.f < t2.f)
                    return -1;
                if (t1.f > t2.f)
                    return 1;

                if (t1.g > t2.g)
                    return -1;
                if (t1.g < t2.g)
                    return 1;
            }
            return 0;
        }
    }
}
