using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    public InputField playerNameInput;
    public Button playerNameConfirmBtn;

    public Button clearBtn;

    private const int MAX_HIGHSCORES = 10;

    private void Awake()
    {
        SetHighScoresTableElementsInvisible();
     
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

    }
    private void SetHighscoresTableElementsVisible()
    {
        this.playerNameInput.gameObject.SetActive(false);
        this.playerNameConfirmBtn.gameObject.SetActive(false);
        this.highscorePanel.gameObject.SetActive(true);
        this.highscoreTableTitle.gameObject.SetActive(true);
        this.positionTitle.gameObject.SetActive(true);
        this.scoreTitle.gameObject.SetActive(true);
        this.playerNameTitle.gameObject.SetActive(true);
        this.clearBtn.gameObject.SetActive(true);
    }

    private List<HighscoreEntry> CreateHighscoresTable(HighScores highScores, string playerName)
    {
        int coinsNumber = PlayerManager.numberOfCoins;

        List<HighscoreEntry> currentHighscoreList = highScores.highscoreEntriesList != null
            ? highScores.highscoreEntriesList
            : new List<HighscoreEntry>();
       
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

    public void OnConfirmPlayerNameButtonClick()
    {
        string playerName = this.playerNameInput.text;

        SetHighscoresTableElementsVisible();

        HighScores highScores = JsonUtility.FromJson<HighScores>(PlayerPrefs.GetString("highscoresTable"));
        if(highScores == null)
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

        PlayerPrefs.DeleteKey("highscoresTable");
        PlayerPrefs.Save();
    }
}

public class HighScores
{
    public List<HighscoreEntry> highscoreEntriesList;
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
