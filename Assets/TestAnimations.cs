using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimations : MonoBehaviour {

    public Animator[] anims;
    private AnimationClip[] clips;
    private float time = 0f;

    private void Start()
    {
        clips = anims[1].runtimeAnimatorController.animationClips;
        for (int j = 0; j < clips.Length; j++)
        {
            if (clips[j].name == "Level Disappear 01")
            {
                time = clips[j].length;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("FadeCoroutine");
        }
    }

    IEnumerator FadeCoroutine()
    {
        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here
        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
    }
}
