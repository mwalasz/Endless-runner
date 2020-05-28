using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    public Text gameOverText;
    public Text speedText;
    public Button replayBtn;
    public Button quitBtn;
    public Button highscoreBtn;

    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void HideUIElements()
    {
        this.gameOverText.gameObject.SetActive(false);
        this.speedText.gameObject.SetActive(false);
        this.replayBtn.gameObject.SetActive(false);
        this.quitBtn.gameObject.SetActive(false);
        this.highscoreBtn.gameObject.SetActive(false);
    }

    public void UnhideUIElements()
    {
        this.gameOverText.gameObject.SetActive(true);
        this.speedText.gameObject.SetActive(true);
        this.replayBtn.gameObject.SetActive(true);
        this.quitBtn.gameObject.SetActive(true);
        this.highscoreBtn.gameObject.SetActive(true);
    }

    public void HighscoreTable()
    {
        HideUIElements();    
        SceneManager.LoadScene("HighScores", LoadSceneMode.Additive);
    }
}
