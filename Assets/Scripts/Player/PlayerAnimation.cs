using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] Animator _ballAnimator;
    [SerializeField] RuntimeAnimatorController _shotUpOverride;
    [SerializeField] Animator _armAnimator;

    //public RuntimeAnimatorController _armOverride;

    private RuntimeAnimatorController _originAnimController;
    bool _isBall;

    Animator _currentAnimator;

    private void Start()
    {
        _currentAnimator = _playerAnimator;
        _originAnimController = _currentAnimator.runtimeAnimatorController;
    }

    public void Move(float move)
    {
        _currentAnimator.SetFloat("run", Mathf.Abs(move));
    }
    public void Jump(bool jump)
    {
        //if (_isBall) return;
        _currentAnimator.SetBool("jump", jump);
    }

    public void SetCoyote(bool isCoyote)
    {
        _currentAnimator.SetBool("coyote", isCoyote);
    }

    public void DoubleJump()
    {
        //if (_isBall) return;
        _currentAnimator.SetTrigger("doubleJump");
    }

    public void Shoot()
    {
        //if (_isBall) return;
        //_currentAnimator.SetTrigger("shoot");
        _armAnimator.SetTrigger("shoot");
    }

    public void Fly()
    {
        _playerAnimator.SetBool("fly", true);
    }

    public void NotFly()
    {
        _playerAnimator.SetBool("fly", false);
        _playerAnimator.SetBool("fly forward", false);
        _playerAnimator.SetBool("fly backward", false);
        _playerAnimator.SetBool("fly upward", false);
    }

    public void Hover()
    {
        _playerAnimator.SetBool("fly", true);
        _playerAnimator.SetBool("fly forward", false);
        _playerAnimator.SetBool("fly backward", false);
        _playerAnimator.SetBool("fly upward", false);
    }

    public void FlyBackward()
    {
        _playerAnimator.SetBool("fly forward", false);
        _playerAnimator.SetBool("fly backward", true);
        _playerAnimator.SetBool("fly upward", false);
    }

    public void FlyForward()
    {
        _playerAnimator.SetBool("fly forward", true);
        _playerAnimator.SetBool("fly backward", false);
        _playerAnimator.SetBool("fly upward", false);
    }

    public void FlyUpward()
    {
        _playerAnimator.SetBool("fly upward", true);
        _playerAnimator.SetBool("fly forward", false);
        _playerAnimator.SetBool("fly backward", false);
    }
    public void FlyDownward()
    {
        _playerAnimator.SetBool("fly upward", false);
        _playerAnimator.SetBool("fly forward", true);
        _playerAnimator.SetBool("fly backward", false);
    }

    public void ShootUp()
    {
        //if (_isBall) return;
        //_currentAnimator.SetTrigger("shootUp");
        _currentAnimator.SetTrigger("shoot");
    }

    public void OverrideAnimator()
    {
        _currentAnimator.runtimeAnimatorController = _shotUpOverride;

    }

    public void OverrideArmController(RuntimeAnimatorController controller)
    {
        _armAnimator.runtimeAnimatorController = controller;
    }

    public void RestoreAnimator()
    {
        _currentAnimator.runtimeAnimatorController = _originAnimController;

    }

    public void ActivateBall()
    {
        _isBall = true;
        _currentAnimator = _ballAnimator;
    }

    public void DeativateBall()
    {
        _isBall = false;
        _currentAnimator = _playerAnimator;
    }

    public void DisableAnimator()
    {
        _playerAnimator.enabled = false;
    }

    public void EnableAnimator()
    {
        _playerAnimator.enabled = true;
    }

}
