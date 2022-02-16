using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JoinManager : MonoBehaviourPunCallbacks
{
    public List<string> rooms = new List<string>();

    public InputField inputField;
    public GameObject text;

    playerData playerData;

    public int playerCount;
    string playerID;

    void Start()
    {
        playerData = GameObject.Find("PlayerData").GetComponent<playerData>();
        generateID();
    }

    public void play()
    {
        if (inputField.text.Length > 0)
        {
            playerData.playerName = inputField.text;
            playerData.playerID = playerID;

            if (!rooms.Contains("myRoom"))
                PhotonNetwork.CreateRoom("myRoom");
            else
                if (playerCount < 5)
                PhotonNetwork.JoinRoom("myRoom");
        }
        else
        {
            text.SetActive(true);
            StartCoroutine(error());
        }
       
    }

    IEnumerator error()
    {
        yield return new WaitForSeconds(1);
        text.SetActive(false);
    }

    void generateID()
    {
        playerID = "";

        int charAmount = 40;

        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789QWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*()_+";

        for (int j = 0; j < charAmount; j++)
        {
            playerID += glyphs[Random.Range(0, glyphs.Length)];
        }
    }

    public override void OnJoinedRoom()
    {

        PhotonNetwork.LoadLevel("Room");
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];          
            
            if (info.RemovedFromList)
            {
                rooms.Remove(info.Name);
            }
            else
            {
                rooms.Add(info.Name);
                playerCount = roomList[i].PlayerCount;
            }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);

    }

    public void exit()
    {
        Application.Quit();
    }

}
