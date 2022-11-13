using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon[] _weapons;
    [SerializeField] SpriteRenderer _armSprite;

    FireController _fireController;

    //Control sub weapon
    //Control is mecha
    //Control main weapon type


    // Start is called before the first frame update
    void Start()
    {
        _fireController = GetComponent<FireController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Mecha")
        {
            //Enable ride mecha button
        }
    }

    internal void EnableMissleAttachment()
    {
        throw new NotImplementedException();
    }

    internal void SwitchToAPBullet()
    {
        throw new NotImplementedException();
    }

    internal void SwitchToExplosiveBullet()
    {
        throw new NotImplementedException();
    }

    internal void SpawnMechaAtPlayerPosition()
    {
        throw new NotImplementedException();
    }

    internal void EnableMechaBackpack()
    {
        throw new NotImplementedException();
    }
}
