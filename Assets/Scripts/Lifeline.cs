using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifeline : MonoBehaviour {

	private GameObject glowParent, overlayParent;

	void Start () {
		glowParent = transform.GetChild(0).gameObject;
		overlayParent = transform.GetChild(1).gameObject;
	}
	
	public void LoseLife()
	{
		if(glowParent == null || overlayParent == null)
			return;
		PlayAllInAnimators(glowParent.transform.GetComponentsInChildren<Animator>());
		PlayAllInAnimators(overlayParent.transform.GetComponentsInChildren<Animator>());
	}

	private void PlayAllInAnimators(Animator[] childAnimators)
	{
		foreach (Animator animator in childAnimators)
		{
			animator.Play("Life Fade Out");
		}
	}
}
