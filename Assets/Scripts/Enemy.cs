using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float scaledMoveDistance = 1;


    private Vector3 newPosition;
    public float Moves;
    

    // Use this for initialization
    void Start()
    {
        Moves = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.movesMade > Moves)
        {
            EnemyMove(0, 0, -scaledMoveDistance); //Right
            Moves = player.movesMade;
        }
    }

    private void EnemyMove(float x, float y, float z)
    {
        Vector3 movePosition = transform.position + new Vector3(x, y, z); // Change to fixed space movement later
        Collider[] collisions = Physics.OverlapBox(movePosition, new Vector3(0.5f, 0.5f, 0.5f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            if (col.tag == "Obstacle")
            {
                return;
            }
        }
        newPosition = new Vector3(x, y, z);
        //playerCube.MoveAnimation(); // Play movement animation
    }

    public void TranslateEnemy()
    {
        transform.position += newPosition;
    }
}
