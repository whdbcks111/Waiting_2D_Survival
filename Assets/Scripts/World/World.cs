using System.Collections.Generic;
using UnityEngine;

public class World
{
    public WorldGenerator Generator { get; }
    private Dictionary<ChunkCoord, Chunk> _chunks = new();

    public World(WorldGenerator generator)
    {
        Generator = generator;
    }
    
    public Block GetBlock(int x, int y)
    {
        if (!_chunks.TryGetValue(new ChunkCoord(x >> 4, y >> 4), out var chunk)) return null;
        return chunk.GetBlock((byte)(x & 0xF), (byte)(y & 0xF));
    }

    public void SetBlock(int x, int y, Block block)
    {
        var coord = new ChunkCoord(x >> 4, y >> 4);
        var chunk = GetChunk(coord);
        chunk.SetBlock((byte)(x & 0xF), (byte)(y & 0xF), block);
    }

    public Chunk GetChunk(int x, int y)
        => GetChunk(new ChunkCoord(x, y));

    public Chunk GetChunk(ChunkCoord coord)
    {
        if (_chunks.TryGetValue(coord, out var chunk)) return chunk;
        chunk = Generator.GetChunk(coord);
        _chunks.TryAdd(coord, chunk);
        return chunk;
    }

    public Chunk GetLoadedChunk(ChunkCoord coord)
    {
        if (_chunks.TryGetValue(coord, out var chunk)) return chunk;
        return null;
    }

    public Block GetBlock(Vector2Int pos) => GetBlock(pos.x, pos.y);
}
