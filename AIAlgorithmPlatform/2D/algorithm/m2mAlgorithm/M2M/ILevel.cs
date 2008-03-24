using Position_Interface;

namespace M2M
{
    public interface ILevel
    {
        int GetUnitNumInWidth();
       
        int GetUnitNumInHeight();

        float GetGridWidth();

        float GetGridHeight();

        float ConvertRealValueToRelativeValueX(float realityValue);

        float ConvertRealValueToRelativeValueY(float realityValue);

        int ConvertRelativeValueToPartSequenceX(float relativelValue);

        int ConvertRelativeValueToPartSequenceY(float relativelValue);

        int ConvertRealValueToPartSequenceX(float realityValue);

        int ConvertRealValueToPartSequenceY(float realityValue);

        float ConvertPartSequenceXToRealValue(float x);

        float ConvertPartSequenceYToRealValue(float y);

        IPart GetPartRefByPartIndex(int x, int y);

        IPart GetPartRefByPoint(IPosition targetPoint);
    }
}