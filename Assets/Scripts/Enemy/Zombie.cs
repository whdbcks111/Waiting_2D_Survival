using System.Collections;
using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField]
    private float _moveSpeed = 2.8F;
    [SerializeField]
    private float _lookRadius = 5.8F;


    private float _moveChangeTime;
    private Vector2 _movePos;

    [SerializeField]
    private float _attackTimer = 1.9F;
    private float _attackTime;

    private State _state = State.None;
    private GameObject _target;

    [SerializeField]
    private float _damagePower = 7F;

    private float _walkTimer;

    protected override void AI()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        var nearGameObjects = Physics2D.OverlapCircleAll(transform.position, _lookRadius);
        var beforeState = _state;
        _state = State.None;
        foreach (var col in nearGameObjects)
        {
            if (col.tag == "Player")
            {
                _state = State.Player;
                if (beforeState != State.Player)
                {
                    AudioManager.Instance.PlayOneShot("detect", 0.4f);
                }
                _target = col.gameObject;
                break;
            }
            if (_state != State.None && _state != State.Fence) continue;

            _state = col.tag switch
            {
                "Seed" => State.Seed,
                "Fence" => State.Fence,
                _ => State.None,
            };

            _target = col.gameObject;
        }

        switch (_state)
        {
            case State.Player:
                _walkTimer += Time.deltaTime;
                if (_walkTimer >= .5)
                {
                    AudioManager.Instance.PlayOneShot("zombie_walk",
                        Mathf.Clamp01(1 - Vector2.Distance(transform.position, Player.Instance.transform.position) / _lookRadius));
                    _walkTimer = 0;
                }
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);
                break;
            case State.Seed:
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);
                break;
            case State.None:
                _moveChangeTime += Time.deltaTime;
                if (_moveChangeTime >= 3)
                {
                    _movePos = new Vector2(Random.Range(-.5F, .5F), Random.Range(-.5F, .5F));
                    _moveChangeTime = 0;
                }
                transform.Translate(_movePos * _moveSpeed * Time.deltaTime);
                _target = null;
                break;
        }
    }

    private void Attack()
    {
        _attackTime += Time.deltaTime;
        if (_attackTime <= _attackTimer) return;
        if (_state == State.None) return;
        if (Vector2.Distance(transform.position, _target.transform.position) > .6F) return;
        _attackTime = 0;
        AudioManager.Instance.PlayOneShot("bite", .5f);


        switch (_state)
        {
            case State.Player:
                Player.Instance.HealthPoint -= _damagePower;
                AudioManager.Instance.PlayOneShot("bite", .5f);
                if (Random.Range(0, 1) < 0.1) Player.Instance.AddEffect(new(StatusEffectInfo.Blood, 1, 60));
                break;
            case State.Seed:
            case State.Fence:
                _target.GetComponent<BreakableBlock>()?.Hit(false);
                break;
        }
    }

    public override void Damaged(float damage)
    {
        AudioManager.Instance.PlayOneShot("zombie_hit", .7f);
        _healthPoint -= damage;
        StartCoroutine(nameof(HitDisplay));
        base.Damaged(damage);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Fence") return;
        if (_attackTime <= _attackTimer) return;
        AudioManager.Instance.PlayOneShot("bite", .5f);
        collision.gameObject.GetComponent<BreakableBlock>()?.Hit(false);
        _attackTime = 0;
    }

    public override void Die()
    {
        foreach (var item in _dropItems)
        {
            if (Random.Range(0, 1F) < item.Percent)
                Player.Instance.Inventory.AddItem(item.Item);
        }
        Destroy(gameObject);
    }

    private IEnumerator HitDisplay()
    {
        var color = _spriteRenderer.color;
        var changeColor = _spriteRenderer.color;
        changeColor.a = .5F;
        _spriteRenderer.color = changeColor;
        yield return new WaitForSeconds(.1F);
        _spriteRenderer.color = color;
    }

    private enum State
    {
        None,
        Player,
        Seed,
        Fence
    }
}
