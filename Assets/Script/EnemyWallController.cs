using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallController : MonoBehaviour
{
    public float ThrowTime = 1;
    private GameObject player;
    public GameObject wallFly;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

       // SetVelocityToJump(new Vector3(14.5f, 0.5f, 0),2);
    }
    float lastThrowWall = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 400 * Time.deltaTime, Space.Self);
        lastThrowWall += Time.deltaTime;
        if(lastThrowWall > ThrowTime)
        {
            lastThrowWall = 0;
            var bestPath = GameController.FindWay(player.GetComponent<PlayerController>().nextPos, new Vector3(14.5f, 14.5f, 0));

            if (bestPath.Count < 6)
                return;
            var deltaPosition = bestPath[5] - bestPath[4];

         
            GameObject fly = Instantiate(wallFly, transform.position, Quaternion.identity);
            if (deltaPosition.x!= 0) //Hori
            {
                var vector = bestPath[4] + new Vector2(0.5f, 0);
                fly.GetComponent<WallFlyController>().SetVelocityToJump(vector, true);
            }
            else if(deltaPosition.y!=0) //Verti
            {
                var vector = bestPath[4] + new Vector2(0,0.5f);
                fly.GetComponent<WallFlyController>().SetVelocityToJump(vector, false);
            }
        }
    }
  
}
