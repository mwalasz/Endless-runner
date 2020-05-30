using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Transform highscorePanel;
    public Transform highscoreEntriesContainer;
    public Transform highscoreEntryTemplate;
    private List<Transform> highScoreEntriesTransforms;
    public Transform highscoreTableTitle;
    public Transform positionTitle;
    public Transform scoreTitle;
    public Transform playerNameTitle;

    public Button clearBtn;
    public Button backBtn;

    public static bool isScoreAlreadyAdded = false;

    private static HighScores highScores;

    private const int MAX_HIGHSCORES = 10;

    private void Awake()
    {
        if(isScoreAlreadyAdded == false)
        {
            SetHighScoresTableElementsInvisible();

            GenerateHighscoresTable(PlayerManager.getPlayerName());
            isScoreAlreadyAdded = true;
        }
        else
        {
            this.highscoreEntryTemplate.gameObject.SetActive(false);
            GenerateHighscoresTable("");
        }         
    }

    private void SetHighScoresTableElementsInvisible()
    {
        this.highscorePanel.gameObject.SetActive(false);
        this.highscoreTableTitle.gameObject.SetActive(false);
        this.positionTitle.gameObject.SetActive(false);
        this.scoreTitle.gameObject.SetActive(false);
        this.playerNameTitle.gameObject.SetActive(false);
        this.highscoreEntryTemplate.gameObject.SetActive(false);
        this.clearBtn.gameObject.SetActive(false);
        this.backBtn.gameObject.SetActive(false);

    }
    private void SetHighscoresTableElementsVisible()
    {
        this.highscorePanel.gameObject.SetActive(true);
        this.highscoreTableTitle.gameObject.SetActive(true);
        this.positionTitle.gameObject.SetActive(true);
        this.scoreTitle.gameObject.SetActive(true);
        this.playerNameTitle.gameObject.SetActive(true);
        this.clearBtn.gameObject.SetActive(true);
        this.backBtn.gameObject.SetActive(true);
    }

    private List<HighscoreEntry> CreateHighscoresTable(HighScores highScores, string playerName)
    {
        int coinsNumber = PlayerManager.numberOfCoins;

        List<HighscoreEntry> currentHighscoreList = highScores.highscoreEntriesList != null
            ? highScores.highscoreEntriesList
            : new List<HighscoreEntry>();

        if (playerName != "")
        {
            if (currentHighscoreList.Count > 0)
            {
                int currentMinimumScore = currentHighscoreList.Min(highscoreEntry => highscoreEntry.playerScore);
                if (coinsNumber < currentMinimumScore && currentHighscoreList.Count == MAX_HIGHSCORES)
                {
                    return currentHighscoreList;
                }

                int currentMinimumScoreEntryIndex = currentHighscoreList.FindIndex(highscoreEntry => highscoreEntry.playerScore == currentMinimumScore);

                if (currentHighscoreList.Count == MAX_HIGHSCORES)
                {
                    currentHighscoreList.RemoveAt(currentMinimumScoreEntryIndex);
                }
            }

            currentHighscoreList.Add(new HighscoreEntry(playerName, coinsNumber));
        }
        currentHighscoreList = currentHighscoreList.OrderByDescending(highscoreEntry => highscoreEntry.playerScore).ToList();

        return currentHighscoreList;
    }

    private void DisplayHighscoresTable(List<HighscoreEntry> highscoreEntriesList)
    {
        this.highScoreEntriesTransforms = new List<Transform>();
        float highscoreEntryHeight = 30.0f;
        int counter = 0;
        foreach (var highscore in highscoreEntriesList)
        {
            Transform highscoreEntry = Instantiate(this.highscoreEntryTemplate, this.highscoreEntriesContainer);
            RectTransform highscoreEntryRectTransform = highscoreEntry.GetComponent<RectTransform>();
            highscoreEntryRectTransform.anchoredPosition = new Vector2(0, -highscoreEntryHeight * counter);
            highscoreEntry.gameObject.SetActive(true);

            int rankingPosition = counter + 1;
            string rankingPositionText = "";
            switch (rankingPosition)
            {
                default:
                    rankingPositionText = rankingPosition + "th";
                    break;
                case 1:
                    rankingPositionText = rankingPosition + "st";
                    break;
                case 2:
                    rankingPositionText = rankingPosition + "nd";
                    break;
                case 3:
                    rankingPositionText = rankingPosition + "rd";
                    break;
            }

            highscoreEntry.Find("Position").GetComponent<Text>().text = rankingPositionText;
            highscoreEntry.Find("Score").GetComponent<Text>().text = highscore.playerScore.ToString();
            highscoreEntry.Find("Name").GetComponent<Text>().text = highscore.playerName;

            this.highScoreEntriesTransforms.Add(highscoreEntry);

            ++counter;
        }
    }

    private void GenerateHighscoresTable(string playerName)
    {
        SetHighscoresTableElementsVisible();

        highScores = JsonUtility.FromJson<HighScores>(PlayerPrefs.GetString("highscoresTable"));
        if (highScores == null)
        {
            highScores = new HighScores();
        }
        highScores.highscoreEntriesList = CreateHighscoresTable(highScores, playerName);
        DisplayHighscoresTable(highScores.highscoreEntriesList);

        PlayerPrefs.SetString("highscoresTable", JsonUtility.ToJson(highScores));
        PlayerPrefs.Save();
    }

    public void OnClearButtonClick()
    {
        foreach (var highscoreEntry in this.highScoreEntriesTransforms)
        {
            highscoreEntry.gameObject.SetActive(false);
        }

        highScores = new HighScores();
        PlayerPrefs.DeleteKey("highscoresTable");
        PlayerPrefs.Save();
    }

    public void OnBackButtonClick()
    {
        Events eventsObject = FindObjectOfType<Events>();
        eventsObject.UnhideGameOverPanel();
        SceneManager.UnloadSceneAsync("HighScores");
    }
}

public class HighScores
{
    public List<HighscoreEntry> highscoreEntriesList;

    public HighScores()
    {
        this.highscoreEntriesList = new List<HighscoreEntry>();
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public string playerName;
   public int playerScore;

    public HighscoreEntry(string playerName, int playerScore)
    {
        this.playerName = playerName;
        this.playerScore = playerScore;
    }
}
