using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static Bullet;
using static FirePattern;

public class FireController : MonoBehaviour
{
    //[SerializeField] Bullet _bullectPrefab;
    [SerializeField] Transform _firePosition;
    [SerializeField] Transform _fireUpPosition;
    [SerializeField] GameObject _bomb;
    [SerializeField] Transform _bombPosition;
    [SerializeField] int _bulletPoolInitialCount = 10;
    [SerializeField] AudioClip _emptyGunShot;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] Light2D _fireLight;
    [SerializeField] CinemachineVirtualCamera _virtCam;
    [SerializeField] Slider _reloadSlider;

    public Weapon _currentWeapon;

    public int poolCount = 0;

    public float capacityMultiplier = 1;

    public float reloadTimeMultiplier = 1;

    public float fireRateMultiplier = 1f;

    float _reloadTimeCounter;
    private int bulletCount;
    //ObjectPool<Bullet> _bulletPool;
    PlayerAnimation _playerAnimation;
    Transform _whereToFire;
    FirePattern _pattern;
    float _fireCounter = 0;

    //IEnumerator _currentReloadCoroutine;

    private UpgradeController _upgradeController;

    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;




    private bool _canFire = true;
    private bool _reloading = false;

    public bool _isStanding { get; set; } = true;

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _whereToFire = _firePosition;
        //_bulletPool = new ObjectPool<Bullet>(CreateBullet, _bullectPrefab, _firePosition, _bulletPoolInitialCount);
        bulletCount = Mathf.CeilToInt(_currentWeapon.capacity * capacityMultiplier);
        initialPosition = Camera.main.transform.localPosition;
        _cinemachineBasicMultiChannelPerlin = _virtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _upgradeController = GetComponent<UpgradeController>();
    }

    private void OnLevelWasLoaded()
    {
        //_bulletPool = new ObjectPool<Bullet>(CreateBullet, _bullectPrefab, _firePosition, _bulletPoolInitialCount);
    }


    private void OnDisable()
    {
        //_bulletPool.ClearPool();
    }

    private void Update()
    {
        _ammoText.text = bulletCount + "/" + Mathf.CeilToInt(_currentWeapon.capacity * capacityMultiplier);
        //poolCount = _bulletPool.GetCount();
        //DecideFirePosition();
        if (Input.GetKeyDown(KeyCode.R) && bulletCount < Mathf.CeilToInt(_currentWeapon.capacity * capacityMultiplier))
        {
            TriggerReload();
        }
        if (_reloadTimeCounter > 0)
        {
            _reloadSlider.value = _reloadTimeCounter / (_currentWeapon.reloadTime * reloadTimeMultiplier);
            _reloadTimeCounter -= Time.deltaTime;
            return;
        }
        else if(_reloading && _reloadTimeCounter <= 0)
        {
            _reloadSlider.gameObject.SetActive(false);
            _reloading = false;
            bulletCount = Mathf.CeilToInt(_currentWeapon.capacity * capacityMultiplier);
        }
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
        {
            if (bulletCount > 0)
            {
                if (_fireCounter <= 0)
                {
                    _fireCounter = _currentWeapon.fireRate * fireRateMultiplier;
                    if (bulletCount <= 0)
                    {
                        _audioSource.PlayOneShot(_emptyGunShot);
                    }
                    else
                    {
                        if (_isStanding && _canFire)
                        {
                            Shoot();
                        }
                    }
                }
            }
            else
            {
                TriggerReload();
            }
        }
        _fireCounter = Mathf.Max(0, _fireCounter - Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _canFire = true;
        }
        //if (bulletCount <= 0)
        //{
        //    _currentReloadCoroutine = Reload();
        //    StartCoroutine(_currentReloadCoroutine);
        //}

    }

    private void TriggerReload()
    {
        if (_reloading) return;
        _reloadTimeCounter = _currentWeapon.reloadTime * reloadTimeMultiplier;
        _reloadSlider.gameObject.SetActive(true);
        _reloadSlider.value = 0;
        _reloading = true;
    }

    private void Shoot()
    {
        /*
            FirePatternDirection[] firePatternDirections = _pattern.directions;
            foreach(FirePatternDirection pattern in firePatternDirections)
            {
                Bullet bullet = _bulletPool.Dequeue();
                //bullet.SetDirection(ConvertFirePatternDirectionToBulletDirection(pattern));
                bullet.SetDirection(_firePosition.transform.right);
                bullet.transform.position = _whereToFire.position;
                if (bullet.GetObjectPool() == null)
                {
                    bullet.SetObjectPool(_bulletPool);
                }

            }
        */
        StartCoroutine(FireEffect());
        if (!_currentWeapon.autoMode)
        {
            _canFire = false;
        }

        _currentWeapon.Fire().Invoke(_firePosition,_firePosition.transform.right, _upgradeController.damageMultiplier);
        _playerAnimation.Shoot();
        _audioSource.PlayOneShot(_currentWeapon.gunshot);
        bulletCount -= 1;
        shakeDuration = .2f;

    }

    IEnumerator FireEffect()
    {
        _fireLight.gameObject.SetActive(true);
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _currentWeapon.cameraShakeMagnitude;
        yield return new WaitForSeconds(.05f);
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _fireLight.gameObject.SetActive(false);

    }

    private void DecideFirePosition()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _whereToFire = _fireUpPosition;
        }
        else
        {
            _whereToFire = _firePosition;
        }
    }

    private Bullet CreateBullet(Bullet gameObject, Transform transform)
    {
        Bullet bullet = GameObject.Instantiate(gameObject, transform.position, transform.rotation);
        bullet.gameObject.SetActive(false);
        //bullet.SetObjectPool(_bulletPool);
        //DontDestroyOnLoad(bullet);
        return bullet;
    }

    public void DisableFire()
    {
        _canFire = false;

    }

    public void EnableFire()
    {
        _canFire = true;
    }

    public void ChangeWeapon(Weapon nextWeapon)
    {
        bulletCount = Mathf.CeilToInt(nextWeapon.capacity * capacityMultiplier);
        _currentWeapon = nextWeapon;
        //StopCoroutine(_currentReloadCoroutine);
        _playerAnimation.OverrideArmController(nextWeapon.weaponAnimatorController);
        _fireCounter = 0;
    }

    public void Reload(float percentage)
    {
        bulletCount += (int)Mathf.Ceil(_currentWeapon.capacity * capacityMultiplier * percentage);
    }
}
