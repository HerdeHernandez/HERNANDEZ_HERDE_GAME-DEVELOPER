using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

[RequireComponent(typeof(MeshCollider))]

public class PlayerMovement : MonoBehaviour, IPunObservable
{
    public string Me, ID;
    public int Score;

    public Text nameText;
    Transform scoreBoard;

    public PolygonCollider2D polygonCollider;
    public PolygonCollider2D groundCollider;
    public MeshCollider meshCollider;
    public Camera camera;

    public Vector3 playerPos;

    Mesh mesh;

    public Collider colliderGround;

    private Vector3 screenPoint;
    private Vector3 offset;    

    public float scale = 0.5f;
    float speed = .03f;

    public bool mouseHold = false;

    PhotonView view;

    void Awake()
    {
        //  polygonCollider = GameObject.Find("Hole2D").GetComponent<PolygonCollider2D>();


        GameObject colliderParent = GameObject.Find("Collider");

        groundCollider = colliderParent.transform.GetChild(0).GetComponent<PolygonCollider2D>();

        meshCollider = colliderParent.transform.GetChild(1).GetComponent<MeshCollider>();

        colliderGround = GameObject.Find("Quad").GetComponent<Collider>();
        // camera = GameObject.Find("Camera").GetComponent<Camera>();      
    }
    void Start()
    {
        Transform objParent = GameObject.Find("Objects").transform;

        foreach (Transform obj in objParent)
        { 
            if (obj.gameObject.layer == LayerMask.NameToLayer("Objects"))
                Physics.IgnoreCollision(obj.GetComponent<Collider>(), meshCollider, true);
        }

        view = this.GetComponent<PhotonView>();
        scoreBoard = GameObject.Find("Content").transform;

        this.transform.GetChild(1).GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, colliderGround, true);
        Physics.IgnoreCollision(other, meshCollider, false);

        other.GetComponent<objectInfo>().eatenBy = this.GetComponent<PhotonView>();
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, colliderGround, false);
        Physics.IgnoreCollision(other, meshCollider, true);

        other.GetComponent<objectInfo>().eatenBy = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.hasChanged == true)
        {
            this.transform.hasChanged = false;
            polygonCollider.transform.position = new Vector2(this.transform.position.x, this.transform.position.z);
            polygonCollider.transform.localScale = this.transform.localScale * scale;
            Hole();
            meshCollider3d();   
        }       
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (camera.gameObject.activeInHierarchy == false)
                camera.gameObject.SetActive(true);
                nameText.gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                mouseHold = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseHold = false;
            }

            if (mouseHold == true)
            {
                movement();
            }
        }

      
    }

    private void Hole()
    {
        Vector2[] PointPos = polygonCollider.GetPath(0);

        for (int i = 0; i < PointPos.Length; i++)
        {
            PointPos[i] = polygonCollider.transform.TransformPoint(PointPos[i]);
        }

        groundCollider.pathCount = 2;
        groundCollider.SetPath(1, PointPos);
    }

    private void meshCollider3d()
    {
        if (mesh != null)
            Destroy(mesh);

        mesh = groundCollider.CreateMesh(true, true);
        meshCollider.sharedMesh = mesh;
    }

    void movement()
    {           
        Vector3 p = Input.mousePosition;
        p.z = camera.transform.position.y;
        Vector3 pos = camera.ScreenToWorldPoint(p);


        if (pos.x >= this.transform.position.x)
        {
            this.transform.position = new Vector3(this.transform.position.x + speed, this.transform.position.y, this.transform.position.z);
            ////print("r");
        }
        if (pos.x <= this.transform.position.x)
        {
            this.transform.position = new Vector3(this.transform.position.x - speed, this.transform.position.y, this.transform.position.z);
            //print("l");
        }
        if (pos.z >= this.transform.position.z)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + speed);
            //print("t");
        }
        if (pos.z <= this.transform.position.z)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - speed);
            //print("d");
        }

      //  camera.transform.position = new Vector3(this.transform.position.x, camera.transform.position.y, this.transform.position.z);

        playerPos = pos;
    }

    public void Resize()
    {
        if (this.transform.localScale.x < 5)
        {
            if (Score % 10 == 0)
            {
                speed *= 1.1f;
                StartCoroutine(Scale());

            }
        }      
    }


    public IEnumerator Scale()
    {
        Vector3 start = this.transform.localScale;
        Vector3 End = start * 2;

        float time = 0;
       
        while (time <= .15f)
        {           
            time += Time.deltaTime;
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, camera.transform.localPosition / 1.028f, time);
            this.transform.localScale = Vector3.Lerp(start, End, time);
            
            yield return null;
          //  camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y / 0.0031f, camera.transform.localPosition.z);
        }
       // yield return new WaitForSeconds(.4f);
    }

    [PunRPC]
    void setName(string name, string ID)
    {
        Me = name;
        this.ID = ID;

        nameText.text = name;

        scoreBoard = GameObject.Find("Content").transform;

        var NameScore = Resources.Load("NameScore");

        var NameScoreDetail = Instantiate(NameScore, scoreBoard) as GameObject;
        NameScoreDetail.name = ID;
        NameScoreDetail.transform.GetChild(0).GetComponent<Text>().text = Me;
        NameScoreDetail.transform.GetChild(1).GetComponent<Text>().text = Score.ToString();
    }

    [PunRPC]
    void setScore()
    {
        Score++;

        Resize();

        scoreBoard = GameObject.Find("Content").transform;

        foreach (Transform child in scoreBoard)
        {
            if(child.name == this.ID)
                child.GetChild(1).GetComponent<Text>().text = Score.ToString();
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Me);
            stream.SendNext(Score);
        }

        else if (stream.IsReading)
        {
            Me = (string)stream.ReceiveNext();
            Score = (int)stream.ReceiveNext();
        }
            
    }

    public void exit()
    {
        StartCoroutine(exitGame());
    }

    IEnumerator exitGame()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("Start");
    }
}
