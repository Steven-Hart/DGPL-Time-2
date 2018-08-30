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
                //transform.parent = playerTransform; // Camera moves with player
                transform.position = new Vector3(-2, 8, -5); // Up for change when level is made
                transform.rotation = Quaternion.Euler(45, 45, 0);
                GetComponent<Camera>().orthographic = true;
                GetComponent<Camera>().cullingMask = ~(1 << 8); // Hide 2D only layer
                break;
            case Perspectives.View3D: // An optional option just in case
                transform.parent = null;
                transform.position = new Vector3(5, 10, 0);
                transform.LookAt(playerTransform);
                GetComponent<Camera>().orthographic = false;
                GetComponent<Camera>().cullingMask = ~(1 << 8); // Hide 2D only layer
                break;
            case Perspectives.View2D: // Default: Static top down 2D view
            default:
                transform.parent = playerTransform; // Camera moves with player
                transform.position = new Vector3(3, 10, 0);
                transform.rotation =Quaternion.Euler(90, 90, 0); // Top down
                GetComponent<Camera>().orthographic = true;
                GetComponent<Camera>().cullingMask = -1;
                break;
        }
    }

    public void CameraMove(Vector3 translatePosition)
    {
        transform.position += translatePosition;
    }
}
