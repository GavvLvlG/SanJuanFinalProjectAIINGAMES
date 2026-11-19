using UnityEngine;

public class CPU : MonoBehaviour
{
    private enum State
    {
        WaitForServe,
        TrackBall,
        Recover
    }

    [Header("References")]
    [SerializeField] private Transform ball;
    [SerializeField] private Ball gameBall;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField, Min(0f)] private float deadZone = 0.15f;
    [SerializeField, Min(0f)] private float decisionInterval = 0.8f;
    [SerializeField, Range(0f, 1f)] private float followBallChance = 0.65f;

    [Header("Upgrades")]
    [SerializeField, Min(0f)] private float paddleWidthIncrease = 0.5f;
    [SerializeField, Min(0f)] private float moveSpeedIncrease = 1f;

    private State currentState = State.WaitForServe;
    private Rigidbody2D ballBody;
    private float targetY;
    private float nextDecisionTime;
    private float currentMoveSpeed;

    private void Awake()
    {
        if (ball == null && gameBall != null)
            ball = gameBall.transform;

        if (ball != null)
            ballBody = ball.GetComponent<Rigidbody2D>();

        currentMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (ball == null || ballBody == null)
            return;

        if (currentState == State.WaitForServe)
        {
            if (IsBallMoving())
                ChangeState(State.TrackBall);

            return;
        }

        if (currentState == State.TrackBall)
        {
            if (Time.time >= nextDecisionTime)
                ChooseTarget();

            MoveTowardTarget();

            if (IsBallMovingAwayFromCpu())
                ChangeState(State.Recover);
        }
        else if (currentState == State.Recover)
        {
            targetY = 0f;
            MoveTowardTarget();

            if (!IsBallMovingAwayFromCpu())
                ChangeState(State.TrackBall);
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.TrackBall)
            ChooseTarget();
    }

    private void ChooseTarget()
    {
        nextDecisionTime = Time.time + decisionInterval;

        targetY = Random.value < followBallChance
            ? Mathf.Clamp(ball.position.y, minY, maxY)
            : Random.Range(minY, maxY);
    }

    private void MoveTowardTarget()
    {
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            currentMoveSpeed * Time.deltaTime
        );

        newY = Mathf.Clamp(newY, minY, maxY);
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    private bool IsBallMoving()
    {
        return ballBody != null && ballBody.linearVelocity.sqrMagnitude > 0.01f;
    }

    private bool IsBallMovingAwayFromCpu()
    {
        // Works whether the CPU is on the right or left of the ball.
        bool cpuIsRightOfBall = transform.position.x > ball.position.x;
        return cpuIsRightOfBall
            ? ballBody.linearVelocity.x < 0f
            : ballBody.linearVelocity.x > 0f;
    }

    public void ChooseRandomUpgrade()
    {
        int upgrade = Random.Range(0, 2);

        switch (upgrade)
        {
            case 0:
                Vector3 scale = transform.localScale;
                scale.y += paddleWidthIncrease; 
                transform.localScale = scale;
                break;

            case 1:
                currentMoveSpeed += moveSpeedIncrease;
                break;
        }
    }
}