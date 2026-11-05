using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] private bool playerScoresHere;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Ball ball;

    [SerializeField] private RunManager runManager;

    private bool hasScored;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball") || hasScored)
            return;

        if (scoreManager == null || ball == null)
        {
            Debug.LogError("Assign ScoreManager and Ball in the GoalZone Inspector.", this);
            return;
        }

        hasScored = true;

        if (playerScoresHere)
        {
            scoreManager.PlayerScores();
            if (runManager != null)
    runManager.PlayerScoredRound();
        }
        else
            scoreManager.CpuScores();

        ball.ResetBall();
        ball.Launch();
        
        // Prevent another score until the ball leaves this trigger.
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
            hasScored = false;
    }
}