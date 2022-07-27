using UnityEngine;

public class Chunk
{
    private World _world;
    public const byte ChunkSize = 16;
    private Block[,] _blocks = new Block[ChunkSize, ChunkSize];
    public ChunkCoord Coord { get; }

    public Vector2Int Location { get; }
    public GameObject gameObject { get; } = new GameObject();


    public Chunk(World world, ChunkCoord coord, bool generate = true, Block[,] blocks = null)
    {
        _world = world;
        Coord = coord;
        Location = new Vector2Int(coord.X << 4, coord.Y << 4);
        gameObject.name = string.Format("Chunk({0}, {1})", Coord.X, Coord.Y);
        gameObject.transform.position = (Vector2)Location;
        gameObject.transform.parent = _world.Generator.transform;
        gameObject.SetActive(false);

        if (generate)
            GenerateWorld();
        else
            _blocks = blocks;
    }

    private void GenerateWorld()
    {
        GenerateSpecialBlock();
        GenerateBlock();
    }

    private void GenerateSpecialBlock()
    {
        if (Random.Range(0, 1F) > .38) return;
        //var random = Random.Range(0, 1F);
        var pos = RandomBlockPos();
        var prefab = Blocks.Crafting.gameObject;
        /*if(random < .5)
        {
            
        }*/
        var block = Object.Instantiate(prefab, (Vector2)(pos + Location), Quaternion.identity).GetComponent<Block>();
        block.IsDefault = false;
        block.transform.parent = gameObject.transform;
        _blocks[pos.x, pos.y] = block;
    }

    private void GenerateBlock()
    {
        var blockAmount = ChunkSize * ChunkSize * _world.Generator.SpawnAmountPercent;
        for (var i = 0; i < blockAmount; i++)
        {
            var pos = RandomBlockPos();
            var prefab = Blocks.Bush.gameObject;
            var random = Random.Range(0, 10F) * .1F;
            if (random < _world.Generator.SpawnStonePercent)
            {
                prefab = Blocks.Stone.gameObject;
            }
            var block = Object.Instantiate(prefab, (Vector2)(pos + Location), Quaternion.identity).GetComponent<Block>();
            block.IsDefault = false;
            block.transform.parent = gameObject.transform;
        }
    }
    
    private Vector2Int RandomBlockPos()
    {
        var pos = new Vector2Int(Random.Range(0, ChunkSize), Random.Range(0, ChunkSize));
        while(_blocks[pos.x, pos.y] != null)
            pos = new Vector2Int(Random.Range(0, ChunkSize), Random.Range(0, ChunkSize));
        return pos;
    }

    public void SetBlock(byte x, byte y, Block block)
    {
        var pos = new Vector2Int(x, y);
        if (block.IsDefault)
            block = Object.Instantiate(block.gameObject, (Vector2)(pos + Location), Quaternion.identity).GetComponent<Block>();
        else
            block.transform.position = (Vector2)(pos + Location);
        
        block.transform.parent = gameObject.transform;
        _blocks[x, y] = block;
    }
    public Block GetBlock(byte x, byte y) => _blocks[x, y];
}
