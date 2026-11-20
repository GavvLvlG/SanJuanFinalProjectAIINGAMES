using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public interface IScoreManager
{
    void CpuScores();
    void PlayerScores();
    void ResetScores();
}

public class ScoreManager : MonoBehaviour, IScoreManager
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text cpuScoreText;
    [SerializeField] private RunManager runManager;
    [SerializeField] private CPU cpu;

    [SerializeField] private GameObject gameOverScreen;
    
    private bool gameOver;

    private int playerScore;
    private int cpuScore;
    private int pointsSinceUpgrade;

    private void Start()
    {
        UpdateUI();
    }

    public void PlayerScores()
    {
        int points = runManager != null
            ? runManager.GetScoreMultiplier()
            : 1;

        playerScore += points;
        pointsSinceUpgrade += points;
        UpdateUI();

        if (pointsSinceUpgrade >= 5 && runManager != null)
        {
            pointsSinceUpgrade -= 5;
            runManager.PlayerScoredRound();
            cpu.ChooseRandomUpgrade();
            
        }
    }

    public void CpuScores()
    {
        cpuScore++;
        UpdateUI();
       if (cpuScore >= 5)
{
    gameOver = true;
    Time.timeScale = 0f;

    if (gameOverScreen != null)
        gameOverScreen.SetActive(true);
}
    }

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
public void RestartGame()
{
    Time.timeScale = 1f; // Restore time before reloading
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
public void GoToMainMenu()
{
    Time.timeScale = 1f; // Unpause before changing scenes
    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
}
 private void UpdateUI()
    {
        if (playerScoreText != null)
            playerScoreText.text = playerScore.ToString();

        if (cpuScoreText != null)
            cpuScoreText.text = cpuScore.ToString();
    }
}