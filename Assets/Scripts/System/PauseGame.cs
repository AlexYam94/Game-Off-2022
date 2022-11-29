using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject _pauseMenu;
    private bool _isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                Resume();
            }
            else
            {
                Pause();

            }
        }
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _isPause = !_isPause;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        _isPause = !_isPause;
        Time.timeScale = 0;
    }
}
