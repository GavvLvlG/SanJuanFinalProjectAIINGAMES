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

    private float targetY;
    private float nextDecisionTime;

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
