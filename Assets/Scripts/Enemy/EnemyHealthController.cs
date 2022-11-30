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
    float _currentHealth;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _currentHealth = _totalHealth;
    }

    private void Update()
    {
        if(_currentHealth <= 0)
        {
            if (_deathEffect != null)
            {
                Instantiate(_deathEffect, transform.position, transform.rotation);
            }
            var bodyParts = WarriorBugDeathEffectObjectPool.instance.GetBodyParts();
            if (bodyParts != null)
            {
                bodyParts.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
                bodyParts.transform.rotation = transform.rotation;
                bodyParts.GetComponent<DestroyOverTime>().Reset();
            }
            if (_deathSound != null)
            {
                GameObject.Instantiate(_deathSound, transform.position, Quaternion.identity).transform.SetParent(null);
            }
            GetComponent<DropitemController>()?.DropItem();
            ScoreController.GetInstance()?.Add(_score);
            onDeath?.Invoke();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        _currentHealth = _totalHealth;
        _hitAnimator.enabled = false;
    }

    public void Damage(float damageAmount)
    {
        StartCoroutine(Hit());
        if ((_currentHealth -= damageAmount) <= 0)
        {
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
