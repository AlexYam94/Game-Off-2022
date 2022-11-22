using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHItEffectController : MonoBehaviour
{
    [SerializeField] Animator _virbateAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Vibrate()
    {
        StartCoroutine(VibrateCoroutine());
    }

    private IEnumerator VibrateCoroutine()
    {
        _virbateAnimator.enabled = true;
        yield return new WaitForSeconds(.5f);
        _virbateAnimator.enabled = false;
    }
}
