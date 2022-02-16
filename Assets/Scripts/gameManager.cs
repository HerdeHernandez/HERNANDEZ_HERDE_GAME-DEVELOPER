using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour 
{ 
    public List<string> players = new List<string>();
    public GameObject[] gos;
    Transform scoreBoard;
    string id;

    public int targetFrameRate = 60;

    void Start()
    {
        scoreBoard = GameObject.Find("Content").transform;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    void Update()
    {
        players.Clear();
        gos = GameObject.FindGameObjectsWithTag("Player");

        List<playerInfo> ranking = new List<playerInfo>();
        
        foreach (GameObject go in gos)
        {
            if (!players.Contains(go.GetComponent<PlayerMovement>().ID))
            {
                players.Add(go.GetComponent<PlayerMovement>().ID);

                playerInfo player = new playerInfo()
                {
                    name = go.GetComponent<PlayerMovement>().Me,
                    id = go.GetComponent<PlayerMovement>().ID,
                    score = go.GetComponent<PlayerMovement>().Score,

                };

                ranking.Add(player);
            }               
        }

        foreach (Transform child in scoreBoard)
        {
            if (child.name != "NameScore")
                Destroy(child.gameObject);
        }

        ranking.Sort((y, x) => x.score.CompareTo(y.score));

        foreach (playerInfo p in ranking)
        {
            var NameScore = Resources.Load("NameScore");

            var NameScoreDetail = Instantiate(NameScore, scoreBoard) as GameObject;
            NameScoreDetail.name = p.id;
            NameScoreDetail.transform.GetChild(0).GetComponent<Text>().text = p.name;
            NameScoreDetail.transform.GetChild(1).GetComponent<Text>().text = p.score.ToString();
        }

        foreach (Transform child in scoreBoard)
        {
            if (!players.Contains(child.name) && child.name != "NameScore")
                Destroy(child.gameObject);
        }

        
    } 
}

public class playerInfo
{
    public string name;
    public string id;
    public int score;
}


