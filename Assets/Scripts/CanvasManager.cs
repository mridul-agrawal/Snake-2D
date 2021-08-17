using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public CanvasRenderer GameOverPanel;

    public void ResetScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void EnableDeathUI()
    {
        GameOverPanel.gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }

}
