using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFireLogic
{

    public void Fire(Transform firePoint, Vector2 direction, float damageMultiplier, HomingMissile m)
    {
        //TODO:
        m.transform.position = firePoint.position;
        m.transform.rotation = Quaternion.LookRotation(direction);
        m.flyForward();
    }
}
