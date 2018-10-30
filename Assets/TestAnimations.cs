using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimations : MonoBehaviour {

    public Animator[] anims;
    private AnimCheck[] animChecks;

    private void Start()
    {
        animChecks = new AnimCheck[anims.Length];
        for (int i = 0; i < anims.Length; i++)
        {
            animChecks[i] = anims[i].gameObject.GetComponent<AnimCheck>();
        }
    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //anims[1].Play("Level Disappear 01");
        //    anims[0].SetBool("Fade In", true);
        //    anims[1].SetBool("Level Visible", false);

        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    anims[0].SetBool("Fade In", false);
        //    anims[1].SetBool("Level Visible", true);
        //}
        //if (animChecks[0].Finished)
        //{
        //    Debug.Log("Finished");
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anims[0].Play("Fade In");
            anims[0].Play("Fade Out");
        }
    }

    private bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
