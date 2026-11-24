using UnityEngine;

public class CPU : MonoBehaviour
{
    private enum State
    {
        WaitForServe,
        TrackBall,
        Recover
    }

    private const int CpuUpgradeCount = 4;

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

    [Header("CPU power-ups")]
    [SerializeField, Min(0f)] private float paddleWidthIncrease = 0.5f;
    [SerializeField, Min(0f)] private float moveSpeedIncrease = 1f;
    [SerializeField, Min(0f)] private float followBallChanceIncrease = 0.1f;
    [SerializeField, Min(0f)] private float decisionIntervalDecrease = 0.15f;

    [Header("Player power-ups")]
    [SerializeField, Min(0f)] private float playerPaddleShrink = 0.25f;
    [SerializeField, Min(0f)] private float playerMoveSpeedReduction = 1f;
    [SerializeField, Min(0f)] private float playerDecisionDelay = 0.3f;

    private State currentState = State.WaitForServe;
    private Rigidbody2D ballBody;
    private float targetY;
    private float nextDecisionTime;
    private float currentMoveSpeed;
    private float currentFollowBallChance;
    private float currentDecisionInterval;

    private void Awake()
    {
        if (ball == null && gameBall != null)
            ball = gameBall.transform;

        if (ball != null)
            ballBody = ball.GetComponent<Rigidbody2D>();

        currentMoveSpeed = moveSpeed;
        currentFollowBallChance = followBallChance;
        currentDecisionInterval = decisionInterval;
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
        nextDecisionTime = Time.time + currentDecisionInterval;

        targetY = Random.value < currentFollowBallChance
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
        bool cpuIsRightOfBall = transform.position.x > ball.position.x;
        return cpuIsRightOfBall
            ? ballBody.linearVelocity.x < 0f
            : ballBody.linearVelocity.x > 0f;
    }

    public void ChooseRandomUpgrade()
    {
        int upgrade = Random.Range(0, CpuUpgradeCount);

        switch (upgrade)
        {
            case 0:
                Vector3 scale = transform.localScale;
                scale.y += paddleWidthIncrease;
                transform.localScale = scale;
                Debug.Log("CPU power-up: wider paddle.", this);
                break;

            case 1:
                currentMoveSpeed += moveSpeedIncrease;
                Debug.Log("CPU power-up: faster movement.", this);
                break;

            case 2:
                currentFollowBallChance = Mathf.Clamp01(
                    currentFollowBallChance + followBallChanceIncrease
                );
                Debug.Log("CPU power-up: improved tracking.", this);
                break;

            case 3:
                currentDecisionInterval = Mathf.Max(
                    0.1f,
                    currentDecisionInterval - decisionIntervalDecrease
                );
                Debug.Log("CPU power-up: faster reactions.", this);
                break;
        }
    }

    public void ApplyPlayerPaddleShrink()
    {
        Vector3 scale = transform.localScale;
        scale.y = Mathf.Max(0.5f, scale.y - playerPaddleShrink);
        transform.localScale = scale;
    }

    public void ApplyPlayerMovementSlow()
    {
        currentMoveSpeed = Mathf.Max(
            1f,
            currentMoveSpeed - playerMoveSpeedReduction
        );
    }

    public void ApplyPlayerDecisionDelay()
    {
        currentDecisionInterval += playerDecisionDelay;
    }
}