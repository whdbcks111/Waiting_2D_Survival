using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FarmBlock : BreakableBlock
{
    [SerializeField]
    private float _growingTime = 50;

    [SerializeField]
    private Sprite[] _images;

    private float _splitTime;

    private int _level;

    [SerializeField]
    private ItemInfo _item;
    [SerializeField]
    private ItemInfo _seeds;

    void Start()
    {
        _splitTime = _growingTime / (_images.Length - 1);
        StartCoroutine(nameof(Grow));
    }

    private IEnumerator Grow()
    {
        var growTime = new WaitForSeconds(_splitTime);
        for (int i = 0; i < _images.Length - 1; i++)
        {
            //_images[i];
            _spriteRenderer.sprite = _images[i];
            _level++;
            yield return growTime;
        }
        _spriteRenderer.sprite = _images[_images.Length - 1];
        _level++;
    }
    
    public override void Hit(bool isPlayer)
    {
        base.Hit(isPlayer);
    }

    public override void Break(bool isPlayer)
    {
        if (_level >= _images.Length && isPlayer)
        {
            //TODO: Item
            Player.Instance.Inventory.AddItem(_item);
            Player.Instance.Inventory.AddItem(_seeds);
        }
        Destroy(gameObject);
    }
}
