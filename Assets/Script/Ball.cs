using UnityEngine;

public class Ball : MonoBehaviour
{

    public Rigidbody2D rb;
    public float StartSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool isRight = UnityEngine.Random.value >= 0.5f;

        float xVelocity = -1f;

        if (isRight == true)
        {
            xVelocity = 1f;
        }

        float yVelocity = UnityEngine.Random.Range(-1, 1);


        rb.linearVelocity = new Vector2(xVelocity * StartSpeed, yVelocity * StartSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBall()
{
    transform.position = Vector3.zero;

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
        rb.linearVelocity = Vector2.zero;
}
}
