using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public int timeOfGame;
    public Text coinsText;
    public Text timeText;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        timeOfGame = 0;

        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    void GetElapsedTime()
    {
        if (isGameStarted)
        {
            timer += Time.deltaTime;
            timeOfGame =  Convert.ToInt32(timer % 60);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetElapsedTime();

        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "Coins: " + numberOfCoins;
        
        timeText.text = "Time: " + timeOfGame;

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
            
    }
}
