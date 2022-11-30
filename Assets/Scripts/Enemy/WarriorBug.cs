using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBug : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpRange = 10f;
    [SerializeField] float _jumpChance = .5f;
    [SerializeField] float _jumpUpwardForce = 10f;
    [SerializeField] float _jumpForwardForce = 15f;
    [SerializeField] Transform _groundDetectStartPosition;
    [SerializeField] float _groundDetectDistance = .7f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _jumpCooldown = 2f;
    [SerializeField] float _jumpSpread = .3f;

    CapsuleCollider2D _collider;
    Rigidbody2D _rb;
    GameObject _player;
    Animator _anim;
    float _jumpCooldownCounter = 0;
    private bool _isJumping = false;

    private float _scaleX;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        GetComponent<EnemyHealthController>().onDeath += Cleanup;
        _scaleX = transform.localScale.x;

        _collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isJumping)
        {
            _anim.SetBool("walk", true);
            _anim.SetBool("jump", false);
            if (transform.position.x < _player.transform.position.x)
            {
                flipX(true);
            }
            else
            {
                flipX(false);
            }
        }
        else
        {
            _anim.SetBool("walk", false);
            _anim.SetBool("jump", true);
        }
        if (IsGrounded())
        {
            _isJumping = false;
            float jumpChangeRollValue = Random.Range(0f, 1f);
            if (!_isJumping && (_jumpCooldownCounter > 0 || Vector2.Distance(transform.position, _player.transform.position) > _jumpRange || jumpChangeRollValue > _jumpChance))
            {
                MoveToPlayer();
            }
            else if (_jumpCooldownCounter <= 0 &&jumpChangeRollValue <= _jumpChance)
            {
                //JumpToPlayer();
                StartCoroutine(JumpToPlayer());
            }
            if (_jumpCooldownCounter > 0)
            {
                _jumpCooldownCounter -= Time.deltaTime;
            }
        }
    }

    private void OnEnable()
    {
        _groundDetectStartPosition.gameObject.SetActive(true);
    }

    private void MoveToPlayer()
    {
        if (transform.position.x < _player.transform.position.x)
        {
            _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
            flipX(true);
        }
        else
        {
            _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
            flipX(false);
        }
        //if(transform.position.y < _patrolPoints[_currentXPoint].position.y - .5f && _rb.velocity.y < .1f)
        //{
        //    _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        //}
    }

    private IEnumerator JumpToPlayer()
    {
        _isJumping = true;
        _jumpCooldownCounter = _jumpCooldown;
        _groundDetectStartPosition.gameObject.SetActive(false);
        float offset = 1 + Random.Range(-_jumpSpread, _jumpSpread);
        if (transform.position.x < _player.transform.position.x)
        {
            //_rb.velocity = new Vector2(_jumpForwardForce, _jumpUpwardForce);
            //_rb.AddForce(new Vector2(_jumpForwardForce, _jumpUpwardForce), ForceMode2D.Impulse);

            _rb.velocity = new Vector2(_rb.velocity.x, _jumpUpwardForce * offset);
            //_rb.AddForce(new Vector2(0, _jumpUpwardForce), ForceMode2D.Impulse);
            yield return null;
            _rb.velocity = new Vector2(_jumpForwardForce, _rb.velocity.y);
            //_rb.AddForce(new Vector2(_jumpForwardForce, 0), ForceMode2D.Impulse);
        }
        else
        {
            //_rb.velocity = new Vector2(-_jumpForwardForce, _jumpUpwardForce);
            //_rb.AddForce(new Vector2(-_jumpForwardForce, _jumpUpwardForce), ForceMode2D.Impulse);
            //_rb.AddForce(new Vector2(0, _jumpUpwardForce), ForceMode2D.Impulse);
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpUpwardForce * offset);
            yield return null;
            _rb.velocity = new Vector2(-_jumpForwardForce, _rb.velocity.y);
            //_rb.AddForce(new Vector2(-_jumpForwardForce, 0), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1f);
        _groundDetectStartPosition.gameObject.SetActive(true);
    }

    private void Cleanup()
    {
    }

    private void flipX(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? _scaleX : _scaleX * - 1;
        transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        if (!_groundDetectStartPosition.gameObject.activeSelf) return false;
        RaycastHit2D hitLeft;
        RaycastHit2D hitMiddle;
        RaycastHit2D hitRight;
        Vector2 middle = transform.position;
        Vector2 left = transform.position;
        Vector2 right = transform.position;
        middle.y = _groundDetectStartPosition.position.y;
        left.y = _groundDetectStartPosition.position.y;
        right.y = _groundDetectStartPosition.position.y;
        left.x += _collider.size.x / 2;
        right.x -= _collider.size.x / 2f;
        hitMiddle = Physics2D.Raycast(middle, Vector2.down, _groundDetectDistance, _groundLayer);
        hitLeft = Physics2D.Raycast(left, Vector2.down, _groundDetectDistance, _groundLayer);
        hitRight = Physics2D.Raycast(right, Vector2.down, _groundDetectDistance, _groundLayer);
        Debug.DrawRay(middle, Vector2.down * _groundDetectDistance, Color.red);
        Debug.DrawRay(left, Vector2.down * _groundDetectDistance, Color.red);
        Debug.DrawRay(right, Vector2.down * _groundDetectDistance, Color.red);
        bool isGround = hitLeft.collider != null || hitMiddle.collider != null || hitRight.collider != null;
        //_playerAnimation.Jump(!isGround);
        return isGround;

    }

}
