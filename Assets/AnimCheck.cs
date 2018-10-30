using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCheck : MonoBehaviour {

    private bool finished;

    public bool Finished
    {
        get
        {
            return finished;
        }

        set
        {
            finished = value;
        }
    }

    private void Awake()
    {
        finished = false;
    }

    private void AnimStart()
    {
        finished = false;
    }

    private void AnimEnd()
    {
        finished = true;
    }
}
