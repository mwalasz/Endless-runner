using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    public Text gameOverText;
    public Text speedText;
    public InputField playerNameInput;
    public Button confirmPlayerNameBtn;
    public Button skipSavingScoreBtn;
    public GameObject gameOverPanel;

    public void Awake()
    {
        HideScoreSaving();
        gameOverPanel.SetActive(false);
    }

    public void ReplayGame()
    {
        Highscore.isScoreAlreadyAdded = false;
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
        this.playerNameInput.gameObject.SetActive(false);
        this.confirmPlayerNameBtn.gameObject.SetActive(false);
        this.skipSavingScoreBtn.gameObject.SetActive(false);
    }

    public void UnhideUIElements()
    {
        this.gameOverText.gameObject.SetActive(true);
        this.speedText.gameObject.SetActive(true);
    }

    public void UnhideScoreSaving()
    {
        playerNameInput.gameObject.SetActive(true);
        confirmPlayerNameBtn.gameObject.SetActive(true);
        skipSavingScoreBtn.gameObject.SetActive(true);
    }

   public void HideScoreSaving()
    {
        playerNameInput.gameObject.SetActive(false);
        confirmPlayerNameBtn.gameObject.SetActive(false);
        skipSavingScoreBtn.gameObject.SetActive(false);
    }

    public void OnConfirmPlayerNameBtnClick()
    {
        PlayerManager.SetPlayerName(this.playerNameInput.text);
        HideScoreSaving();
        SceneManager.LoadScene("HighScores", LoadSceneMode.Additive);
    }

    public void OnSkipBtnClick()
    {
        HideScoreSaving();
        this.gameOverPanel.SetActive(true);
        Highscore.isScoreAlreadyAdded = true;
    }

    public void UnhideGameOverPanel()
    {
        this.gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        this.gameOverPanel.SetActive(false);
    }

    public void HighscoreTable()
    {
        HideUIElements();
        HideGameOverPanel();
        SceneManager.LoadScene("HighScores", LoadSceneMode.Additive);
    }
}
