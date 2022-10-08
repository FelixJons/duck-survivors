using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler;
    private Rigidbody2D rb2;
    private Vector2 moveDirection;
    private bool isFacingRight = true;
    [SerializeField] private float speed = 3f;
    
    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        isFacingRight = true;
    }

    private void Update()
    {
        SetDirection();
        MovePosition();
    }

    private void MovePosition()
    {
        //transform.position += (Vector3) moveDirection * (Time.deltaTime * speed);
        rb2.velocity = moveDirection.normalized * speed;
    }

    private void SetDirection()
    {
        moveDirection = playerInputHandler.MoveInput;

        switch (moveDirection.x)
        {
            case > 0 when !isFacingRight:
            case < 0 when isFacingRight:
                Flip();
                break;
        }
    }

    private void Flip()
    {
        transform.eulerAngles = isFacingRight ? new Vector3(0, 180) : new Vector3(0, 0);
        isFacingRight = !isFacingRight;
    }
}