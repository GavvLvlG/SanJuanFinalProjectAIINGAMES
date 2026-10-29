using UnityEngine;

public class GoalZone : MonoBehaviour
{
   [SerializeField] private bool playerScoresHere;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Ball ball;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (playerScoresHere)
            scoreManager.PlayerScores();
        else
            scoreManager.CpuScores();

        ball.ResetBall();
    }
}
