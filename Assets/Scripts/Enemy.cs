using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float scaledMoveDistance = 1;
    public bool moveDelay;
    public EnemyCube enemyCube;


    private Vector3 newPosition;
    public float Moves;
    private enum Direction
    {
        Left,
        Right
    }
    private Direction currentDirection = Direction.Right;
    

    // Use this for initialization
    void Start()
    {
        Moves = 1f;
    }



    public void ChooseDirection()
    {
        if(currentDirection == Direction.Right)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            EnemyMove(0, 0, -scaledMoveDistance); //Right
        }
        else if (currentDirection == Direction.Left)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            EnemyMove(0, 0, scaledMoveDistance);
        }
        else
        {
            Debug.LogError("Unkown enemy Direction");
        }
    }

    private void EnemyMove(float x, float y, float z)
    {
        Vector3 movePosition = transform.position + new Vector3(x, y, z); // Change to fixed space movement later
        Vector3 relativePosition = movePosition - transform.position;
        relativePosition = relativePosition/2;
        Collider[] collisions = Physics.OverlapBox(movePosition - relativePosition, new Vector3(0.1f, 1.1f, 0.1f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Obstacle":
                    if (currentDirection == Direction.Left)
                    {
                        currentDirection = Direction.Right;
                    }
                    else
                    {
                        currentDirection = Direction.Left;
                    }
                    ChooseDirection();
                    return;
                default:
                    continue;
            }
        }
        collisions = Physics.OverlapBox(movePosition, new Vector3(0.4f, 1.1f, 0.4f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Ground":
                    newPosition = new Vector3(x, y, z);
                    enemyCube.MoveAnimation(); // Play movement animation
                    return;
                default:
                    continue;
            }

        }
        if (currentDirection == Direction.Left)
        {
            currentDirection = Direction.Right;
        }
        else
        {
            currentDirection = Direction.Left;
        }
        ChooseDirection();
    }

    public void TranslateEnemy()
    {
        transform.position += newPosition;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.NextLife(); 
        }
    }
}
