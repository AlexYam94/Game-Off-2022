using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlignSlider : MonoBehaviour
{
    Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        float vol;
        if ((vol = PlayerPrefs.GetFloat(SetVolume.VOLUME, -1f)) >= 0){
            _slider.value = vol;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
