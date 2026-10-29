using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Assign the "Move" action from your Input Actions asset in the Inspector.
    [SerializeField] private InputActionReference moveAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}