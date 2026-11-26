using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IScoreManager
{
    /// <summary>Adds a point to the CPU's current-round score.</summary>
    void CpuScores();

    /// <summary>Adds the current score-multiplier value to the player's run total.</summary>
    void PlayerScores();

    /// <summary>Resets the current round without clearing the player's run total.</summary>
    void ResetScores();
}

public class ScoreManager : MonoBehaviour, IScoreManager
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text cpuScoreText;
    [SerializeField] private RunManager runManager;
    [SerializeField] private CPU cpu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private string highScorePlayerName = "Player";

    private bool gameOver;
    private int playerScore;
    private int cpuScore;
    private int totalPlayerScore;
    private int pointsSinceUpgrade;

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>Records a player point and tracks its contribution to the full run.</summary>
    public void PlayerScores()
    {
        if (gameOver)
            return;

        int points = runManager != null ? runManager.GetScoreMultiplier() : 1;
        playerScore += points;
        totalPlayerScore += points;
        pointsSinceUpgrade += points;
        UpdateUI();

        if (pointsSinceUpgrade >= 5 && runManager != null)
        {
            pointsSinceUpgrade -= 5;
            runManager.PlayerScoredRound();
        }
    }

    /// <summary>Records a CPU point and ends the run when the CPU reaches five.</summary>
    public void CpuScores()
    {
        if (gameOver)
            return;

        cpuScore++;
        UpdateUI();

        if (cpuScore >= 5)
            EndGame();
    }

    /// <summary>Resets per-round scores while preserving the accumulated run score.</summary>
    public void ResetScores()
    {
        playerScore = 0;
        cpuScore = 0;
        pointsSinceUpgrade = 0;
        gameOver = false;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        UpdateUI();
    }

    /// <summary>Restores time and reloads the active gameplay scene.</summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Restores time and returns to the main menu.</summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void EndGame()
    {
        if (gameOver)
            return;

        gameOver = true;
        HighScoreTable.AddScore(highScorePlayerName, totalPlayerScore);
        GameAudio.Instance?.StopBackgroundMusic();
        GameAudio.Instance?.PlayGameOverSound();
        Time.timeScale = 0f;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
    }

    private void UpdateUI()
    {
        if (playerScoreText != null)
            playerScoreText.text = playerScore.ToString();

        if (cpuScoreText != null)
            cpuScoreText.text = cpuScore.ToString();
    }
}
