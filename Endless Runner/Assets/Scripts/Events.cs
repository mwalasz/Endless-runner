using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{

    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void HighscoreTable()
    {
        SceneManager.LoadScene("HighScores");
    }
}
