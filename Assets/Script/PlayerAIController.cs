using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class PlayerAIController : MonoBehaviour
{
    public float speed = 5;
    public GameObject enemy;
    public GameController gameController;
    public GameObject playerShadow;
    public Vector3 nextPos;
    private bool isWaiting = false;
    private Vector3 deltaPos;
    private List<Vector3> listGoTo = new List<Vector3>();
    public List<NodeConnection> listConnected = new List<NodeConnection>();
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        nextPos = transform.position;
        deltaPos = new Vector3(0, 0, 0);

        foreach (Vector2 pos in gameController.listPath)
        {
            listGoTo.Add(pos);
        }
    }

    public void Restart()
    {
        transform.position = new Vector3(0.5f, 0.5f, 0);
        nextPos = transform.position;
    }
    void Awake()
    {

    }
    void FixedUpdate()
    {


    }
    public void Go(List<Vector2> listPath)
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
    }
    // Update is called once per frame
    float deltaOpacity = 0;
    bool isRunAway = false;
    bool isWaitEnemyCloser = false;
    //public void MoveByButton(int i)
    //{
    //    switch (i)
    //    {
    //        case 1:
    //            {
    //                Vector3 temp = roundingVector(transform.position) + new Vector3(-1, 0, 0);
    //                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
    //                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
    //                {
    //                    nextPos = temp;
    //                    deltaPos = new Vector3(-1, 0, 0);
    //                    isWaiting = true;
    //                }
    //            }
    //            break;
    //        case 2:
    //            {
    //                Vector3 temp = roundingVector(transform.position) + new Vector3(0, -1, 0);
    //                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
    //                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
    //                {
    //                    nextPos = temp;
    //                    deltaPos = new Vector3(0, -1, 0);
    //                    isWaiting = true;
    //                }
    //            }
    //            break;
    //        case 3:
    //            {
    //                Vector3 temp = roundingVector(transform.position) + new Vector3(1, 0, 0);
    //                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
    //                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
    //                {
    //                    nextPos = temp;
    //                    deltaPos = new Vector3(1, 0, 0);
    //                    isWaiting = true;
    //                }
    //            }
    //            break;
    //        case 4:
    //            {
    //                Vector3 temp = roundingVector(transform.position) + new Vector3(0, 1, 0);
    //                NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
    //                if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
    //                {
    //                    nextPos = temp;
    //                    deltaPos = new Vector3(0, 1, 0);
    //                    isWaiting = true;
    //                }
    //            }
    //            break;
    //    }
    //}
    void Update()
    {

        if (Vector3.Distance(transform.position, new Vector3(14.5f, 14.5f, 0)) < 0.01)
        {
            isWin = true;
            return; 
        }
        if (Vector3.Distance(PlayerController.roundingVector(transform.position), nextPos) == 0)
        {
            isWaiting = false;
            deltaOpacity = 0.15f;
        }
        if (!isWaiting)
        {
            if (currentIndex < listGoTo.Count)
            {

                bool canGO = true;
                int max = currentIndex + 6 < listGoTo.Count ? currentIndex + 6 : listGoTo.Count;
                for(int i = currentIndex;i< max; i++)
                {
                    if (Vector3.Distance(listGoTo[i], enemy.transform.position) < 1.5f)
                    {
                        var vector = new Vector3((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f, 0);
                        Go(GameController.FindWay(vector, vector + new Vector3(-3,3)));
                        canGO = false;
                        isRunAway = true;
                        break;
                    }
                }
                if (canGO)
                {
                    SetGoTo(listGoTo[currentIndex]);
                }

            }
            else
            {
                if (isRunAway)
                {
                    isRunAway = false;
                    var vector = new Vector3((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f, 0);
                    Go(GameController.FindWay(vector, new Vector3(14.5f, 14.5f, 0)));
                }
            }
           

            //    if (Input.GetKey(KeyCode.UpArrow) || vertiJoyStick.Direction.y > 0)
            //    {
            //        Vector3 temp = roundingVector(transform.position) + new Vector3(0, 1, 0);
            //        NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
            //        if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
            //        {
            //            nextPos = temp;
            //            deltaPos = new Vector3(0, 1, 0);
            //            isWaiting = true;
            //        }
            //    }
            //    else if (Input.GetKey(KeyCode.DownArrow) || vertiJoyStick.Direction.y < 0)
            //    {
            //        Vector3 temp = roundingVector(transform.position) + new Vector3(0, -1, 0);
            //        NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
            //        if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
            //        {
            //            nextPos = temp;
            //            deltaPos = new Vector3(0, -1, 0);
            //            isWaiting = true;
            //        }
            //    }
            //    else if (Input.GetKey(KeyCode.LeftArrow) || horiJoyStick.Direction.x < 0)
            //    {
            //        Vector3 temp = roundingVector(transform.position) + new Vector3(-1, 0, 0);
            //        NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
            //        if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
            //        {
            //            nextPos = temp;
            //            deltaPos = new Vector3(-1, 0, 0);
            //            isWaiting = true;
            //        }
            //    }
            //    else if (Input.GetKey(KeyCode.RightArrow) || horiJoyStick.Direction.x > 0)
            //    {
            //        Vector3 temp = roundingVector(transform.position) + new Vector3(1, 0, 0);
            //        NodeConnection node = new NodeConnection(roundingVector(transform.position), temp);
            //        if (listConnected.FirstOrDefault(l => l.isEqual(node)) != null)
            //        {
            //            nextPos = temp;
            //            deltaPos = new Vector3(1, 0, 0);
            //            isWaiting = true;
            //        }

            //    }
            //    else
            //    {
            //        if (!isWaiting)
            //            deltaPos = new Vector3(0, 0, 0);
            //    }

        }
        if (isWaiting)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, step);
            GameObject shadowClone = Instantiate(playerShadow, transform.position, Quaternion.identity);
            Color currenColor = shadowClone.GetComponent<Renderer>().material.color;
            shadowClone.GetComponent<Renderer>().material.color = new Color(currenColor.r, currenColor.g, currenColor.b, deltaOpacity);
            if (deltaOpacity < 0.6f)
                deltaOpacity += 0.02f;
        }
    }
}
