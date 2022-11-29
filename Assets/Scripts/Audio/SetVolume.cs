using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public static string VOLUME = "volume";
    // Start is called before the first frame update
    void Start()
    {
        float vol;
        if((vol = PlayerPrefs.GetFloat(VOLUME, -1f)) > 0)
        {
            AudioListener.volume = vol;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVol(float val)
    {
        AudioListener.volume = val;
        PlayerPrefs.SetFloat(VOLUME, val);
        PlayerPrefs.Save();
    }
}
