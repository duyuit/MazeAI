using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    public float speed = 0.02f;
    public GameObject lineConnection = null;

    private Vector3 nextPos;
    private bool isWaiting = false;
    private Vector3 deltaPos;
    private List<Vector3> listGoTo = new List<Vector3>();
    private List<GameObject> listConnectDrawed = new List<GameObject>();
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
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
    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(PlayerController.roundingVector(transform.position), nextPos) == 0)
        {
            isWaiting = false;
        }
        if (!isWaiting)
        {
            if (currentIndex < listGoTo.Count)
                SetGoTo(listGoTo[currentIndex]);
            else
            {
                foreach (var line in listConnectDrawed)
                    Destroy(line);
                listConnectDrawed.Clear();
                Destroy(gameObject);
            }
        }

        if (isWaiting)
        {
            DrawConnectionLine(transform.position, transform.position + deltaPos * speed);
            transform.position = transform.position + deltaPos * speed;
        }
      
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
    void DrawConnectionLine(Vector3 start, Vector3 end)
    {
        GameObject sample1 = Instantiate(lineConnection);
        LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
        lineRenderer1.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        lineRenderer1.SetPosition(0, start);
        lineRenderer1.SetPosition(1, end);
        listConnectDrawed.Add(sample1);
    }
}
