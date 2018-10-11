using UnityEngine;
using System.Collections.Generic;

public class GateTrigger : MonoBehaviour
{
    public List<GameObject> Link = new List<GameObject>();
    public AudioClip sound_gate;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (GameObject gate in Link)
            {
                gate.SetActive(false); // Disables gate
                if (!gate.activeSelf)
                    source.PlayOneShot(sound_gate, 1.0f);
                //gate.SetActive(!gate.activeSelf); // Toggles Gate
            }
        }
    }
}
