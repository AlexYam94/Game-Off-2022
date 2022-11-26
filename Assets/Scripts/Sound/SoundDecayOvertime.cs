using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDecayOvertime : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    [SerializeField] bool _shouldDestroy = false;
    [SerializeField] bool _shouldDisable = false;
    [SerializeField] float _maxVolume = .5f;

    private AudioSource _as;
    private float _counter;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _counter = _lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _counter = Mathf.Max(0, _counter - Time.deltaTime);
        _as.volume = (_counter / _lifeTime) - (1- _maxVolume);
        if(_counter <= 0 && _shouldDisable)
        {
            gameObject.SetActive(false);
        }
        if (_counter <= 0 && _shouldDestroy)
        {
            Destroy(gameObject);
        }
    }
}
