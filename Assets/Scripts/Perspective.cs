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
    public Vector3 originPosition, startPosition;

    private Vector3 _targetcameraposition;

    void Start()
    {
        switch (selectedPerspective)
        {
            case Perspectives.Isometric: // Orthographic iso view
                //transform.parent = playerTransform; // Camera moves with player
                transform.position = startPosition; // Up for change when level is made
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
        _targetcameraposition = originPosition = transform.position;
        
    }

    void Update()
    {
        CameraMove(TargetCameraPosition);
    }

    public void CameraMove(Vector3 targetPosition)
    {
        //Vector3 targetPosition = transform.position + translatePosition;
        Vector3 movePosition = new Vector3((targetPosition.x - transform.position.x)*0.1f, 0, (targetPosition.z - transform.position.z) * 0.1f);
        transform.position += movePosition;
        //Debug.Log(TargetCameraPosition);
        //transform.position += translatePosition;

    }

    public Vector3 TargetCameraPosition
    {
        get
        {
            return _targetcameraposition;
        }
        set
        {
            _targetcameraposition = transform.position + value;
        }
    }

    public void CameraPositionReset()
    {
        _targetcameraposition = transform.position = originPosition;
    }
}
