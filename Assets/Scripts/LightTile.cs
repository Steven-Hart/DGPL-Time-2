using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTile : MonoBehaviour {

    public GameObject tileHighlight;
    public Transform parent;

    public void Highlight(Vector3 playerLocation)
    {
        Vector3 position = new Vector3(playerLocation.x, tileHighlight.transform.position.y, playerLocation.z);
        GameObject highlight = Instantiate(tileHighlight, position, tileHighlight.transform.rotation, parent);
    }
}
