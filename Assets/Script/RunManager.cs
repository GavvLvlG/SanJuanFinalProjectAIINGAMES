using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] upgradeButtonLabels;

    [Header("Run settings")]
    [SerializeField] private float startingBallSpeed = 5f;
    [SerializeField] private float speedIncreasePerRound = 0.5f;

    private int round = 1;
    private float speedBonus;
    private bool choosingUpgrade;

    private string[] upgradeNames =
    {
        "Faster ball",
        "Extra life",
        "Bonus score"
    };

    private void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    // Call this when the player scores.
    public void PlayerScoredRound()
    {
        if (choosingUpgrade) return;

        choosingUpgrade = true;
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int upgradeIndex = i;
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ChooseUpgrade(upgradeIndex));

            if (i < upgradeButtonLabels.Length)
                upgradeButtonLabels[i].text = upgradeNames[i];
        }
    }

    private void ChooseUpgrade(int index)
    {
        switch (index)
        {
            case 0: // Faster ball
                speedBonus += 1f;
                break;

            case 1: // Extra life placeholder
                Debug.Log("Add your extra-life logic here.");
                break;

            case 2: // Bonus score placeholder
                Debug.Log("Add your bonus-score logic here.");
                break;

            default:
                return;
        }

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

        Debug.Log($"Round {round} started.");
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}