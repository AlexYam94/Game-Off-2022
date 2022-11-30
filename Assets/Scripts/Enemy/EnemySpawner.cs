using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyToSpawn;
    [SerializeField] float _spawnInterval;
    [SerializeField] float _increaseSpawnInterval;
    [SerializeField] int _increaseSpawnAmount;
    [SerializeField] Collider2D _spawnArea;
    [SerializeField] int _maxAvailable = 50;
    [SerializeField] int _maxSpawnNumber = 20;
    [SerializeField] float _minPlayerDistance = 2f;

    public Action reduceEnemyNumberDelegate;
    public Action addEnemyDelegate;

    ObjectPool<GameObject> _pool;
    GameObject _player;
    float _spawnCounter;
    float _increaseCounter;
    private int _spawnNumber;
    Bounds _bounds;
    List<GameObject> _spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCounter = 0;
        _spawnNumber = 1;
        _increaseCounter = _increaseSpawnInterval;
        _bounds = _spawnArea.bounds;
        _spawnedEnemies = new List<GameObject>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _pool = new ObjectPool<GameObject>(()=>Spawn(), null, null, null, false, 20, 200);
    }

    // Update is called once per frame
    void Update()
    {
        //CleanupDeadEnemies();
        //_spawnCounter -= Time.deltaTime;
        //_increaseCounter -= Time.deltaTime;
        //if (_increaseCounter < 0 && _spawnedEnemies.Count < _maxAvailable)
        //{
        //    _spawnNumber++;
        //    _increaseCounter = _increaseSpawnInterval;
        //}
        //if (_spawnCounter < 0)
        //{
        //    if (_spawnedEnemies.Count < _maxAvailable)
        //    {
        //        Spawn();
        //    }
        //    _spawnCounter = _spawnInterval;
        //}
    }

    private void CleanupDeadEnemies()
    {
        for(int i=0;i<_spawnedEnemies.Count;i++)
        {
            if(_spawnedEnemies[i]==null)
            {
                _spawnedEnemies.RemoveAt(i);
                reduceEnemyNumberDelegate.Invoke();
            }
        }
    }

    private GameObject Spawn()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(0, _enemyToSpawn.Length);
        var g = Instantiate(_enemyToSpawn[index], Vector3.zero, transform.rotation);
        _spawnedEnemies.Add(g);
        g.GetComponent<EnemyHealthController>().onDeath += () => reduceEnemyNumberDelegate.Invoke();
        g.GetComponent<EnemyHealthController>().onDeath += () => _pool.Release(g);
        //addEnemyDelegate.Invoke();
        return g;
    }

    public void Spawn(int numberToSpawn)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            var g = _pool.Get();
            g.GetComponent<EnemyHealthController>().Reset();
            g.SetActive(true);
            System.Random rand = new System.Random();
            Vector3 pos;
            do
            {
                pos = new Vector3(
                 UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
                 UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y),
                 _player.transform.position.z);
            } while (Vector3.Distance(pos, _player.transform.position) < _minPlayerDistance);
            g.transform.position = pos;
            //Spawn();
            //System.Random rand = new System.Random();
            //int index = rand.Next(0, _enemyToSpawn.Length);
            //Vector3 pos;
            //do
            //{
            //    pos = new Vector3(
            //     UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
            //     UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y),
            //     _player.transform.position.z);
            //} while (Vector3.Distance(pos, _player.transform.position) < _minPlayerDistance);
            //var g = Instantiate(_enemyToSpawn[index], pos, transform.rotation);
            ////_spawnedEnemies.Add(g);
            //g.GetComponent<EnemyHealthController>().onDeath += () => reduceEnemyNumberDelegate.Invoke();
            addEnemyDelegate.Invoke();
        }

    }

    public void CleanPool()
    {
        for(int i = 0; i < _spawnedEnemies.Count; i++)
        {
            if (_pool.CountActive > 0)
            {
                _pool.Release(_spawnedEnemies[i]);
            }
        }
    }
}
