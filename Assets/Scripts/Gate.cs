using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	public void DisableGate()
	{
		animator.Play("Disable Gate");
	}

	private void DisableGateFinal()
	{
		gameObject.SetActive(false);
	}
}
