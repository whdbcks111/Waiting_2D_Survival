public class ChunkCoord
{
    public readonly int X, Y;
    public ChunkCoord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(ChunkCoord a, ChunkCoord b)
    {
        if (a is null || b is null) return a is null && b is null;
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(ChunkCoord a, ChunkCoord b) => !(a == b);

    public override bool Equals(object obj)
    {
        return obj is ChunkCoord coord && coord == this;
    }

    public override int GetHashCode()
    {
        return (X << 4) | Y;
    }
}
