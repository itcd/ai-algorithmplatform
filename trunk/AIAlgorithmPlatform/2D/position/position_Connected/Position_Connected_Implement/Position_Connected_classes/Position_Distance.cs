using System;
using System.Collections.Generic;
using System.Text;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    [Serializable]
    public struct Position_Distance
    {
        public IPosition_Connected position;
        public float distance;

        public Position_Distance(IPosition_Connected position, float distance)
        {
            this.position = position;
            this.distance = distance;
        }
    }
}
