using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameObject Link;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Link.SetActive(false); // Disables gate
            //Gate.SetActive(!Gate.activeSelf); // Toggles Gate
        }
    }
}
