using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private const string HighScoreContentName = "Element";
    private const string HighScoreRowTemplateName = "HighScoreElement";
    private const string PlayerNameTextName = "Player Name";
    private const string PlayerScoreTextName = "Player Score";
    private const int MaximumDisplayedScores = 4;

    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject highscorePanel;
    [SerializeField] private TMP_Text roundsPlayedText;

    private Transform highScoreContent;
    private GameObject highScoreRowTemplate;
    private readonly List<GameObject> generatedHighScoreRows = new List<GameObject>();

    private void Awake()
    {
        if (highscorePanel == null)
            return;

        highScoreContent = highscorePanel.transform.Find(HighScoreContentName);
        if (highScoreContent == null)
        {
            Debug.LogError($"High-score panel is missing its '{HighScoreContentName}' content object.", this);
            return;
        }

        Transform template = highScoreContent.Find(HighScoreRowTemplateName);
        if (template == null)
        {
            Debug.LogError($"High-score content is missing its '{HighScoreRowTemplateName}' row template.", this);
            return;
        }

        highScoreRowTemplate = template.gameObject;
        highScoreRowTemplate.SetActive(false);
    }

    /// <summary>Loads the configured gameplay scene.</summary>
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>Opens the high-score panel and refreshes its saved entries.</summary>
    public void OpenHighscorePanel()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (highscorePanel != null)
            highscorePanel.SetActive(true);

        if (roundsPlayedText != null)
            roundsPlayedText.text = $"Rounds played: {PlayerPrefs.GetInt("RoundsPlayed", 0)}";

        RefreshHighScoreRows();
    }

    /// <summary>Closes the high-score panel and returns to the main menu.</summary>
    public void CloseHighscorePanel()
    {
        if (highscorePanel != null)
            highscorePanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    /// <summary>Requests application shutdown.</summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    private void RefreshHighScoreRows()
    {
        if (highScoreContent == null || highScoreRowTemplate == null)
            return;

        for (int i = 0; i < generatedHighScoreRows.Count; i++)
        {
            if (generatedHighScoreRows[i] != null)
                Destroy(generatedHighScoreRows[i]);
        }

        generatedHighScoreRows.Clear();

        List<HighScoreTable.Entry> entries = HighScoreTable.LoadEntries();
        if (entries.Count == 0)
        {
            CreateHighScoreRow("No scores yet", "—");
            return;
        }

        int entryCount = Mathf.Min(entries.Count, MaximumDisplayedScores);
        for (int i = 0; i < entryCount; i++)
        {
            HighScoreTable.Entry entry = entries[i];
            string playerName = string.IsNullOrWhiteSpace(entry.playerName) ? "Player" : entry.playerName;
            CreateHighScoreRow($"#{i + 1:00}  {playerName}", entry.score.ToString("N0"));
        }
    }

    private void CreateHighScoreRow(string playerName, string score)
    {
        GameObject row = Instantiate(highScoreRowTemplate, highScoreContent);
        row.name = $"HighScoreRow_{generatedHighScoreRows.Count + 1}";

        TMP_Text playerNameText = row.transform.Find(PlayerNameTextName)?.GetComponent<TMP_Text>();
        TMP_Text playerScoreText = row.transform.Find(PlayerScoreTextName)?.GetComponent<TMP_Text>();

        if (playerNameText == null || playerScoreText == null)
        {
            Debug.LogError("High-score row needs 'Player Name' and 'Player Score' TMP text children.", row);
            Destroy(row);
            return;
        }

        playerNameText.text = playerName;
        playerScoreText.text = score;
        generatedHighScoreRows.Add(row);
        row.SetActive(true);
    }
}
