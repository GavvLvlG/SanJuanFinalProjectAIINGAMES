using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IRunManager
{
    int GetScoreMultiplier();
    void PlayerScoredRound();
}

public class RunManager : MonoBehaviour, IRunManager
{
    private enum PlayerUpgrade
    {
        WiderPaddle,
        FasterBall,
        ScoreMultiplier,
        ShrinkCpuPaddle,
        SlowCpu,
        DelayCpuDecisions
    }

    private const int PlayerOfferCount = 3;
    private const int ScoreMultiplierIncrease = 1;
    private const float BallSpeedBonusPerUpgrade = 1f;

    private static readonly PlayerUpgrade[] PlayerUpgradePool =
    {
        PlayerUpgrade.WiderPaddle,
        PlayerUpgrade.FasterBall,
        PlayerUpgrade.ScoreMultiplier,
        PlayerUpgrade.ShrinkCpuPaddle,
        PlayerUpgrade.SlowCpu,
        PlayerUpgrade.DelayCpuDecisions
    };

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
    private float speedBonus;
    private int scoreMultiplier = 1;
    private bool choosingUpgrade;

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

        if (cpu != null)
            cpu.ChooseRandomUpgrade();
        else
            Debug.LogWarning("CPU is not assigned; no CPU power-up was applied.", this);

        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        if (upgradeButtons == null || upgradeButtons.Length == 0)
        {
            Debug.LogError("No upgrade buttons assigned.", this);
            FinishUpgradeSelection();
            return;
        }

        if (upgradeButtons.Length < PlayerOfferCount)
            Debug.LogWarning("Assign three upgrade buttons to show all random choices.", this);

        PlayerUpgrade[] randomizedOffers = GetRandomUpgradeOffers();
        int offerCount = Mathf.Min(PlayerOfferCount, upgradeButtons.Length);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            Button button = upgradeButtons[i];
            if (button == null)
                continue;

            button.onClick.RemoveAllListeners();

            if (i < offerCount)
            {
                PlayerUpgrade selectedUpgrade = randomizedOffers[i];
                button.interactable = true;
                button.onClick.AddListener(() => ChooseUpgrade(selectedUpgrade));

                if (upgradeButtonLabels != null &&
                    i < upgradeButtonLabels.Length &&
                    upgradeButtonLabels[i] != null)
                {
                    upgradeButtonLabels[i].text = GetUpgradeLabel(selectedUpgrade);
                }
            }
            else
            {
                button.interactable = false;
            }
        }
    }

    private PlayerUpgrade[] GetRandomUpgradeOffers()
    {
        PlayerUpgrade[] shuffledUpgrades = (PlayerUpgrade[])PlayerUpgradePool.Clone();

        for (int i = 0; i < shuffledUpgrades.Length; i++)
        {
            int randomIndex = Random.Range(i, shuffledUpgrades.Length);
            PlayerUpgrade temporaryUpgrade = shuffledUpgrades[i];
            shuffledUpgrades[i] = shuffledUpgrades[randomIndex];
            shuffledUpgrades[randomIndex] = temporaryUpgrade;
        }

        PlayerUpgrade[] offers = new PlayerUpgrade[PlayerOfferCount];
        for (int i = 0; i < offers.Length; i++)
            offers[i] = shuffledUpgrades[i];

        return offers;
    }

    private string GetUpgradeLabel(PlayerUpgrade upgrade)
    {
        switch (upgrade)
        {
            case PlayerUpgrade.WiderPaddle:
                return "Wider paddle";
            case PlayerUpgrade.FasterBall:
                return "Faster ball";
            case PlayerUpgrade.ScoreMultiplier:
                return "Score multiplier";
            case PlayerUpgrade.ShrinkCpuPaddle:
                return "Shrink CPU paddle";
            case PlayerUpgrade.SlowCpu:
                return "Slow CPU movement";
            case PlayerUpgrade.DelayCpuDecisions:
                return "Delay CPU reactions";
            default:
                return "Unknown upgrade";
        }
    }

    private void ChooseUpgrade(PlayerUpgrade upgrade)
    {
        switch (upgrade)
        {
            case PlayerUpgrade.WiderPaddle:
                if (playerPaddle != null)
                {
                    Vector3 scale = playerPaddle.localScale;
                    scale.y += paddleWidthIncrease;
                    playerPaddle.localScale = scale;
                }
                break;

            case PlayerUpgrade.FasterBall:
                speedBonus += BallSpeedBonusPerUpgrade;
                break;

            case PlayerUpgrade.ScoreMultiplier:
                scoreMultiplier += ScoreMultiplierIncrease;
                break;

            case PlayerUpgrade.ShrinkCpuPaddle:
                if (cpu != null)
                    cpu.ApplyPlayerPaddleShrink();
                break;

            case PlayerUpgrade.SlowCpu:
                if (cpu != null)
                    cpu.ApplyPlayerMovementSlow();
                break;

            case PlayerUpgrade.DelayCpuDecisions:
                if (cpu != null)
                    cpu.ApplyPlayerDecisionDelay();
                break;

            default:
                Debug.LogWarning($"Invalid upgrade: {upgrade}", this);
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

    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
