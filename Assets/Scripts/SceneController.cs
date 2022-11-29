using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] int _gameOverSceneIndex;
    [SerializeField] int _nextSceneIndex = 0;

    public void LoadNextScene()
    {
        StartCoroutine(LoadNexSceneCoroutine());
    }

    public void StartGame()
    {
        LoadNextScene();
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(_gameOverSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene(_nextSceneIndex);
        }
    }

    IEnumerator LoadNexSceneCoroutine()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;


        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentIndex + 1);

        while (!asyncOperation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        //if (SceneManager.GetActiveScene().name.ToLower().CompareTo("gameover") != 0)
        //{
        //    PlayerPrefs.SetInt("lastSceneIndex", currentIndex + 1);
        //    SavingController.GetInstance().Save();
        //}
    }
}
