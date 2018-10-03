using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour {

    private Camera cam;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        transform.rotation = cam.transform.rotation;
        transform.position = cam.transform.position - offset;
	}
}
