using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmbush : MonoBehaviour
{
    public float speed = 0.02f;
    // Start is called before the first frame update
    private Vector3 nextPos;
    private bool isWaiting = false;
    private Vector3 deltaPos;
    private List<Vector3> listGoTo = new List<Vector3>();
    int currentIndex = 0;
    private GameObject player;
    private float lastTimeChase = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.position = new Vector3(UnityEngine.Random.Range(0, 14) + 0.5f, UnityEngine.Random.Range(10, 14) + 0.5f, 0);
        nextPos = transform.position;
        deltaPos = new Vector3(0, 0, 0);
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
    public void Go(List<Vector2> listPath)
    {
        nextPos = PlayerController.roundingVector(transform.position);
        listGoTo.Clear();
        foreach (Vector2 pos in listPath)
        {
            listGoTo.Add(pos);
        }
        currentIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(player.transform.position, transform.position) < 0.1)
        //{
        //    GameController.isFail = true;
        //    isWaiting = false;
        //    listGoTo.Clear();
        //    currentIndex = 0;
        //    return;
        //}
        if (Vector3.Distance(PlayerController.roundingVector(transform.position), nextPos) < 0.01)
        {
            isWaiting = false;
        }
        if (!isWaiting)
        {
            if (currentIndex < listGoTo.Count)
                SetGoTo(listGoTo[currentIndex]);
        }

        lastTimeChase += Time.deltaTime;
        var vector = new Vector3((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f, 0);
        if (lastTimeChase > 0.5f 
            && Vector3.Distance(player.transform.position,transform.position) < 4
            && Vector3.Distance(vector, PlayerController.roundingVector(transform.position)) < 0.01)
        {
            lastTimeChase = 0;
            var playerPath = GameController.FindWay(player.GetComponent<PlayerController>().nextPos, new Vector3(14.5f,14.5f,0));
            if (playerPath.Count > 5)
            {
                //for (int i = 3; i < playerPath.Count - 2; i++)
                //{
                //    if (playerPath[i].x != playerPath[i + 2].x && playerPath[i].y != playerPath[i + 2].y)
                //    {
                //        var path = GameController.FindWay(vector, playerPath[i+1]);
                //        Go(path);
                //        break;
                //    }
                //}
                var path = GameController.FindWay(vector, playerPath[3]);
                Go(path);
            }
         
        }

        if (isWaiting)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, step);
        }
    }
}
