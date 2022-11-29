using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterController : MonoBehaviour
{
    [SerializeField] AudioSource _thrusterSound;
    [SerializeField] Animator[] _thrusters;
    [SerializeField] float _thrusterStartTime = .5f;
    [SerializeField] float _energyRecoverPerSecond = 15f;
    [SerializeField] float _startEnergyRecoverDelay = 2f;
    [SerializeField] Slider _energyBar;
    [SerializeField] float _dashEnergyConsumptionMultiplier = 1.5f;

    public float maxEnergy = 100f;
    public float _flyEnergyPerSecond = 10f;

    PlayerController _playerController;

    private bool _startThruster = false;
    private float _thrusterStartCounter = 0f;
    float _startEnergyRecoverDelayCounter = 0f;
    private bool _isFlying = false;
    private float _energyConsumptionMultiplier = 1f;

    public float _currentEnergy;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        //StartCoroutine(RegenerateEnergy());
        _currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) return;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift)) {
                _energyConsumptionMultiplier = _dashEnergyConsumptionMultiplier;
            }
            else
            {
                _energyConsumptionMultiplier = 1f;
            }
            _isFlying = true;
        }
        else
        {
            _isFlying = false;
        }

        CheckStartRecoverEnergy();
        HandleEneryBar();
        RegenerateEnergy();
        HandleThrustSound();
        HandleThrustVisual();

    }

    private void HandleThrustVisual()
    {
        if (_isFlying && _currentEnergy > 0)
        {
            _currentEnergy = Mathf.Max(0, _currentEnergy - _flyEnergyPerSecond * _energyConsumptionMultiplier * Time.deltaTime);
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

    private void HandleThrustSound()
    {
        if (_startThruster)
        {
            _thrusterStartCounter = Mathf.Clamp(Time.deltaTime + _thrusterStartCounter, 0, _thrusterStartTime);
        }
        else
        {
            _thrusterStartCounter = Mathf.Clamp(_thrusterStartCounter - Time.deltaTime, 0, _thrusterStartTime);

        }
        _thrusterSound.volume = _thrusterStartCounter / _thrusterStartTime;
    }

    private void RegenerateEnergy()
    {
        if (_startEnergyRecoverDelayCounter >= _startEnergyRecoverDelay && _currentEnergy < maxEnergy)
        {
            _currentEnergy = Mathf.Min(maxEnergy, _currentEnergy + _energyRecoverPerSecond * Time.deltaTime);
        }
    }

    private void CheckStartRecoverEnergy()
    {
        if (!_isFlying || _currentEnergy <= 0)
        {
            _startEnergyRecoverDelayCounter += Time.deltaTime;
        }
        else
        {
            _startEnergyRecoverDelayCounter = 0;
        }
    }

    private void HandleEneryBar()
    {
        if (_currentEnergy < maxEnergy)
        {
            _energyBar.gameObject.SetActive(true);
        }
        else
        {
            _energyBar.gameObject.SetActive(false);
        }
        _energyBar.value = Mathf.Min(1, _currentEnergy / maxEnergy);
    }
}
