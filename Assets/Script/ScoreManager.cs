using TMPro;
using UnityEngine;

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
        }
    }

    public void CpuScores()
    {
        cpuScore++;
        UpdateUI();
    }

    public void ResetScores()
    {
        playerScore = 0;
        cpuScore = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerScoreText != null)
            playerScoreText.text = playerScore.ToString();

        if (cpuScoreText != null)
            cpuScoreText.text = cpuScore.ToString();
    }
}