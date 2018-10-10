using UnityEngine;
using System.Collections.Generic;

public class GateTrigger : MonoBehaviour
{
    public List<GameObject> Link = new List<GameObject>();

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (GameObject gate in Link)
            {
                gate.SetActive(false); // Disables gate
                //gate.SetActive(!gate.activeSelf); // Toggles Gate
            }
        }
    }
}
