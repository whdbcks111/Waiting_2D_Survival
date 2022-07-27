using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _maxHealthPoint = 50F;
    protected float _healthPoint;
    
    [SerializeField]
    protected DropInfo[] _dropItems;


    private GameObject _bar;

    private GameObject _back;

    protected SpriteRenderer _spriteRenderer;

    private Vector2 _beforePos;
    private ChunkCoord _chunkCoord;

    private void Awake()
    {
        _healthPoint = _maxHealthPoint;
        _back = Instantiate(Resources.Load<GameObject>("EnemyHpBar"), transform);
        _bar = _back.transform.GetChild(0).gameObject;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _beforePos = transform.position;
    }

    public virtual void Damaged(float damage) {
        _back.SetActive(true);
        StartCoroutine(nameof(HealthDisplay));
    }

    private void Update()
    {
        AI();
        OutOfChunkCheck();
        _beforePos = transform.position;
    }

    private void OutOfChunkCheck()
    {
        var pos = Vector2Int.FloorToInt(transform.position);
        var coord = new ChunkCoord(pos.x >> 4, pos.y >> 4);
        if (coord == _chunkCoord) return;

        var chunk = WorldGenerator.Instance.World.GetLoadedChunk(coord);
        if(chunk == null)
        {
            transform.position = _beforePos;
            return;
        }

        transform.parent = chunk.gameObject.transform;
        _chunkCoord = coord;
    }

    protected virtual void AI() { }

    private IEnumerator HealthDisplay() {
        float targetX = Mathf.Clamp01(_healthPoint / (float)_maxHealthPoint);
        var scale = _bar.transform.localScale;
        while(scale.x > targetX + 0.01) {
            yield return null;
            scale.x = Mathf.Lerp(scale.x, targetX, Time.deltaTime * 8);
            _bar.transform.localScale = scale;
        }
        if (_healthPoint <= 0) Die();
    }
    public virtual void Die() { }
}
