using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGateAnimation : MonoBehaviour {

    public Animator[] anims;
    
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            anims[0].Play("Disable Gate");
            anims[2].Play("Fade In");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anims[1].Play("Disable Gate");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anims[2].Play("Fade In");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anims[2].Play("Fade Out");
        }
    }
}
