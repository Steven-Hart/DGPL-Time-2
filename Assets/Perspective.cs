using UnityEngine;

public enum Perspectives
{
    View3D, View2D, Isometric
}

[RequireComponent(typeof(Camera))]
public class Perspective : MonoBehaviour
{
    public Perspectives selectedPerspective;
    public Transform playerTransform;

    void Start()
    {
        switch (selectedPerspective)
        {
            case Perspectives.Isometric: // Orthographic iso view
                transform.parent = playerTransform; // Camera moves with player
                transform.position = new Vector3(-10, 5, -5); // Up for change when level is made
                transform.rotation = Quaternion.Euler(25, 45, 0);
                GetComponent<Camera>().orthographic = true;
                break;
            case Perspectives.View3D: // An optional option just in case
                transform.parent = null;
                transform.position = new Vector3(5, 10, 0);
                transform.LookAt(playerTransform);
                GetComponent<Camera>().orthographic = false;
                break;
            case Perspectives.View2D: // Default: Static top down 2D view
            default:
                transform.parent = null;
                transform.position = new Vector3(0, 10, 0);
                transform.rotation = Quaternion.Euler(90, 0, 0); // Top down
                GetComponent<Camera>().orthographic = true;
                break;
        }
    }

}
