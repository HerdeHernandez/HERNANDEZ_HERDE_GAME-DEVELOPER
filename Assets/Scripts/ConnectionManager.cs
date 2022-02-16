using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Slider slider;
    public GameObject load;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        slider.value = 0;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
       slider.value = 1;
       // print("asd");
    }

    private void Update()
    {
       

        if (slider.value < .9f)
        {
            slider.value += .005f;
        }
        else if (slider.value == 1)
        {
            load.SetActive(false);

        }
    }
}
