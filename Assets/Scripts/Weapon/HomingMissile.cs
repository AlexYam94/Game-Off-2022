using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] int _damage = 5;
    [SerializeField] float _startChaseAfter = .1f;
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] AudioSource _explosionSound;
    [SerializeField] LayerMask _hitLayer;
    [SerializeField] LayerMask _damageLayer;
    [SerializeField] float _explosionArea = 3f;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] GameObject _smoke;
    [SerializeField] GameObject _light;
    [SerializeField] GameObject _explosionEffect;

    public float speed = 30;
    public float rotateSpeed = 200f;
    public bool isHoming;
    public Vector2 direction { get; set; }
    public Transform target { get; set; }

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool startChasing = false;
    private bool _hit = false;
    public int damageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, _lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_hit) return;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x +   _explosionArea, transform.position.y + _explosionArea, 0));
        if (!startChasing)
        {
            flyForward();
        }
        if (isHoming && startChasing)
        {
            Vector2 direction = (Vector2)target.position - _rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rb.angularVelocity = -rotateAmount * rotateSpeed;
            _rb.velocity = transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if( ((1 << other.gameObject.layer) & _hitLayer) == (1 << other.gameObject.layer)){
            _explosionSound.gameObject.SetActive(true);
            _explosionSound.transform.SetParent(null);
            _sprite.enabled = false;
            _collider.enabled = false;
            _rb.velocity = Vector2.zero;
            _hit = true;
            //_smoke.SetActive(false);
            _light.SetActive(false);
            _explosionEffect.SetActive(true);
            ExplosionDamage();
        }
       
    }

    private void ExplosionDamage()
    {
        var hits = Physics2D.CircleCastAll(transform.position, _explosionArea, transform.forward,10, _damageLayer);
        foreach(var h in hits)
        {
            if(h.transform.gameObject.tag == "Enemy")
            {
                h.transform.gameObject.GetComponent<EnemyHealthController>().Damage(_damage);
            }
            else if(h.transform.gameObject.tag == "Player")
            {
                h.transform.gameObject.GetComponent<PlayerHealthController>().Damage(_damage);
            }
        }
    }

    public void flyForward()
    {
        _rb.velocity = (direction).normalized * speed;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
