using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text;
using System;

public class DestroyObjects : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");


        PhotonView eatenBy = other.GetComponent<objectInfo>().eatenBy;

        if (eatenBy.IsMine)
        {
            eatenBy.RPC("setScore", RpcTarget.AllBufferedViaServer);
        }
       
        Destroy(other.gameObject);
    }

  
}

