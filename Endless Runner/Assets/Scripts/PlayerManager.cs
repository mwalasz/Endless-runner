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
    public float timer;

    public Text coinsText;
    public Text timeText;
    public Text speedText;
    
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        timeOfGame = 0;

        speed = 0;

        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        UpdateSpeed();
        
        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "Coins: " + numberOfCoins;
        timeText.text = "Time: " + FormatTimeText();
        speedText.text = "Speed: " + FormatSpeedText();

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
            
    }
    
    void UpdateTime()
    {
        if (isGameStarted)
        {
            timer += Time.deltaTime;
            timeOfGame =  Convert.ToInt32(timer);
        }
    }

    void UpdateSpeed()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController controller = player.GetComponent<PlayerController>();
        speed = Convert.ToInt32(controller.displayedSpeed);
    }

    string FormatSpeedText()
    {
        Color orange = new Color(1.0f, 0.64f, 0.0f);

        switch (speed)
        {
            case int s when (s < 220 && s >= 150):
                speedText.color = orange;
                speedText.fontSize = 55;
                break;

            case int s when (s <= 300 && s >= 220):
                speedText.color = Color.red;
                speedText.fontSize = 65;
                break;

            default:
                break;
        }

        return string.Format("{0} km/h", speed);
    }

    string FormatTimeText()
    {
        return (timeOfGame.ToString()).PadLeft(3, ' ') + "s";
    }
}
