using Position_Interface;

namespace WriteLineInGridEngine
{
    public interface IWriteLineInGridEngine
    {
        IPositionSet WriteLineInGrid(float gridWidth, float gridHeight, IPosition startPosition, IPosition endPosition);
    }
}