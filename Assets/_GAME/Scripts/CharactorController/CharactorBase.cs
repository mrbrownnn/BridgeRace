namespace _GAME.Scripts
{
    using UnityEngine;
    using System.Collections.Generic;
    using _GAME.Scripts.Enum;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform bricksHolder;
    [SerializeField] protected SkinnedMeshRenderer characterRenderer;
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected GameObject brickPrefab;
    [SerializeField] protected float brickYOffset = 0.25f;

    protected List<PlayerBrick> playerBricks = new List<PlayerBrick>();
    protected ColorType charColor;
    protected Rigidbody rb;
    protected bool isMoving = false;
    protected int currentFloor = 0;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnInit(ColorType color)
    {
        charColor = color;
        ChangeColor();
        ClearBricks();
    }

    public void ChangeColor()
    {
        Material material = colorData.GetMaterialByColorType(charColor);
        if (characterRenderer != null && material != null)
        {
            characterRenderer.material = material;
        }
    }

    public void AddBrick(ColorType brickColor)
    {
        Vector3 brickPosition = bricksHolder.position;
        brickPosition.y += playerBricks.Count * brickYOffset;

        GameObject brickObj = Instantiate(brickPrefab, brickPosition, Quaternion.identity, bricksHolder);
        PlayerBrick brick = brickObj.GetComponent<PlayerBrick>();
        brick.SetColor(brickColor, colorData.GetMaterialByColorType(brickColor));
        playerBricks.Add(brick);
    }

    public bool RemoveBrick()
    {
        if (playerBricks.Count > 0)
        {
            int lastIndex = playerBricks.Count - 1;
            PlayerBrick brick = playerBricks[lastIndex];
            playerBricks.RemoveAt(lastIndex);
            Destroy(brick.gameObject);
            return true;
        }
        return false;
    }
    public void ClearBricks()
    {
        foreach (PlayerBrick brick in playerBricks)
        {
            Destroy(brick.gameObject);
        }
        playerBricks.Clear();
    }
    public void ChangeAnimation(string animationName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(animationName, value);
        }
    }
    public bool CanClimbStairs()
    {
        return playerBricks.Count > 0;
    }
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }
    public int GetCurrentFloor()
    {
        return currentFloor;
    }
    public int GetBrickCount()
    {
        return playerBricks.Count;
    }
    public ColorType GetCharColor()
    {
        return charColor;
    }
    protected bool IsGoingUpStairs()
    {
        return transform.forward.z > 0;
    }
}
}