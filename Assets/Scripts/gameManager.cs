using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using System.Text;

public class gameManager : MonoBehaviour 
{ 
    public List<string> players = new List<string>();
    public GameObject[] gos;
    Transform scoreBoard;
    string id;

    void Start()
    {
        scoreBoard = GameObject.Find("Content").transform;
    }

    void Update()
    {
        players.Clear();
        gos = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject go in gos)
        {
            if (!players.Contains(go.GetComponent<PlayerMovement>().ID))
                players.Add(go.GetComponent<PlayerMovement>().ID);
        }

        foreach (Transform child in scoreBoard)
        {
            if (!players.Contains(child.name) && child.name != "NameScore")
                Destroy(child.gameObject);
        }

        /* foreach (string go in gos)
         {
             if(go.GetComponent<PlayerMovement>())
         }*/

    }
}


