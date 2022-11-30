using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] bool _DeactiveOverTime = false;

    float _deactCounter;
    // Start is called before the first frame update
    void Start()
    {
        if (!_DeactiveOverTime)
        {
            Destroy(gameObject, _lifeTime);
        }
        else
        {
            _deactCounter = _lifeTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_DeactiveOverTime)
        {
            _deactCounter -= Time.deltaTime;
            if(_deactCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
