using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class EnemyNormalController : MonoBehaviour
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
        if (Vector3.Distance(player.transform.position, transform.position) < 0.1)
        {
            GameController.isFail = true;
            isWaiting = false;
            listGoTo.Clear();
            currentIndex = 0;
            return;
        }
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
        if (lastTimeChase > 0.5f  && Vector3.Distance(vector, PlayerController.roundingVector(transform.position)) < 0.01)
        {
            var path = GameController.FindWay(vector, PlayerController.roundingVector(player.GetComponent<PlayerController>().nextPos));
            Go(path);
            lastTimeChase = 0;
        }

        if (isWaiting)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, step);
        }
    }
}
