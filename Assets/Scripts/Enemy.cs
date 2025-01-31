﻿using System.Collections;
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
    public Direction currentDirection = Direction.Right;
	public bool debug = false;
    

    // Use this for initialization
    void Start()
    {
        Moves = 1f;
    }

	void Update()
	{
	}

    public void ChooseDirection(bool blocked = false)
    {
		if (blocked)
		{
			Vector3 movePosition;
			switch (currentDirection)
			{
				case Direction.Right:
					transform.rotation = Quaternion.Euler(0, -90, 0);
					movePosition = new Vector3(0, 0, -scaledMoveDistance); //Right
					break;
				case Direction.Left:
					transform.rotation = Quaternion.Euler(0, 90, 0);
					movePosition = new Vector3(0, 0, scaledMoveDistance);
					break;
				case Direction.Up:
					transform.rotation = Quaternion.Euler(0, 180, 0);
					movePosition = new Vector3(scaledMoveDistance, 0, 0);
					break;
				case Direction.Down:
				default:
					transform.rotation = Quaternion.Euler(0, 0, 0);
					movePosition = new Vector3(-scaledMoveDistance, 0, 0);
					break;
			}
			Collider[] collisions = Physics.OverlapBox(transform.localPosition + movePosition, new Vector3(0.1f, 0.1f, 0.1f)); // Check for obstacles
			foreach (Collider col in collisions)
			{
				switch (col.tag)
				{
					case "Obstacle":
						return;
					default:
						continue;
				}
			}
			collisions = Physics.OverlapBox(transform.localPosition + movePosition, new Vector3(0.48f, 0.8f, 0.48f)); // Check for obstacles
			foreach (Collider col in collisions)
			{
				switch (col.tag)
				{
					case "Ground":
						newPosition = movePosition;
						enemyCube.MoveAnimation(); // Play movement animation
						return;
					default:
						continue;
				}
			}
			return;
		}
		ContinueDirection();
    }

	public void ChooseDirectionOnly()
	{
		Vector3 movePosition;
		switch (currentDirection)
		{
			case Direction.Right:
				transform.rotation = Quaternion.Euler(0, -90, 0);
				movePosition = new Vector3(0, 0, -scaledMoveDistance); //Right
				break;
			case Direction.Left:
				transform.rotation = Quaternion.Euler(0, 90, 0);
				movePosition = new Vector3(0, 0, scaledMoveDistance);
				break;
			case Direction.Up:
				transform.rotation = Quaternion.Euler(0, 180, 0);
				movePosition = new Vector3(scaledMoveDistance, 0, 0);
				break;
			case Direction.Down:
			default:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				movePosition = new Vector3(-scaledMoveDistance, 0, 0);
				break;
		}
		Collider[] collisions = Physics.OverlapBox(transform.localPosition + movePosition, new Vector3(0.1f, 0.1f, 0.1f)); // Check for obstacles
		foreach (Collider col in collisions)
		{
			switch (col.tag)
			{
				case "Obstacle":
					TurnAround(false);
					return;
				default:
					continue;
			}
		}
		collisions = Physics.OverlapBox(transform.localPosition + movePosition, new Vector3(0.48f, 0.8f, 0.48f)); // Check for obstacles
		foreach (Collider col in collisions)
		{
			switch (col.tag)
			{
				case "Ground":
					return;
				default:
					continue;
			}
		}
		TurnAround(false);
	}

    private void EnemyMove(float x, float y, float z)
    {
        Vector3 movePosition = transform.position + new Vector3(x, y, z); // Change to fixed space movement later
        Vector3 relativePosition = movePosition - transform.position;
        relativePosition = relativePosition/2;
        Collider[] collisions = Physics.OverlapBox(movePosition - relativePosition, new Vector3(0.1f, 0.1f, 0.1f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Obstacle":
					TurnAround();
                    return;
                default:
                    continue;
            }
        }
        collisions = Physics.OverlapBox(movePosition, new Vector3(0.48f, 0.8f, 0.48f)); // Check for obstacles
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
		TurnAround();
    }

	private void ContinueDirection()
	{
		switch (currentDirection)
		{
			case Direction.Right:
				transform.rotation = Quaternion.Euler(0, -90, 0);
				EnemyMove(0, 0, -scaledMoveDistance); //Right
				break;
			case Direction.Left:
				transform.rotation = Quaternion.Euler(0, 90, 0);
				EnemyMove(0, 0, scaledMoveDistance);
				break;
			case Direction.Up:
				transform.rotation = Quaternion.Euler(0, 180, 0);
				EnemyMove(scaledMoveDistance, 0, 0);
				break;
			case Direction.Down:
			default:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				EnemyMove(-scaledMoveDistance, 0, 0);
				break;
		}
	}
	private void TurnAround(bool move = true)
	{
		switch (currentDirection)
		{
			case Direction.Left:
				transform.rotation = Quaternion.Euler(0, -90, 0);
				currentDirection = Direction.Right;
				break;
			case Direction.Right:
				transform.rotation = Quaternion.Euler(0, 90, 0);
				currentDirection = Direction.Left;
				break;
			case Direction.Up:
				transform.rotation = Quaternion.Euler(0, 180, 0);
				currentDirection = Direction.Down;
				break;
			case Direction.Down:
			default:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				currentDirection = Direction.Up;
				break;
		}
		if (move)
			ChooseDirection(true);
	}

    public void TranslateEnemy()
    {
        transform.position += newPosition;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Lose(); 
        }
    }
}

public enum Direction
{
	Left,
	Right,
	Up,
	Down
}