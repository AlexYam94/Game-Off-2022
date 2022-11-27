using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] float _totalHealth = 6;
    [SerializeField] GameObject _deathEffect;
    [SerializeField] int _score = 100;
    [SerializeField] Animator _hitAnimator;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] GameObject _deathSound;

    public Action onDeath;
    private AudioSource _as;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    public void Damage(float damageAmount)
    {
        StartCoroutine(Hit());
        if ((_totalHealth -= damageAmount) <= 0)
        {
            if (_deathEffect != null)
            {
                Instantiate(_deathEffect, transform.position, transform.rotation);
            }
            if (_deathSound != null)
            {
                GameObject.Instantiate(_deathSound,transform.position, Quaternion.identity).transform.SetParent(null);
            }
            GetComponent<DropitemController>()?.DropItem();
            ScoreController.GetInstance()?.Add(_score);
            onDeath?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            _as.PlayOneShot(_hitSound);
        }
    }

    IEnumerator Hit()
    {
        if (_hitAnimator != null)
        {
            _hitAnimator.enabled = true;
            yield return new WaitForSeconds(_hitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            _hitAnimator.enabled = false;
        }
    }
}
