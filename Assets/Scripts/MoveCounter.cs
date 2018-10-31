using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCounter : MonoBehaviour {

	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		ResetMove();
	}

	public void LoseMove()
	{
		animator.Play("UseMove");
	}

	private void DisableSelf()
	{
		gameObject.SetActive(false);
	}

	public void ResetMove()
	{
		animator.Play("StartMove");
	}
}
