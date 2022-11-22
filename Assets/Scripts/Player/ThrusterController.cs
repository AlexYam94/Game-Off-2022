using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    [SerializeField] AudioSource _thrusterSound;
    [SerializeField] Animator[] _thrusters;
    [SerializeField] float _thrusterStartTime = .5f;
    [SerializeField] float _maxEnergy = 100f;
    [SerializeField] float _flyEnergyPerSecond = 10f;
    [SerializeField] float _energyRecoverPerSecond = 15f;
    [SerializeField] float _startEnergyRecoverDelay = 2f;

    PlayerController _playerController;

    private bool _startThruster = false;
    private float _thrusterStartCounter = 0f;
    float _startEnergyRecoverDelayCounter = 0f;
    private bool _isFlying = false;

    public float _currentEnergy;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        //StartCoroutine(RegenerateEnergy());
        _currentEnergy = _maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
        {
            _isFlying = true;
        }
        else
        {
            _isFlying = false;
        }

        //if (_playerController.isGrounded && !_isFlying)
        if (!_isFlying)
        {
            _startEnergyRecoverDelayCounter += Time.deltaTime;
        }
        else
        {
            _startEnergyRecoverDelayCounter = 0;
        }

        if (_startEnergyRecoverDelayCounter >= _startEnergyRecoverDelay && _currentEnergy < _maxEnergy)
        {
            _currentEnergy = Mathf.Min(_maxEnergy, _currentEnergy + _energyRecoverPerSecond / 10 * Time.deltaTime);
        }

            if (_startThruster)
        {
            _thrusterStartCounter = Mathf.Clamp(Time.deltaTime + _thrusterStartCounter, 0, _thrusterStartTime);
        }
        else
        {
            _thrusterStartCounter = Mathf.Clamp(_thrusterStartCounter - Time.deltaTime, 0, _thrusterStartTime);

        }
        _thrusterSound.volume = _thrusterStartCounter / _thrusterStartTime;

        if (_isFlying && _currentEnergy > 0)
        {
            _currentEnergy = Mathf.Max(0, _currentEnergy - _flyEnergyPerSecond * Time.deltaTime);
            foreach (var a in _thrusters)
            {
                a.gameObject.SetActive(true);
            }
            _startThruster = true;
        }
        else
        {
            _startThruster = false;
            foreach (var a in _thrusters)
            {
                a.gameObject.SetActive(false);
            }
        }
        
    }
    IEnumerator RegenerateEnergy()
    {
        while(true)
        {
            if (_startEnergyRecoverDelayCounter >= _startEnergyRecoverDelay && _currentEnergy < _maxEnergy)
            {
                _currentEnergy += Mathf.Min(_maxEnergy, _energyRecoverPerSecond / 10 * Time.deltaTime);
                yield return new WaitForSeconds(.1f);
            }
            yield return null;
        }
    }
}
