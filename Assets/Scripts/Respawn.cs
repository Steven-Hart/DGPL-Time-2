using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {


    public Vector3 adjustedRespawn()
    {
        Vector3 respawnPoint = new Vector3();
        respawnPoint.x = (float)Math.Round(gameObject.transform.position.x, MidpointRounding.AwayFromZero) / 2;
        respawnPoint.z = (float)Math.Round(gameObject.transform.position.z, MidpointRounding.AwayFromZero) / 2;
        respawnPoint.y = 1.5f; //(float)Math.Round(gameObject.transform.y, MidpointRounding.AwayFromZero) / 2;

        return respawnPoint;

    } 
}
