using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] Transform _groundDetectStartPosition;
    [SerializeField] float _groundDetectDistance = .7f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] CapsuleCollider2D _collider;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashTime;
    [SerializeField] float _waitAfterDashing;
    [SerializeField] GameObject _standing;
    [SerializeField] GameObject _ball;
    [SerializeField] float _waitToBall;
    [SerializeField] Transform _standDetectStartPosition;
    [SerializeField] float _standDetectDistance = .2f;
    [SerializeField] float _ballSpeedMultiplier = .8f;
    [SerializeField] CircleCollider2D _ballCollider;
    [SerializeField] float _coyoteTime = .3f;
    [SerializeField] float _thrustSpeedMutiplier = 3f;
    [SerializeField] AudioSource _footStepAudioSource;
    [SerializeField] float _footStepDecayTime = .5f;

    private float _coyoteCounter;
    bool _resetJumpNeeded = false;
    PlayerAnimation _playerAnimation;
    FireController _fireController;
    bool _canDoubleJump = false;
    float _dashCounter;
    PlayerEffectController _playerEffectController;
    float _dashRechargeCounter;
    float _ballCounter;
    bool _canStand;
    AbilitiesController _abilitiesController;
    ThrusterController _thrusterController;
    bool _canInput;
    Animation _anim;
    private float _speedScale = 1;
    private float _jumpForceScale = 1;
    private bool _invertedControl = false;
    private bool _canStop = true;
    private bool _isVisible = true;
    private float _footStepDecayCounter = 0f;
    private bool _startFootstep = false;

    public float batteryCapacity = 100f;
    public bool isInMecha = false;

    public float gravityScale = 50f;
    public bool canFly = true;
    public bool isGrounded = true;

    private void Start()
    {
        ScoreController.GetInstance()?.ResetScore();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _fireController = GetComponent<FireController>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        //SwitchToStanding();
        _abilitiesController = GetComponent<AbilitiesController>();
        _canInput = true;
        _canStand = true;
        _invertedControl = false;
        _canStop = true;
        _coyoteCounter = _coyoteTime;
        _isVisible = true;
        _footStepAudioSource = GetComponent<AudioSource>();
        _thrusterController = GetComponent<ThrusterController>();
}

    private void Update()
    {
        handleFootStep();
        if (!_canInput)
        {
            _rb.velocity = new Vector2(0, -50);
            //_playerAnimation.Move(0);
            //_playerAnimation.Jump(false);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal") * (_invertedControl? -1 : 1);
        if (horizontal < 0)
        {
            Flip(false);
        }
        else if (horizontal > 0)
        {
            Flip(true);
        }

        _playerAnimation.RestoreAnimator();

        if (!isInMecha)
        {
            Move();
        }
        else
        {
            MechaMove();
        }
        //if (!Input.GetKey(KeyCode.W))
        //{
        //    Move();
        //}
        //else
        //{
        //    _rb.velocity = new Vector2(0, _rb.velocity.y);
        //}
    }

    private void FixedUpdate()
    {
    }

    private void handleFootStep()
    {
        if (_startFootstep)
        {
            _footStepDecayCounter = Mathf.Clamp(Time.deltaTime + _footStepDecayCounter, 0, _footStepDecayTime);
        }
        else
        {
            _footStepDecayCounter = Mathf.Clamp(_footStepDecayCounter - Time.deltaTime, 0, _footStepDecayTime);

        }
        _footStepAudioSource.volume = _footStepDecayCounter / _footStepDecayTime;
    }


    private void MechaMove()
    {
        bool isFlying = false;
        if (_thrusterController._currentEnergy>0 && Input.GetKey(KeyCode.LeftShift))
        {
            isFlying = true;
            _speedScale = _thrustSpeedMutiplier;
            _rb.gravityScale = 0;
            _playerAnimation.Fly();
        }
        else if (_thrusterController._currentEnergy > 0 && Input.GetKey(KeyCode.Space))
        {
            isFlying = true;
            _speedScale = 1;
            _rb.gravityScale = 0;
            _playerAnimation.Fly();
        }
        else
        {
            _speedScale = 1;
            _rb.gravityScale = gravityScale;
            _playerAnimation.NotFly();
        }
        /* dash */
        _canStand = canStand();
        if (_dashRechargeCounter > 0)
        {
            _dashRechargeCounter = Mathf.Max(_dashRechargeCounter - Time.deltaTime, 0);
        }
        //else if (_abilitiesController.canDash && Input.GetButtonDown("Fire2") && _canStand)
        else if (_abilitiesController.canDash && Input.GetKeyDown(KeyCode.Mouse1) && _canStand)
        {
            _dashCounter = _dashTime;
            _dashRechargeCounter = _waitAfterDashing;
            if (_isVisible)
            {
                _playerEffectController.ShowAfterImage(_playerSprite);
            }
            //SwitchToStanding();
        }

        if (_dashCounter > 0)
        {
            _dashCounter -= Time.deltaTime;
            _rb.velocity = new Vector2(_dashSpeed * transform.localScale.x, 0);
            _playerEffectController.CountDown(Time.deltaTime);
            if (_playerEffectController.GetAfterImageCounter() <= 0)
            {
                if (_isVisible)
                {
                    _playerEffectController.ShowAfterImage(_playerSprite);
                }
            }
            return;
        }
        /* dash */

        isGrounded = IsGrounded();
        Vector2 velocity = _rb.velocity;
        float horizontal = Input.GetAxisRaw("Horizontal") * (_invertedControl ? -1 : 1);
        float vertical = Input.GetAxisRaw("Vertical") * (_invertedControl ? -1 : 1);
        DecideFlyDirection(horizontal,vertical);
        if (horizontal != 0 && isGrounded && !isFlying)
        {
            _startFootstep = true;
        }
        else
        {
            _startFootstep = false;
        }
        if (_canStop)
        {
            if (horizontal != 0)
                velocity.x = horizontal * _moveSpeed * _speedScale;
            if (vertical != 0 && isFlying)
                velocity.y = vertical * _moveSpeed * _speedScale;
        }
        else
        {
            if (horizontal != 0 || Input.GetKey(KeyCode.LeftShift))
            {
                if (!isFlying)
                    _footStepAudioSource.enabled = true;
                if (horizontal != 0)
                    velocity.x = horizontal * _moveSpeed * _speedScale;
                if (vertical != 0 && isFlying)
                    velocity.y = vertical * _moveSpeed * _speedScale;

            }
            else
            {
                _footStepAudioSource.enabled = false;
            }
        }

        if (isGrounded)
        {
            _playerAnimation.Move(horizontal);
        }

        if (!isGrounded)
        {
            _coyoteCounter -= Time.deltaTime;
            _playerAnimation.SetCoyote(true);
        }
        else
        {
            _coyoteCounter = _coyoteTime;
            _playerAnimation.SetCoyote(false);
        }

        // Jump
        if ((_coyoteCounter > 0) && Input.GetButtonDown("Jump"))
        {
            velocity.y = _jumpForce * _jumpForceScale;

            _playerAnimation.Jump(!isGrounded);
            //if (grounded)
            if (isGrounded || _coyoteCounter > 0)
            {
                _coyoteCounter = 0;
                //StartCoroutine("ResetJump");
            }
        }

        _rb.velocity = velocity;
    }

    private void DecideFlyDirection(float horizontal, float vertical)
    {
        float xScale = transform.localScale.x;
        if ((horizontal > 0 && xScale > 0) || (horizontal < 0 && xScale < 0))
        {
            _playerAnimation.FlyForward();
        }
        else if ((horizontal < 0 && xScale > 0) || (horizontal > 0 && xScale < 0))
        {
            _playerAnimation.FlyBackward();
        }
        else if (horizontal == 0 && vertical > 0)
        {
            _playerAnimation.FlyUpward();
        }else if (horizontal == 0 && vertical < 0)
        {
            _playerAnimation.FlyDownward();
        }
        else if(!Input.GetKey(KeyCode.LeftShift) || ( horizontal ==0 && vertical == 0))
        {
            _playerAnimation.Hover();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedScale = 2;
        }
        else
        {
            _speedScale = 1;
        }
        /* dash */
        _canStand = canStand();
        if (_dashRechargeCounter > 0)
        {
            _dashRechargeCounter = Mathf.Max(_dashRechargeCounter - Time.deltaTime, 0);
        }
        //else if (_abilitiesController.canDash && Input.GetButtonDown("Fire2") && _canStand)
        else if (_abilitiesController.canDash && Input.GetKeyDown(KeyCode.LeftShift) && _canStand)
        {
            _dashCounter = _dashTime;
            _dashRechargeCounter = _waitAfterDashing;
            //if (_isVisible)
            //{
            //    _playerEffectController.ShowAfterImage(_playerSprite);
            //}
            //SwitchToStanding();
        }

        if (_dashCounter > 0)
        {
            _dashCounter -= Time.deltaTime;
            _rb.velocity = new Vector2(_dashSpeed * transform.localScale.x, 0);
            _playerEffectController.CountDown(Time.deltaTime);
            if (_playerEffectController.GetAfterImageCounter() <= 0)
            {
                if (_isVisible)
                {
                    _playerEffectController.ShowAfterImage(_playerSprite);
                }
            }
            return;
        }
        /* dash */

        bool grounded = IsGrounded();
        Vector2 velocity = _rb.velocity;
        float horizontal = Input.GetAxisRaw("Horizontal") * (_invertedControl ? -1 : 1);
        if (_ball.activeSelf)
        {
            velocity.x = horizontal * _moveSpeed * _ballSpeedMultiplier;
        }
        else
        {
            if (horizontal != 0 && grounded)
            {
                _footStepAudioSource.enabled = true;
            }
            else
            {
                _footStepAudioSource.enabled = false;
            }
            if (_canStop)
            {
                velocity.x = horizontal * _moveSpeed * _speedScale;
            }
            else
            {
                if (horizontal != 0 || Input.GetKey(KeyCode.LeftShift))
                {
                    _footStepAudioSource.enabled = true;
                    velocity.x = horizontal * _moveSpeed * _speedScale;
                }
                else
                {
                    _footStepAudioSource.enabled = false;
                }
            }
        }
        _playerAnimation.Move(horizontal);

        if (!grounded)
        {
            _coyoteCounter -= Time.deltaTime;
            _playerAnimation.SetCoyote(true);
        }
        else
        {
            _coyoteCounter = _coyoteTime;
            _playerAnimation.SetCoyote(false);
        }
        //if (_coyoteCounter > 0)
        //{
        //    _canDoubleJump = false;
        //        }

        // Jump
        if (((_abilitiesController.canDoubleJump && _canDoubleJump) || (_coyoteCounter > 0)) && Input.GetButtonDown("Jump"))
        {
            if (_ball.activeSelf)
            {
                velocity.y = _jumpForce * _ballSpeedMultiplier;
            }
            else
            {
                velocity.y = _jumpForce * _jumpForceScale;
            }

            _playerAnimation.Jump(!grounded);
            //if (grounded)
            if (grounded || _coyoteCounter > 0)
            {
                _coyoteCounter = 0;
                //StartCoroutine("ResetJump");
                _canDoubleJump = true;
            }
            else
            {
                _playerAnimation.SetCoyote(false);
                _playerAnimation.DoubleJump();
                _canDoubleJump = false;
            }
        }

        _rb.velocity = velocity;
    }

    private void Flip(bool facingRight)
    {
        //_playerSprite.flipX = !facingRight;
        //Vector3 scale = transform.localScale;
        //if (!facingRight) scale.x = System.Math.Abs(scale.x) * -1;
        //else scale.x = System.Math.Abs(scale.x);

        //transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitLeft;
        RaycastHit2D hitMiddle;
        RaycastHit2D hitRight;
        Vector2 middle = transform.position;
        Vector2 left = transform.position;
        Vector2 right = transform.position;
        middle.y = _groundDetectStartPosition .position.y;
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
        _playerAnimation.Jump(!isGround);
        return isGround;

    }

    public bool canStand()
    {
        RaycastHit2D hitLeft;
        RaycastHit2D hitMiddle;
        RaycastHit2D hitRight;
        Vector2 middle = _standDetectStartPosition.position;
        Vector2 left = _standDetectStartPosition.position;
        Vector2 right = _standDetectStartPosition.position;
        float radius = _ballCollider.radius;
        left.x += radius;
        right.x -= radius;
        hitMiddle = Physics2D.Raycast(middle, Vector2.down, _standDetectDistance, _groundLayer);
        hitLeft = Physics2D.Raycast(left, Vector2.down, _standDetectDistance, _groundLayer);
        hitRight = Physics2D.Raycast(right, Vector2.down, _standDetectDistance, _groundLayer);
        Debug.DrawRay(middle, Vector2.down * _standDetectDistance, Color.red);
        Debug.DrawRay(left, Vector2.down * _standDetectDistance, Color.red);
        Debug.DrawRay(right, Vector2.down * _standDetectDistance, Color.red);
        bool canStand = hitLeft.collider == null && hitMiddle.collider == null && hitRight.collider == null;
        return canStand;
    }

    private void SwitchToStanding()
    {
        _standing.SetActive(true);
        _ball.SetActive(false);
        _playerAnimation.DeativateBall();
        _fireController._isStanding = true;
    }

    private void SwitchToBall()
    {
        _playerAnimation.ActivateBall();
        _standing.SetActive(false);
        _ball.SetActive(true);
        _fireController._isStanding = false;
    }

    public void DisableInput()
    {
        _fireController.enabled = false;
        _canInput = false;
    }

    public void EnableInput()
    {
        _fireController.enabled = true;
        _canInput = true;
    }

    public void FreezeSprite()
    {
        _playerAnimation.DisableAnimator();
    }

    public void UnfreezeSprite()
    {
        _playerAnimation.EnableAnimator();
    }

    public bool CanPlayerInput()
    {
        return _canInput;
    }

    public void SetSpeedScale(float scale)
    {
        _speedScale = scale;
    }
    public void SetJumpForceScale(float scale)
    {
        _jumpForceScale = scale;
    }

    public void ToggleInvertControl()
    {
        _invertedControl = !_invertedControl;
    }

    public void ToggleCanStop()
    {
        _canStop = !_canStop;
    }

    public void ToggleInvisible()
    {
        _isVisible = !_isVisible;
    }

    public bool IsVisible()
    {
        return _isVisible;
    }
}
