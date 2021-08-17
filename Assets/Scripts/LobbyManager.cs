using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public void SinglePlayerButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CoOpButton()
    {
        SceneManager.LoadScene(2);
    }


    public void QuitButton()
    {
        Application.Quit();
    }

}
