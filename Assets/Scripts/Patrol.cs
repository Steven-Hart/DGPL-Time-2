using UnityEngine;

public class Patrol : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(Vector3 movePosition)
    {
        Collider[] boxColliders = Physics.OverlapBox(transform.position + movePosition, new Vector3(0.4f, 0.4f, 0.4f));
        if (boxColliders.Length != 0)
        {
            foreach (Collider col in boxColliders)
            {
                if(col.CompareTag("obstacle"))
                {
                    // Invert Direction
                }
            }
        }
    }


}
