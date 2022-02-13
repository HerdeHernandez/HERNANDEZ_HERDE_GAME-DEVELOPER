using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
public class PlayerMovement : MonoBehaviour
{
    public string Me;
    public int Score;

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
    float speed = .011f;
    void Start()
    {
        GameObject[] Objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (var obj in Objects)
        { 
            if (obj.layer == LayerMask.NameToLayer("Objects"))
                Physics.IgnoreCollision(obj.GetComponent<Collider>(), meshCollider, true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, colliderGround, true);
        Physics.IgnoreCollision(other, meshCollider, false);

        other.tag = this.name;
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, colliderGround, false);
        Physics.IgnoreCollision(other, meshCollider, true);

        other.tag = this.name;
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

    void OnMouseDrag()
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

        camera.transform.position = new Vector3(this.transform.position.x, camera.transform.position.y, this.transform.position.z);

        playerPos = pos;
    }

    public void Resize()
    {
        Score++;

        if (Score % 10 == 0)
        {
            speed *= 1.1f;
            StartCoroutine(Scale());
            print("asd");
        }
    }


    public IEnumerator Scale()
    {
        Vector3 start = this.transform.localScale;
        Vector3 End = start * 2;

        float time = 0;

        while (time <= .4f)
        {
            time += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(start, End, time);
            yield return null;
        }
    }
}
