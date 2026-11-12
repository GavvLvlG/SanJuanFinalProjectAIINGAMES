using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Ball ball;
    [SerializeField] private Transform playerPaddle;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] upgradeButtonLabels;

    [Header("Run settings")]
    [SerializeField] private float startingBallSpeed = 5f;
    [SerializeField] private float speedIncreasePerRound = 0.5f;
    [SerializeField] private float paddleWidthIncrease = 0.5f;

    private int round = 1;
    private float speedBonus;
    private float scoreMultiplier = 1f;
    private bool choosingUpgrade;

    private readonly string[] upgradeNames =
    {
        "Wider paddle",
        "Faster ball",
        "Score multiplier"
    };

    private void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    public void PlayerScoredRound()
    {
        if (choosingUpgrade) return;

        choosingUpgrade = true;
        Time.timeScale = 0f;

        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int upgradeIndex = i;
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(
                () => ChooseUpgrade(upgradeIndex));

            if (i < upgradeButtonLabels.Length)
                upgradeButtonLabels[i].text = upgradeNames[i];
        }
    }

    private void ChooseUpgrade(int index)
    {
        switch (index)
        {
            case 0: // Wider paddle
                if (playerPaddle != null)
                {
                    Vector3 scale = playerPaddle.localScale;
                    scale.y += paddleWidthIncrease;
                    playerPaddle.localScale = scale;
                }
                break;

            case 1: // Faster ball
                speedBonus += 1f;
                break;

            case 2: // Score multiplier
                scoreMultiplier += 1f;
                break;

            default:
                return;
        }

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        choosingUpgrade = false;
        round++;

        Time.timeScale = 1f;

        if (ball != null)
        {
            float newSpeed = startingBallSpeed
                           + (round - 1) * speedIncreasePerRound
                           + speedBonus;
            ball.SetSpeed(newSpeed);
        }

        Debug.Log($"Round {round} started. Score multiplier: {scoreMultiplier}");
    }

    public int GetScoreMultiplier()
    {
        return Mathf.RoundToInt(scoreMultiplier);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}