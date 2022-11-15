using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] public float bulletSpeed;
    [SerializeField] ParticleSystem _impactEffect;
    [SerializeField] float _lifeTime = 10f;
    [SerializeField] int _damageAmount = 1;
    [SerializeField] int _penetrateAmount = 1;
    [SerializeField] int _groundLayer;
    [SerializeField] float _knockbackForce = 5f;
    public int _hitPauseFrame = 5;

    public float damageMultiplier = 1f;
    public bool canFly = true;


    SpriteRenderer _sprite;
    Collider2D _collider;
    TrailRenderer _trail;
    ObjectPool<Bullet> _pool;
    int _penetrateCount = 0;

    Rigidbody2D _rb;
    //Direction _direction;
    Vector3 _direction;
    Vector2 _moveDir = Vector2.right;
    Vector3 _lastFramePos;

    public void SetObjectPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    public ObjectPool<Bullet> GetObjectPool()
    {
        return _pool;
    }

    //public void SetDirection(Direction direction)
    //{
    //    _direction = direction;
    //}
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _trail = GetComponentInChildren<TrailRenderer>();
        Destroy(gameObject, _lifeTime);
        _lastFramePos = transform.position;
        StartCoroutine(GetLastFramePos());
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if(canFly)
            _rb.velocity = (_direction).normalized * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>()?.Damage(_damageAmount * damageMultiplier); 
            _penetrateCount++;
            StartCoroutine(HitPause());
            KnockBack(other.transform);
        }
        //_pool.Enqueue(this);
        //gameObject.SetActive(false);

        if ((other.gameObject.layer == _groundLayer) || _penetrateCount > _penetrateAmount)
        {
            Destroy(gameObject, 1);
            //gameObject.SetActive(false);
            _sprite.enabled = false;
            _collider.enabled = false;
            _trail.enabled = false;
            if (_impactEffect != null)
            {
                //TODO: Play hit sound
                Instantiate(_impactEffect, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnBecameInvisible()
    {
        //_pool.Enqueue(this);
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void KnockBack(Transform other)
    {
        //var direction = (other.position - transform.position).normalized;
        var direction = (transform.position - other.transform.position).normalized;

        other.gameObject.GetComponent<Rigidbody2D>()?.AddForce(direction.normalized * _knockbackForce, ForceMode2D.Force);
    }

    IEnumerator HitPause()
    {
        Time.timeScale = .1f;
        int i = 0;
        while(i < _hitPauseFrame)
        {
            yield return null;
            i++;
        }
        Time.timeScale = 1f;
    }

    IEnumerator GetLastFramePos()
    {
        while (true)
        {
            _lastFramePos = transform.position;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }
}
