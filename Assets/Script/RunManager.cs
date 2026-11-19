using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IRunManager
{
    int GetScoreMultiplier();
    void PlayerScoredRound();
}

public class RunManager : MonoBehaviour, IRunManager
{
    [Header("References")]
    [SerializeField] private Ball ball;
    [SerializeField] private Transform playerPaddle;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] upgradeButtonLabels;
    [SerializeField] private CPU cpu;

    [Header("Run settings")]
    [SerializeField, Min(0f)] private float startingBallSpeed = 5f;
    [SerializeField, Min(0f)] private float speedIncreasePerRound = 0.5f;
    [SerializeField, Min(0f)] private float paddleWidthIncrease = 0.5f;

    private int round = 1;
    private int roundsCompleted;
    private float speedBonus;
    private int scoreMultiplier = 1;
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
        if (choosingUpgrade)
            return;

        choosingUpgrade = true;
        Time.timeScale = 0f;

        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        if (upgradeButtons == null || upgradeButtons.Length == 0)
        {
            Debug.LogError("No upgrade buttons assigned.", this);
            FinishUpgradeSelection();
            return;
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            Button button = upgradeButtons[i];
            if (button == null)
                continue;

            int upgradeIndex = i;
            button.onClick.RemoveAllListeners();

            if (upgradeIndex < upgradeNames.Length)
            {
                button.onClick.AddListener(() => ChooseUpgrade(upgradeIndex));

                if (upgradeButtonLabels != null &&
                    i < upgradeButtonLabels.Length &&
                    upgradeButtonLabels[i] != null)
                {
                    upgradeButtonLabels[i].text = upgradeNames[i];
                }
            }
            else
            {
                button.interactable = false;
            }
        }
    }

    private void ChooseUpgrade(int index)
    {
        switch (index)
        {
            case 0:
                if (playerPaddle != null)
                {
                    // Assumes the paddle's width is its local X axis.
                    Vector3 scale = playerPaddle.localScale;
                    scale.y += paddleWidthIncrease;
                    playerPaddle.localScale = scale;
                }
                break;

            case 1:
                speedBonus += 1f;
                break;

            case 2:
                scoreMultiplier++;
                break;

            default:
                Debug.LogWarning($"Invalid upgrade index: {index}", this);
                return;
        }

        FinishUpgradeSelection();
    }

    private void FinishUpgradeSelection()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (scoreManager != null)
            scoreManager.ResetScores();

        choosingUpgrade = false;
        round++;
        Time.timeScale = 1f;

        float newSpeed = startingBallSpeed
                       + (round - 1) * speedIncreasePerRound
                       + speedBonus;

        if (ball != null)
            ball.SetSpeed(newSpeed);

        Debug.Log($"Round {round} started. Score multiplier: {scoreMultiplier}");
    }

    public void OnRoundEnded()
    {
        roundsCompleted++;

        if (roundsCompleted % 5 != 0 || choosingUpgrade)
            return;

        if (cpu == null)
        {
            Debug.LogWarning("CPU is not assigned; cannot choose the milestone upgrade.", this);
            return;
        }

        // CPU.ChooseRandomUpgrade() must call back into this manager's
        // upgrade-application logic, or otherwise handle the selected upgrade.
        cpu.ChooseRandomUpgrade();
    }

    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}