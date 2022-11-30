using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WarriorBugDeathEffectObjectPool : MonoBehaviour
{
    public static WarriorBugDeathEffectObjectPool instance;

    [SerializeField] GameObject[] _bodyParts;
    ObjectPool<GameObject> _pool;

    List<GameObject> _cache;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance!= this)
        {
            Destroy(this);
        }
        _pool = new ObjectPool<GameObject>(() => CreateDeathEffect(), null, null, null, false, 10, 200);
        _cache = new List<GameObject>();
    }

    public GameObject GetBodyParts()
    {
        var g = _pool.Get();
        g.SetActive(true);
        return g;
    }

    public GameObject CreateDeathEffect()
    {
        int index = Random.Range(0, _bodyParts.Length);
        var g = Instantiate(_bodyParts[index], Vector3.zero, Quaternion.identity);
        _cache.Add(g);
        return g;
    }

    public void CleanPool()
    {
        foreach(var g in _cache)
        {
            if(_pool.CountActive>0)
                _pool.Release(g);
        }
    }
}
