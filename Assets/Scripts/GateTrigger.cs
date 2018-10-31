using UnityEngine;
using System.Collections.Generic;

public class GateTrigger : MonoBehaviour
{
    public List<GameObject> Link = new List<GameObject>();
    public AudioClip sound_gate;
    private AudioSource source;
	private bool triggered = false;
	private Vector3 startPosition;
	private bool wasPlaying;

    void Awake()
    {
        source = GetComponent<AudioSource>();
		startPosition = transform.localPosition;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !triggered)
        {
            foreach (GameObject gate in Link)
			{
				if (gate.activeSelf)
					triggered = true;
                gate.GetComponent<Gate>().DisableGate(); // Disables gate
                //gate.SetActive(!gate.activeSelf); // Toggles Gate
            }
			if (triggered)
			{
				source.PlayOneShot(sound_gate, 1.0f);
				GetComponent<Animator>().Play("Button Press");
			}
		}
    }
}
