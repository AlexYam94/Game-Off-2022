using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] UpgradeEnum[] _uniqueUpgrades;
    [SerializeField] UpgradeEnum[] _mechaUpgrades; 

    Dictionary<UpgradeEnum, int> _existedUpgrades = new Dictionary<UpgradeEnum, int>();

    // Start is called before the first frame update
    void Start() 
    {
        
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
        foreach(var e in _uniqueUpgrades)
        {
            if(e == type)
            {
                if (_existedUpgrades.ContainsKey(type))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Upgrade(UpgradeEnum type)
    {
        switch (type)
        {
            case UpgradeEnum.attack:
                break;
            case UpgradeEnum.ammo:
                break;
            case UpgradeEnum.reload:
                break;
            case UpgradeEnum.fire_rate:
                break;
            case UpgradeEnum.energy_consumption:
                break;
            case UpgradeEnum.mecha:
                break;
            case UpgradeEnum.backpack:
                break;
            case UpgradeEnum.battery:
                break;
            case UpgradeEnum.missile_attachment:
                break;
            case UpgradeEnum.speed:
                break;
            case UpgradeEnum.health:
                break;
            case UpgradeEnum.ap_bullet:
                break;
            case UpgradeEnum.explosive_bullet:
                break;
        }
    }
}
