using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float startSpeed = 5f;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Launch();
    }

    public void ResetBall()
    {
        transform.position = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void Launch()
    {
        if (rb == null)
        {
            Debug.LogError("Ball needs a Rigidbody2D.", this);
            return;
        }

        float directionX = Random.value < 0.5f ? -1f : 1f;
        float directionY = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(directionX, directionY).normalized;
        rb.linearVelocity = direction * startSpeed;
    }
}