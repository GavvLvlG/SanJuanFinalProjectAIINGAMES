using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text cpuScoreText;
    [SerializeField] private RunManager runManager;

    private int playerScore;
    private int cpuScore;

    [SerializeField] private CPU cpu;
    private int nextUpgradeScore = 5;

    private void Start()
    {
        UpdateUI();
    }

    public void PlayerScores()
    {
        int multiplier = runManager != null
            ? runManager.GetScoreMultiplier()
            : 1;

        playerScore += multiplier;
        UpdateUI();

        if (playerScore >= nextUpgradeScore)
{
    nextUpgradeScore += 5;

    if (cpu != null)
        cpu.ChooseRandomUpgrade();

    if (runManager != null)
        runManager.PlayerScoredRound();
}
    }

    public void CpuScores()
    {
        cpuScore++;
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