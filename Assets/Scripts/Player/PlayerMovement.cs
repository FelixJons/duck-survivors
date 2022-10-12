using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler;
    private Rigidbody2D rb2;
    private Vector2 moveDirection;
    private bool isFacingRight = true;
    [SerializeField] private float speed = 3f;
    private bool isRecievingPlayerInput = false;
    private Animator animator;
    

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        isFacingRight = true;
    }

    private void Update()
    {
        SetDirection();
        MovePosition();

        animator.speed = (isRecievingPlayerInput ? 1f : 0f);

    }

    private void MovePosition()
    {
        //transform.position += (Vector3) moveDirection * (Time.deltaTime * speed);
        rb2.velocity = moveDirection.normalized * speed;
    }

    private void SetDirection()
    {
        moveDirection = playerInputHandler.MoveInput;

        isRecievingPlayerInput = (moveDirection != Vector2.zero);

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