using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private Vector2Int _leftRange = Vector2Int.zero;
    private Vector2Int _rightRange = Vector2Int.zero;

    [SerializeField]
    private float _spawnAmountPercent = .04F;
    
    [SerializeField]
    private Vector2Int _offset = new(50, 50);

    private void Awake()
    {
        _leftRange = _offset;
        _rightRange = -_offset;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRange();
    }

    private void PlayerRange()
    {
        var pos = (Vector2) Player.Instance.transform.position;
        var leftRange = Vector2Int.FloorToInt(pos - _offset);
        var rightRange = Vector2Int.CeilToInt(pos + _offset);

        if (leftRange.x < _leftRange.x || leftRange.y < _leftRange.y)
            SpawnRange(leftRange, _leftRange);
        
        if (rightRange.x > _rightRange.x || rightRange.y > _rightRange.y)
            SpawnRange(rightRange, _rightRange);

        _leftRange = leftRange;
        _rightRange = rightRange;
    }

    private void SpawnRange(Vector2 pos1, Vector2 pos2)
    {
        var blockAmount = Mathf.Abs((pos2.x - pos1.x) * (pos2.y - pos1.y));
        var spawnAmount = Mathf.RoundToInt(blockAmount * _spawnAmountPercent);
        for(var i = 0; i < spawnAmount; i++)
        {
            var x = Random.Range(pos1.x, pos2.x);
            var y = Random.Range(pos1.y, pos2.y);
            var pos = new Vector2(x, y);
            var block = Instantiate(Resources.Load("Bush"), pos, Quaternion.identity) as GameObject;
            block.transform.parent = transform;
        }
    }
}
