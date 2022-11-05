using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{

    ChooseUpgrade _chooseUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        _chooseUpgrade = GetComponent<ChooseUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterInterval()
    {
        _chooseUpgrade.ShowUpgrades(3);
    }

    public void EnableNextLevelEntrace()
    {

    }
}
