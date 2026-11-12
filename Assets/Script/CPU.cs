using UnityEngine;

public class CPU : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float deadZone = 0.15f;

    [Header("AI Behavior")]
    [SerializeField, Range(0f, 1f)] private float followBallChance = 0.65f;
    [SerializeField] private float decisionInterval = 0.8f;

    [SerializeField] private Ball gameBall;
    [SerializeField] private float paddleWidthIncrease = 0.5f;
    [SerializeField] private float ballSpeedIncrease = 1f;

    private float targetY;
    private float nextDecisionTime;

    private float cpuSpeedBonus;

public void ChooseRandomUpgrade()
{
    int upgrade = Random.Range(0, 3);

    switch (upgrade)
    {
        case 0: // Wider CPU paddle
            Vector3 scale = transform.localScale;
            scale.x += paddleWidthIncrease;
            transform.localScale = scale;
            break;

        case 1: // Faster ball
            cpuSpeedBonus += ballSpeedIncrease;

            if (gameBall != null)
                gameBall.SetSpeed(5f + cpuSpeedBonus);
            break;

        case 2: // CPU score bonus
            // Add CPU score bonus logic here if desired.
            break;
    }

    Debug.Log($"CPU chose upgrade {upgrade}.");
}

    private void Start()
    {
        ChooseTarget();
    }

    private void Update()
    {
        if (ball == null) return;

        if (Time.time >= nextDecisionTime)
            ChooseTarget();

        float difference = targetY - transform.position.y;

        if (Mathf.Abs(difference) > deadZone)
        {
            float newY = Mathf.MoveTowards(
                transform.position.y,
                targetY,
                moveSpeed * Time.deltaTime
            );

            newY = Mathf.Clamp(newY, minY, maxY);

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );
        }
    }

    private void ChooseTarget()
    {
        nextDecisionTime = Time.time + decisionInterval;

        if (ball != null && Random.value < followBallChance)
            targetY = Mathf.Clamp(ball.position.y, minY, maxY);
        else
            targetY = Random.Range(minY, maxY);
    }
}
