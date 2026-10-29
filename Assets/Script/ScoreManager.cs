using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text cpuScoreText;

    private int playerScore;
    private int cpuScore;

    public void PlayerScores()
    {
        playerScore++;
        UpdateUI();
    }

    public void CpuScores()
    {
        cpuScore++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        playerScoreText.text = playerScore.ToString();
        cpuScoreText.text = cpuScore.ToString();
    }
}