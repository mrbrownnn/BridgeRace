namespace _GAME.Scripts
{
    using UnityEngine;
    using _GAME.Scripts.Enum;
    using _GAME.Scripts.Sound;

public class Player : Character
{
    [SerializeField] private Joystick joystick;
    private Vector3 moveDirection;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (joystick == null) return;

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (Mathf.Abs(horizontal) > 0.2f || Mathf.Abs(vertical) > 0.2f)
        {
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            Move(moveDirection);
            ChangeAnimation("IsRunning", true);
        }
        else
        {
            ChangeAnimation("IsRunning", false);
        }
    }

    public void Move(Vector3 direction)
    {
        // Rotate character to face movement direction
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Move character
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GroundBrick"))
        {
            GroundBrick groundBrick = other.GetComponent<GroundBrick>();
            if (groundBrick != null && !groundBrick.IsCollected && groundBrick.ColorType == charColor)
            {
                groundBrick.Collect();
                AddBrick(charColor);
                SoundManager.Instance.PlaySound("PickupBrick");
            }
        }
        else if (other.CompareTag("Stair"))
        {
            Stair stair = other.GetComponent<Stair>();
            if (stair != null && stair.ColorType == ColorType.BLUE) // BLUE means no color assigned yet
            {
                if (CanClimbStairs() && IsGoingUpStairs())
                {
                    RemoveBrick();
                    stair.SetColor(charColor, colorData.GetMaterialByColorType(charColor));
                    SoundManager.Instance.PlaySound("PlaceBrick");
                }
            }
            else if (stair != null && stair.ColorType != charColor)
            {
                // Cannot climb stairs of different color
                if (IsGoingUpStairs())
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        else if (other.CompareTag("FinishPoint"))
        {
            GameManager.Instance.SetGameState(GameState.Win);
            ChangeAnimation("IsVictory", true);
            SoundManager.Instance.PlaySound("Victory");
        }
    }
}
}