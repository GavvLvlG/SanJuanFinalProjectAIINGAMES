using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text cpuScoreText;

    private int playerScore;
    private int cpuScore;

    private void Start()
    {
        UpdateUI();
    }

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
        if (playerScoreText != null)
            playerScoreText.text = playerScore.ToString();

        if (cpuScoreText != null)
            cpuScoreText.text = cpuScore.ToString();
    }
}