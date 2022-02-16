using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text;
using System;

public class playerSpawn : MonoBehaviour
{
    void Start()
    {
        GameObject player = Resources.Load("Player") as GameObject;

        Vector3 playerPos = new Vector3(UnityEngine.Random.Range(40, -40), 0, UnityEngine.Random.Range(40, -40));
        var inPlayer = PhotonNetwork.Instantiate(player.name, playerPos, Quaternion.identity);

        playerData playerData = GameObject.Find("PlayerData").GetComponent<playerData>();

        PhotonView p = inPlayer.GetComponent<PhotonView>();

        //if(p.IsMine)
           p.RPC("setName", RpcTarget.AllBufferedViaServer, playerData.playerName, playerData.playerID);
    }
}
