using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
    public void GoToMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GoToGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    
    public void ContinuePlaying()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(3);
    }

    public void OpenHelp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void CloseHelp()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
