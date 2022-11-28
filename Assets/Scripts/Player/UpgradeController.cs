using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] UpgradeEnum[] _uniqueUpgrades;
    [SerializeField] UpgradeEnum[] _mechaUpgrades;
    [SerializeField] MissleSystem _missleSystemLeft;
    [SerializeField] MissleSystem _missleSystemRight;

    Dictionary<UpgradeEnum, int> _existedUpgrades = new Dictionary<UpgradeEnum, int>();

    public float damageMultiplier = 1f;
    public float ammoCapacityMultiplier = 1f;
    public float reloadSpeedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float energyConsumptionMultiplier = 1f;
    PlayerController _playerController;
    WeaponController _weaponController;
    PlayerHealthController _playerHealthController;
    EquipmentController _equipmentController;
    FireController _fireController;
    ThrusterController _thrustController;

    // Start is called before the first frame update
    void Start() 
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealthController = GetComponent<PlayerHealthController>();
        _equipmentController = GetComponent<EquipmentController>();
        _fireController = GetComponent<FireController>();
        _thrustController = GetComponent<ThrusterController>();
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
                ammoCapacityMultiplier = 1.2f;
                _missleSystemLeft.maxAmmo = Mathf.CeilToInt(_missleSystemLeft.maxAmmo * ammoCapacityMultiplier);
                _fireController.capacityMultiplier *= ammoCapacityMultiplier;
                break;
            case UpgradeEnum.reload1:
            case UpgradeEnum.reload2:
            case UpgradeEnum.reload3:
                reloadSpeedMultiplier = .8f;
                _fireController.reloadTimeMultiplier *= reloadSpeedMultiplier;
                _missleSystemLeft.reloadTimeMultiplier *= reloadSpeedMultiplier;
                break;
            case UpgradeEnum.fire_rate1:
            case UpgradeEnum.fire_rate2:
            case UpgradeEnum.fire_rate3:
                fireRateMultiplier = 0.8f;
                _fireController.fireRateMultiplier *= fireRateMultiplier;
                _missleSystemLeft.fireRateMultiplier *= fireRateMultiplier;
                break;
            case UpgradeEnum.energy_consumption1:
            case UpgradeEnum.energy_consumption2:
            case UpgradeEnum.energy_consumption3:
                energyConsumptionMultiplier = 0.8f;
                _thrustController._flyEnergyPerSecond *= energyConsumptionMultiplier;
                break;
            case UpgradeEnum.mecha:
                _weaponController.SpawnMechaAtPlayerPosition();
                break;
            case UpgradeEnum.backpack:
                _equipmentController.isJetBackpackEnabled = true;
                break;
            case UpgradeEnum.battery:
                _equipmentController.isExternalBatteryEnabled = true;
                break;
            case UpgradeEnum.missile_attachment_left:
                _equipmentController.isLeftMissleEnabled = true;
                break;
            case UpgradeEnum.missile_attachment_right:
                _equipmentController.isRightMissleEnabled = true;
                break;
            case UpgradeEnum.speed1:
            case UpgradeEnum.speed2:
            case UpgradeEnum.speed3:
                _playerController.thrustSpeedMutiplier *= 1.2f;
                break;
            case UpgradeEnum.health1:
            case UpgradeEnum.health2:
            case UpgradeEnum.health3:
                _playerHealthController.RestoreHealth(.4f);
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
