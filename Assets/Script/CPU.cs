using UnityEngine;

public class CPU : MonoBehaviour
{[
    SerializeField] private Transform ball;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float deadZone = 0.15f;

    private void Update()
    {
        if (ball == null) return;

        float difference = ball.position.y - transform.position.y;

        if (Mathf.Abs(difference) > deadZone)
        {
            float direction = Mathf.Sign(difference);
            float newY = transform.position.y + direction * moveSpeed * Time.deltaTime;
            newY = Mathf.Clamp(newY, minY, maxY);

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );
        }
    }
}
