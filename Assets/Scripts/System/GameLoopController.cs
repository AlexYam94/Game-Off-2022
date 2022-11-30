using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopController : MonoBehaviour
{
    [SerializeField] int nextLevelIndex;

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

    public void EnterInterval(bool firstWave)
    {
        if (firstWave) return;
        _chooseUpgrade.ShowUpgrades(3);
    }

    public void EnableNextLevelEntrace()
    {
        //TODO: Fade in
        //Load next scene
        SceneManager.LoadScene(4);
    }
}
