using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator Instance { get; private set; }
    
    private Vector2Int _leftRange = Vector2Int.zero;
    private Vector2Int _rightRange = Vector2Int.zero;

    [SerializeField]
    private float _spawnAmountPercent = .04F;
    public float SpawnAmountPercent => _spawnAmountPercent;
    
    [SerializeField]
    private Vector2Int _offset = new(30, 30);

    [SerializeField]
    internal float ZombieSpawnTimer = 8F;
    [SerializeField]
    private float ZombieSpawnRange = 13F;
    [SerializeField]
    private float ZombieSpawnPercent = .6F;
    [SerializeField]
    private int ZombieSpawnMaxAmount = 4;

    [SerializeField]
    private GameObject[] _zombies;

    [SerializeField]
    private float _spawnStonePercent = .33F;
    public float SpawnStonePercent => _spawnStonePercent;

    [SerializeField]
    private float _spawnSpecialBuilding = .001F;

    public World World { get; private set; }

    [SerializeField]
    private byte _viewDistance = 2;
    public byte ViewDistance => _viewDistance;

    public List<ChunkCoord> _activedChunks = new();
    

    private void Awake()
    {
        Instance = this;
        World = new World(this);
    }

    private void Start()
    {
        StartCoroutine(nameof(SpawnZombie));
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRange();
    }

    public Chunk GetChunk(ChunkCoord coord)
    {
        //Chunk Save File Check
        return new Chunk(World, coord);
    }

    private void PlayerRange()
    {
        var pos = Vector2Int.FloorToInt(Player.Instance.transform.position);
        var coord = new ChunkCoord(pos.x >> 4, pos.y >> 4);
        if (coord == Player.Instance._chunkCoord) return;

        var chunks = new List<ChunkCoord>();
        for (var y = coord.Y - ViewDistance; y <= coord.Y + ViewDistance; y++)
        {
            for (var x = coord.X - ViewDistance; x <= coord.X + ViewDistance; x++)
            {
                var chunkCoord = new ChunkCoord(x, y);
                _activedChunks.Remove(chunkCoord);
                chunks.Add(chunkCoord);
                
                var chunk = World.GetChunk(chunkCoord);
                if (!chunk.gameObject.activeSelf)
                    chunk.gameObject.SetActive(true);
            }
        }

        foreach (var chunk in _activedChunks)
        {
            var chunkObject = World.GetChunk(chunk);
            chunkObject.gameObject.SetActive(false);
        }

        _activedChunks = chunks;
        Player.Instance._chunkCoord = coord;
    }

    public bool IsAnyBlockHere(Vector2 pos)
    {
        return World.GetBlock(Vector2Int.FloorToInt(pos)) != null;
    }

    private IEnumerator SpawnZombie()
    {
        var time = ZombieSpawnTimer;
        var timer = new WaitForSeconds(time);
        while (true)
        {
            if (time != ZombieSpawnTimer)
            {
                time = ZombieSpawnTimer;
                timer = new WaitForSeconds(time);
            }
            yield return timer;

            if (Random.Range(0, 10F) * .1F > ZombieSpawnPercent) continue;
            
            var amount = Random.Range(1, ZombieSpawnMaxAmount);
            for(var i = 0; i < amount; i++)
            {
                var pos = new Vector2(Random.Range(0, 2) * 2 - 1, Random.Range(0, 2) * 2 - 1) * ZombieSpawnRange;
                var zombie = Instantiate(_zombies[Random.Range(0, _zombies.Length)], pos + (Vector2) Player.Instance.transform.position, Quaternion.identity) as GameObject;
                zombie.transform.parent = transform;
            }
        }
    }
}
