using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace M2M
{
    public interface IM2MStructure
    {
        ILevel GetLevel(int levelSequecne);

        int GetLevelNum();

        void Preprocessing(IPositionSet positionSet);

        void Insert(IPosition point);

        void Remove_NotTestYet(IPosition point);

        IPart GetPartRefByDescendantPart(int AncestorLevelSequence, IPart DescendantPart, int DescendantLevelSequence);

        IPart GetPartRefByChildPart(IPart childPart, int chlidPartSequence);

        IPositionSet GetDescendentPositionSetByAncestorPart(int DescendantLevelSequence, IPart AncestorPart, int AncestorLevelSequence);

        IPositionSet GetChildPositionSetByParentPart(int parentPartLevelSequence, IPart parentPart);

        IPositionSet GetBottonLevelPositionSetByAncestorPart(IPart AncestorPart, int AncestorLevelSequence);
    }
}
