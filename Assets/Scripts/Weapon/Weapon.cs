using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "WeaponObject", order = 0)]
public class Weapon : ScriptableObject
{
    public weaponType type;
    public Bullet bullet;
    public Sprite weaponSprite;
    public AnimatorOverrideController weaponAnimatorController;
    public AudioClip gunshot;
    public bool autoMode = false;
    public int capacity = 10;
    public float reloadTime = 2;
    public float fireRate = .1f;
    public float spread = 1;
    public bool isFlame = true;
    public float cameraShakeMagnitude = 1f;

    public enum weaponType{
        pistol,
        shotgun,
        smg,
        crossbow,
        missile
    }

    public Action<Transform, Vector2, float> Fire()
    {
        switch (type)
        {
            case weaponType.pistol:
                return PistolFire;
            case weaponType.shotgun:
                return ShotgunFire;
            case weaponType.smg:
                return SmgFire;
            case weaponType.crossbow:
                return CrossbpwFire;
        }
        return PistolFire;
    }

    private void PistolFire(Transform firePoint, Vector2 direction, float damageMultiplier)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = firePoint.position;
        float addedOffset;
        do
        {
            addedOffset = (float)new System.Random().NextDouble() * spread;
        } while (addedOffset > spread);
        b.SetDirection(new Vector2(direction.x + addedOffset, direction.y + addedOffset));
        b.SetDirection(direction);
        b.damageMultiplier = damageMultiplier;
    }
    private void ShotgunFire(Transform firePoint, Vector2 direction, float damageMultiplier)
    {
        int bulletCount = 10;
        Quaternion newRot = firePoint.rotation;

        Bullet[] bullets = new Bullet[bulletCount];

        for (int i = 0; i < bulletCount; i++)
        {
            float addedOffset;

            do
            {
                addedOffset = (float)new System.Random().NextDouble() * spread;
            } while (addedOffset > spread);


            Bullet b = Instantiate(bullet, firePoint.position, newRot);
            //b.transform.position = origin;

            b.canFly = false;
            b.damageMultiplier = damageMultiplier;
            bullets[i] = b;
            if (i % 2 == 0)
            {
                b.SetDirection(new Vector2(direction.x + addedOffset, direction.y + addedOffset));
            }
            else
            {
                b.SetDirection(new Vector2(direction.x - addedOffset, direction.y - addedOffset));
            }
        }
        foreach(Bullet b in bullets)
        {
            b.canFly = true;
        }
    }
    private void SmgFire(Transform firePoint, Vector2 direction, float damageMultiplier)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = firePoint.position;
        var pDir = Vector2.Perpendicular(direction) * UnityEngine.Random.Range(-spread, spread);
        b.SetDirection(direction + pDir);
        b.damageMultiplier = damageMultiplier;
    }
    private void CrossbpwFire(Transform firePoint, Vector2 direction, float damageMultiplier)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = firePoint.position;
        float addedOffset;
        do
        {
            addedOffset = (float)new System.Random().NextDouble() * spread;
        } while (addedOffset > spread);
        b.SetDirection(new Vector2(direction.x + addedOffset, direction.y + addedOffset));
        b.SetDirection(direction);
        b.damageMultiplier = damageMultiplier;
    }
}
