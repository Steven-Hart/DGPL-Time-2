using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour {

    private float speed;

    private void Start()
    {
        speed = 100f;
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.left, Time.deltaTime * speed);
	}
}
