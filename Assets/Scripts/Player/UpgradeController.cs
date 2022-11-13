using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] UpgradeEnum[] _uniqueUpgrades;
    [SerializeField] UpgradeEnum[] _mechaUpgrades; 

    Dictionary<UpgradeEnum, int> _existedUpgrades = new Dictionary<UpgradeEnum, int>();

    public float damageMultiplier = 1f;
    public float speedMultiploer = 1f;
    public float ammoCapacityMultiplier = 1f;
    public float reloadSpeedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float energyConsumptionMultiplier = 1f;
    PlayerController _playerController;
    WeaponController _weaponController;
    PlayerHealthController _playerHealthController;

    // Start is called before the first frame update
    void Start() 
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanUpgrade(UpgradeEnum type)
    {
        foreach(var e in _mechaUpgrades)
        {
            if(e == type)
            {
                if (!_existedUpgrades.ContainsKey(UpgradeEnum.mecha))
                {
                    return false;
                }
            }
        }
        //foreach(var e in _uniqueUpgrades)
        //{
        //    if(e == type)
        //    {
        //        if (_existedUpgrades.ContainsKey(type))
        //        {
        //            return false;
        //        }
        //    }
        //}
        if (_existedUpgrades.ContainsKey(type))
        {
            return false;
        }
        return true;
    }

    public void Upgrade(UpgradeEnum type)
    {
        switch (type)
        {
            case UpgradeEnum.attack1:
            case UpgradeEnum.attack2:
            case UpgradeEnum.attack3:
                damageMultiplier *= 1.2f;
                break;
            case UpgradeEnum.ammo1:
            case UpgradeEnum.ammo2:
            case UpgradeEnum.ammo3:
                ammoCapacityMultiplier *= 1.2f;
                break;
            case UpgradeEnum.reload1:
            case UpgradeEnum.reload2:
            case UpgradeEnum.reload3:
                reloadSpeedMultiplier *= 1.2f;
                break;
            case UpgradeEnum.fire_rate1:
            case UpgradeEnum.fire_rate2:
            case UpgradeEnum.fire_rate3:
                fireRateMultiplier *= 1.2f;
                break;
            case UpgradeEnum.energy_consumption1:
            case UpgradeEnum.energy_consumption2:
            case UpgradeEnum.energy_consumption3:
                energyConsumptionMultiplier *= 1.2f;
                break;
            case UpgradeEnum.mecha:
                _weaponController.SpawnMechaAtPlayerPosition();
                break;
            case UpgradeEnum.backpack:
                _weaponController.EnableMechaBackpack();
                break;
            case UpgradeEnum.battery:
                _playerController.batteryCapacity *= 1.5f;
                break;
            case UpgradeEnum.missile_attachment:
                _weaponController.EnableMissleAttachment();
                break;
            case UpgradeEnum.speed1:
            case UpgradeEnum.speed2:
            case UpgradeEnum.speed3:
                speedMultiploer *= 1.2f;
                break;
            case UpgradeEnum.health1:
            case UpgradeEnum.health2:
            case UpgradeEnum.health3:
                _playerHealthController.SetHealthScale(1.2f);
                break;
            case UpgradeEnum.ap_bullet:
                _weaponController.SwitchToAPBullet();
                break;
            case UpgradeEnum.explosive_bullet:
                _weaponController.SwitchToExplosiveBullet();
                break;
            default:
                break;
        }
        _existedUpgrades.Add(type, 1);
    }
}
