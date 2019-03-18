using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.02f;
    // Start is called before the first frame update
    private Vector3 nextPos;
    private bool isWaiting = false;
    private Vector3 deltaPos;
    private List<Vector3> listGoTo = new List<Vector3>();
    int currentIndex = 0;
    private GameObject player;
    public Vector2 locationRange;
    private bool isChaseActive = false;
    public GameObject cautionLine;
    public Sprite normalState;
    public Sprite crazyState;
    private List<GameObject> listCautionLine = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        nextPos = transform.position;
        deltaPos = new Vector3(0, 0, 0);
        locationRange = transform.position;
        DrawCautionLine();
    }
    public void ChangeLocationRange(float x, float y)
    {
        isWaiting = false;
        listGoTo.Clear();
        currentIndex = 0;
        foreach (GameObject g in listCautionLine)
            Destroy(g);
        listCautionLine.Clear();
        transform.position = new Vector3(x, y, 0);
        locationRange = transform.position;
        DrawCautionLine();
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
    public void DrawCautionLine()
    {
        for(int i = 0; i < 5; i++)
        {
           listCautionLine.Add(Instantiate(cautionLine, new Vector3(locationRange.x - 2 + i, locationRange.y - 2.5f, 0), Quaternion.identity));
           listCautionLine.Add(Instantiate(cautionLine, new Vector3(locationRange.x - 2 + i, locationRange.y + 2.5f, 0), Quaternion.identity));
           listCautionLine.Add(Instantiate(cautionLine, new Vector3(locationRange.x + 2.5f, locationRange.y - 2 + i, 0), Quaternion.Euler(new Vector3(0, 0, 90))));
           listCautionLine.Add(Instantiate(cautionLine, new Vector3(locationRange.x - 2.5f, locationRange.y - 2 + i, 0), Quaternion.Euler(new Vector3(0, 0, 90))));
        }
    }
    // Update is called once per frame
    int deltaTimeChasePlayer = 0;
    void SetRange(Vector2 location)
    {
        locationRange = location;
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime, Space.Self);
        if (Vector3.Distance(PlayerController.roundingVector(transform.position), nextPos) < 0.01)
        {
            isWaiting = false;
        }
        //If too far from range, turn back
        if (Vector3.Distance(locationRange, transform.position) > 7)
        {
            isChaseActive = false;
            GetComponent<SpriteRenderer>().sprite = normalState;
        }
        else
        {
            if (Vector3.Distance(player.transform.position, locationRange) < 2*Math.Sqrt(2) )
            {
                GetComponent<SpriteRenderer>().sprite = crazyState;
                isChaseActive = true;
                deltaTimeChasePlayer++;

                //Make enemy move smooth.
                var vector = new Vector3((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f, 0);
                if (deltaTimeChasePlayer > 30 && Vector3.Distance(vector, PlayerController.roundingVector(transform.position)) < 0.01)
                {
                    deltaTimeChasePlayer = 0;
                    transform.position = vector;
                    var path = GameController.FindWay(vector, PlayerController.roundingVector(player.GetComponent<PlayerController>().nextPos));
                    Go(path);
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = normalState;
                isChaseActive = false;
            }
        }

        //Player in range or too close enemy, chase him.


        if (!isWaiting)
        {
            if (currentIndex < listGoTo.Count)
                SetGoTo(listGoTo[currentIndex]);
            else if (!isChaseActive)
            {
                var vector = new Vector3((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f, 0);
                deltaTimeChasePlayer = 0;
                transform.position = vector;
                float x = locationRange.x + UnityEngine.Random.Range(-2, 3);
                float y = locationRange.y + UnityEngine.Random.Range(-2, 3);
                float row = GameController.row - 0.5f;
                x = x > row ? row : x;
                y = y > row ? row : y;
                x = x < 0.5f ? 0.5f : x;
                y = y < 0.5f ? 0.5f : y;

                //If cannot find way, find another
                var path = GameController.FindWay(vector, new Vector3(x, y, 0));
                while(Vector3.Distance(path[path.Count - 1], locationRange) > 8)
                {
                    path = GameController.FindWay(vector, new Vector3(x, y, 0));
                }
                Go(path);
            }
        }
        if (isWaiting)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, step);
        }
        transform.Rotate(Vector3.forward * 400 * Time.deltaTime, Space.Self);



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
}
