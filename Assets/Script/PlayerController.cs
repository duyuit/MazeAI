using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public float speed = 0.02f;

    public GameController gameController;
    public GameObject playerShadow;
    public Vector3 nextPos;
    private bool isWaiting = false;
    private Vector3 deltaPos;
    private List<Vector3> listGoTo;
    public List<NodeConnection> listConnected = new List<NodeConnection>();
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        nextPos = transform.position;
        listGoTo = new List<Vector3>();
        deltaPos = new Vector3(0,0,0);

        foreach (Vector2 pos in gameController.listPath)
        {
            listGoTo.Add(pos);
        }
    }
    public void ResetMap(List<Vector2> listPath)
    {
        if (listGoTo != null)
        {
            listGoTo.Clear();
            foreach (Vector2 pos in listPath)
            {
                listGoTo.Add(pos);
            }
            currentIndex = 0;
        }
    }
    void Awake()
    {
     
    }
    void FixedUpdate()
    {
      

    }
   
    void SetGoTo(Vector3 destination)
    {
        currentIndex++;
        nextPos = destination;
        isWaiting = true;

        double x = destination.x - transform.position.x;
        x = Math.Round(x, 2);
        double y = destination.y - transform.position.y;
        y = Math.Round(y, 2);

        deltaPos.x = (float)x;
        deltaPos.y = (float)y;

        //if (deltaPos.x > 0)
        //{
        //    animator.SetFloat("X", 1);
        //    animator.SetFloat("Y", 0);
        //    animator.SetInteger("State", 1);
        //}

        //if (deltaPos.x < 0)
        //{
        //    animator.SetFloat("X", -1);
        //    animator.SetFloat("Y", 0);
        //    animator.SetInteger("State", 1);
        //}

        //if (deltaPos.y > 0)
        //{
        //    animator.SetFloat("X", 0);
        //    animator.SetFloat("Y", 1);
        //    animator.SetInteger("State", 1);
        //}

        //if (deltaPos.y < 0)
        //{
        //    animator.SetFloat("X", 0);
        //    animator.SetFloat("Y", -1);
        //    animator.SetInteger("State", 1);
        //}

    }
    // Update is called once per frame
    bool isFloatEqual(float a, float b)
    {
        double x = a;
        x = Math.Round(x, 2);
        double y = b;
        y = Math.Round(y, 2);
        return a == b;
    }
    public static Vector3 roundingVector(Vector3 a)
    {
        double x = a.x;
        x = Math.Round(x, 2);
        double y = a.y;
        y = Math.Round(y, 2);

        a.x = (float) x;
        a.y = (float) y;
        return a;
    }

    float deltaOpacity = 0;
    public void MoveByButton(int i)
    {
        switch(i)
        {
            case 1:
                {
                    Vector3 temp = roundingVector(transform.position) + new Vector3(-1, 0, 0);
                    NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                    if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                    {
                        nextPos = temp;
                        deltaPos = new Vector3(-1, 0, 0);
                        isWaiting = true;
                    }
                }
                break;
            case 2:
                {
                    Vector3 temp = roundingVector(transform.position) + new Vector3(0, -1, 0);
                    NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                    if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                    {
                        nextPos = temp;
                        deltaPos = new Vector3(0, -1, 0);
                        isWaiting = true;
                    }
                }
                break;
            case 3:
                {
                    Vector3 temp = roundingVector(transform.position) + new Vector3(1, 0, 0);
                    NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                    if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                    {
                        nextPos = temp;
                        deltaPos = new Vector3(1, 0, 0);
                        isWaiting = true;
                    }
                }
                break;
            case 4:
                {
                    Vector3 temp = roundingVector(transform.position) + new Vector3(0, 1, 0);
                    NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                    if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                    {
                        nextPos = temp;
                        deltaPos = new Vector3(0, 1, 0);
                        isWaiting = true;
                    }
                }
                break;
        }
    }
    void Update()
    {
        if (Vector3.Distance(roundingVector(transform.position), nextPos) == 0)
        {

            isWaiting = false;
            deltaOpacity = 0.15f;
        }
        if (!isWaiting)
        {
            //if (currentIndex < listGoTo.Count)
            //    SetGoTo(listGoTo[currentIndex]);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 temp = roundingVector(transform.position) + new Vector3(0, 1, 0);
                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                {
                    nextPos = temp;
                    deltaPos = new Vector3(0,1, 0);
                    isWaiting = true;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 temp = roundingVector(transform.position) + new Vector3(0, -1, 0);
                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                {
                    nextPos = temp;
                    deltaPos = new Vector3(0, -1, 0);
                    isWaiting = true;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 temp = roundingVector(transform.position) + new Vector3 (-1, 0, 0);
                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                {
                    nextPos = temp;
                    deltaPos = new Vector3(-1, 0, 0);
                    isWaiting = true;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 temp = roundingVector(transform.position) + new Vector3(1, 0, 0);
                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
                {
                    nextPos = temp;
                    deltaPos = new Vector3(1, 0, 0);
                    isWaiting = true;
                }
     
            }
            else
            {
               // animator.SetInteger("State", 0);
                if (!isWaiting)
                    deltaPos = new Vector3(0, 0, 0);  
            }
        }
        if (isWaiting)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, step);
            GameObject shadowClone = Instantiate(playerShadow, transform.position, Quaternion.identity);
            Color currenColor = shadowClone.GetComponent<Renderer>().material.color;
            shadowClone.GetComponent<Renderer>().material.color = new Color(currenColor.r, currenColor.g, currenColor.b, deltaOpacity);
            if(deltaOpacity < 0.6f)
            deltaOpacity += 0.02f;
        }
       

    }
}
