using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace M2M
{
    public delegate void dGetPositionSet(IPositionSet positionSet);
    public delegate void dGetPositionSetInSpecificLevel(ILevel level, int levelSequence, IPositionSet positionSet);
    public delegate void dGetM2MLevel(ILevel level, int levelSequence);
    public delegate void dGetM2MStructure(IM2MStructure m2mStructure);
    public delegate void dMilestone();

    public delegate void dGetRectangle(float upperBound, float lowerBound, float leftBound, float rightBound);
    public delegate void dGetPositionSetOfSpecificSequence(int sequence, IPositionSet positionSet);
    public delegate void dSearchOnePartBeginAndGetSearchPartSequence(int sequence);
    public delegate void dGetPosition(IPosition point);
    public delegate void dGetPartInSpecificLevel(ILevel level,int levelSequence, IPart Part);

    public delegate IPositionSet dConvexHull(IPositionSet positionSet);
    public delegate void dPreProcess(IPositionSet positionSet);
}