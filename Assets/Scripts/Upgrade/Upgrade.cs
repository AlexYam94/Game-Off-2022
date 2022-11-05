using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades", fileName = "UpgradeObject", order = 0)]
public class Upgrade : ScriptableObject
{
    public UpgradeEnum upgradeType;
    public Sprite upgradeIcon;
    public string description;
}
