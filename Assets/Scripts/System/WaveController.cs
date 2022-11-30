using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WaveController : MonoBehaviour
{
    [SerializeField] int _maxWaves = 20;
    [SerializeField] float _timeBetweenWaves = 20;
    [SerializeField] int _maxEnemyPerWave = 100;
    [SerializeField] int _enemyNumberWaveMultiplier = 1;
    [SerializeField] TextMeshProUGUI _waveText;
    [SerializeField] GameObject _intervalTextGameObject;
    [SerializeField] TextMeshProUGUI _intervalCounterText;
    [SerializeField] EnemySpawner[] _enemySpawners;

    private GameLoopController _gameLoopController;
    private int _currentWave = 0;
    private int _currentEnemy = 0;
    private bool _isInterval = true;
    private bool _waveStarted = false;

    private float _intervalCounter = 0;

    private object _mutex = new System.Object();
    // Start is called before the first frame update
    void Start()
    {
        foreach(var e in _enemySpawners)
        {
            e.reduceEnemyNumberDelegate = ReduceEnemyNumber();
            e.addEnemyDelegate = AddEnemy();
        }
        _gameLoopController = GetComponent<GameLoopController>(); 
        _isInterval = true;
        _waveStarted = false;
        _currentWave = 0;
        _currentEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _waveText.text = _currentWave + "/" + _maxWaves;
        if (_currentWave > _maxWaves)
        {
            _gameLoopController.EnableNextLevelEntrace();
            return;
        }
        if (!_isInterval && _currentEnemy <= 0 && _waveStarted) //Wave cleared, enter interval
        {
            //Notify gameloop controller to enter interval
            if (_currentWave <= 0)
            {
                _gameLoopController.EnterInterval(true);
            }
            else
            {
                _gameLoopController.EnterInterval(false);
            }
            _intervalTextGameObject.SetActive(true);
            _isInterval = true;
            _intervalCounter = _timeBetweenWaves;
            _currentWave++;
            ResetWaveStatus();
        }
        if (_isInterval && !_waveStarted && _intervalCounter <= 0)
        {
            _isInterval = false;
            StartWave();
        }
        else if(_intervalCounter >= 0)
        {
            _intervalCounter -= Time.deltaTime;
            _intervalCounterText.text = String.Format("Enemies arrive at {0:0.00} seconds", _intervalCounter); 
        }
        
    }

    private void ResetWaveStatus()
    {
        _currentEnemy = 0;
        _waveStarted = false;
        foreach (var spawner in _enemySpawners)
        {
            spawner.CleanPool();
        }
        WarriorBugDeathEffectObjectPool.instance.CleanPool();
    }

    public void StartWave()
    {
        foreach (var spawner in _enemySpawners)
        {
            //TODO: calculate how ma ny enemy to spawn
            if (_currentEnemy <= _maxEnemyPerWave)
            {
                spawner.Spawn(_currentWave * _enemyNumberWaveMultiplier);
            }
        }
        _intervalTextGameObject.SetActive(false);
        _waveStarted = true;
    }

    public Action ReduceEnemyNumber()
    {
        return () =>
        {
            lock (_mutex)
            {
                _currentEnemy = Mathf.Max(_currentEnemy-1, 0);
            }
        };
    }

    public Action AddEnemy()
    {
        return () =>
        {
            lock (_mutex)
            {
                _currentEnemy++;
            }
        };
    }
}
