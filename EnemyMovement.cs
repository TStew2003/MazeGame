using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float gridSize;
    public LayerMask layerMask;
    public GridMovement player;

    private Vector2 targetPosition;
    private Vector2 nextPosition;
    private Vector2 lastDirection;
    private RaycastHit2D hit;
    private Vector3 startPos;
    private bool isChasingPlayer;

    public enum EnemyState
    {
        Patrolling,
        Chasing,
    }

    private EnemyState currentState;

    void Start()
    {
        // Pick a random direction to start with
        lastDirection = GetRandomDirection();
        targetPosition = (Vector2)transform.position;
        nextPosition = targetPosition + lastDirection * gridSize;
        startPos = transform.position;
        currentState = EnemyState.Patrolling;
        isChasingPlayer = false;
    }

    void Update()
    {
        if (!isChasingPlayer && SensedPlayer())
        {
            isChasingPlayer = true;
            moveSpeed++;
            currentState = EnemyState.Chasing;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                UpdatePatrolling();
                break;
            case EnemyState.Chasing:
                UpdateChasing();
                break;
        }
    }

    void UpdatePatrolling()
    {
        // Check if we've reached our current target position
        if ((Vector2)transform.position == targetPosition)
        {

            // Try to move in a new direction
            Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
            List<Vector2> validDirections = new List<Vector2>();

            foreach (Vector2 dir in directions)
            {
                // Don't try to move in the opposite direction
                if (dir != -lastDirection)
                {
                    Vector2 newPosition = (Vector2)transform.position + dir * gridSize;
                    if (ValidNextCell(newPosition))
                    {
                        validDirections.Add(dir);
                    }
                }
            }

            // If we have valid directions to move in, pick one randomly
            if (validDirections.Count > 0)
            {
                int randomIndex = Random.Range(0, validDirections.Count);
                Vector2 newDirection = validDirections[randomIndex];

                if (newDirection == Vector2.right)
                {
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }
                if (newDirection == Vector2.left)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }

                lastDirection = newDirection;
                targetPosition = (Vector2)transform.position + newDirection * gridSize;
                nextPosition = targetPosition + newDirection * gridSize;
            }
            // If we don't have any valid directions to move in, turn around
            else
            {
                lastDirection = -lastDirection;
                targetPosition = (Vector2)transform.position + lastDirection * gridSize;
                nextPosition = targetPosition + lastDirection * gridSize;
            }
        }

        // Move towards our target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    }

    void UpdateChasing()
    {
        // Check if we've reached our current target position
        if ((Vector2)transform.position == targetPosition)
        {
            if (!ValidNextCell(nextPosition))
            {
                currentState = EnemyState.Patrolling;
                isChasingPlayer = false;
                lastDirection = -lastDirection;
                targetPosition = (Vector2)transform.position + lastDirection * gridSize;
                nextPosition = targetPosition + lastDirection * gridSize;
            }
            else
            {
                targetPosition = (Vector2)transform.position + lastDirection * gridSize;
                nextPosition = targetPosition + lastDirection * gridSize;
            }
        }
        // Move towards our target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    }

    // Helper method to check if a given cell is valid (i.e. there's no obstacle there)
    private bool ValidNextCell(Vector2 position)
    {
        if (player.IsHiding() && !isChasingPlayer)
        {
            Collider2D collider = Physics2D.OverlapCircle(position, 0.1f);
            return collider == null;
        }
        else
        {
            Collider2D collider = Physics2D.OverlapCircle(position, 0.1f, ~layerMask);
            return collider == null;
        }

    }

    // Helper method to get a random direction
    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        int randomIndex = Random.Range(0, directions.Length);
        return directions[randomIndex];
    }

    private bool SensedPlayer()
    {
        hit = Physics2D.Raycast(transform.position, lastDirection, 5f, layerMask);

        // Visualize the raycast
        //Debug.DrawRay(transform.position, lastDirection, Color.red, 0.1f);

        if (hit.collider != null && !player.IsHiding())
        {
            //Debug.Log(this.gameObject.name + ": " + hit.collider.gameObject.name);
            // An enemy is detected
            return true;
        }
        else
        {
            // No enemy is detected
            return false;
        }
    }

    public void Reset()
    {
        this.gameObject.SetActive(false);
        this.transform.position = startPos;
        this.gameObject.SetActive(true);

        // Pick a random direction to start with
        lastDirection = GetRandomDirection();
        targetPosition = (Vector2)transform.position;
        nextPosition = targetPosition + lastDirection * gridSize;

        currentState = EnemyState.Patrolling;
        isChasingPlayer = false;

    }

}

/*
This was scrapped because the maze walls were ignored when raycasting
private bool FacingEnemy()
{
    hit = Physics2D.Raycast(transform.position, lastDirection, 20f, layerMask);

    // Visualize the raycast
    Debug.DrawRay(transform.position, lastDirection, Color.red, 0.1f);

    if (hit.collider != null)
    {
        Debug.Log(this.gameObject.name + ": " + hit.collider.gameObject.name);
        // An enemy is detected
        return true;
    }
    else
    {
        // No enemy is detected
        return false;
    }
}
*/

