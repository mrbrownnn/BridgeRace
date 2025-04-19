namespace _GAME.Scripts
{
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections;
    using _GAME.Scripts.Enum;

public enum BotState
{
    IDLE,
    FIND,
    BUILD
}

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float findRadius = 10f;
    [SerializeField] private int minBricksBeforeBuild = 3;
    [SerializeField] private float delayBetweenStates = 0.5f;

    private BotState currentState = BotState.IDLE;
    private Transform targetBrick;
    private Transform targetStair;
    private bool isProcessingState = false;

    protected override void Awake()
    {
        base.Awake();
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        StartCoroutine(BotStateMachine());
    }

    private IEnumerator BotStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case BotState.IDLE:
                    DoIdleState();
                    break;
                case BotState.FIND:
                    DoFindState();
                    break;
                case BotState.BUILD:
                    DoBuildState();
                    break;
            }

            yield return new WaitForSeconds(delayBetweenStates);
        }
    }

    private void DoIdleState()
    {
        if (isProcessingState) return;
        isProcessingState = true;

        ChangeAnimation("IsRunning", false);
        
        // Transition to FIND state after short delay
        StartCoroutine(TransitionAfterDelay(BotState.FIND, Random.Range(0.5f, 1.5f)));
    }

    private void DoFindState()
    {
        if (isProcessingState) return;
        isProcessingState = true;

        // Find bricks of the bot's color
        targetBrick = FindNearestBrick();

        if (targetBrick != null)
        {
            MoveToTarget(targetBrick.position);
            ChangeAnimation("IsRunning", true);
        }
        else
        {
            // If no bricks found, transition back to IDLE
            TransitionToState(BotState.IDLE);
        }

        // Check if we have enough bricks to build
        if (playerBricks.Count >= minBricksBeforeBuild)
        {
            TransitionToState(BotState.BUILD);
        }
        else
        {
            isProcessingState = false;
        }
    }

    private void DoBuildState()
    {
        if (isProcessingState) return;
        isProcessingState = true;

        // Find stairs to build
        targetStair = FindNearestStair();

        if (targetStair != null && playerBricks.Count > 0)
        {
            MoveToTarget(targetStair.position);
            ChangeAnimation("IsRunning", true);
        }
        else
        {
            // If no stairs or no bricks, go back to FIND
            TransitionToState(BotState.FIND);
        }

        // If we're out of bricks, go back to finding
        if (playerBricks.Count == 0)
        {
            TransitionToState(BotState.FIND);
        }
        else
        {
            isProcessingState = false;
        }
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(targetPosition);
        }
    }

    private Transform FindNearestBrick()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, findRadius);
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("GroundBrick"))
            {
                GroundBrick brick = collider.GetComponent<GroundBrick>();
                if (brick != null && !brick.IsCollected && brick.ColorType == charColor)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = collider.transform;
                    }
                }
            }
        }

        return nearest;
    }

    private Transform FindNearestStair()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, findRadius);
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Stair"))
            {
                Stair stair = collider.GetComponent<Stair>();
                if (stair != null && stair.ColorType == ColorType.BLUE) // BLUE means no color assigned yet
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = collider.transform;
                    }
                }
            }
        }

        return nearest;
    }

    private void TransitionToState(BotState newState)
    {
        currentState = newState;
        isProcessingState = false;
    }

    private IEnumerator TransitionAfterDelay(BotState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        TransitionToState(newState);
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
                
                // Reset target brick since we just collected it
                if (targetBrick == other.transform)
                {
                    targetBrick = null;
                }
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
                    
                    // Reset target stair since we just built on it
                    if (targetStair == other.transform)
                    {
                        targetStair = null;
                    }
                }
            }
        }
        else if (other.CompareTag("FinishPoint"))
        {
            // Bot reached finish - player loses
            GameManager.Instance.SetGameState(GameState.Lose);
            ChangeAnimation("IsVictory", true);
        }
    }
}
}