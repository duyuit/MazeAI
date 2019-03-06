using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject demo = null;
    public int HideWallCount = 120;
    void Start()
    {
        int count = 0;
        int row = 15;
        if(demo!=null)
        {
            for(int i=0;i<row;i++)
            {
                for (int j = 0; j < row -1 ; j++)
                {

                    //Make sure have a path lead to exit or start door.
                    if (i == 1 && j == row -2)
                        continue;
                    if (i == row -2  && j == 0)
                        continue;

                    //Check if it boundary, do not delete this.
                    if (!(i == 0 || i == 14 || j == 0 || j == 13))
                    {
                        bool boolValue = (Random.Range(0, 2) == 0);
                        if (count < HideWallCount)
                        {
                            if (boolValue)
                                continue;
                            count++;
                        }
                    }

                    GameObject sample = Instantiate(demo);
                    LineRenderer lineRenderer = sample.GetComponent<LineRenderer>();
                    lineRenderer.SetPosition(0, new Vector3(j, i, 0));
                    lineRenderer.SetPosition(1, new Vector3(j + 1,i, 0));

                    GameObject sample1 = Instantiate(demo);
                    LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
                    lineRenderer1.SetPosition(0, new Vector3(i, j, 0));
                    lineRenderer1.SetPosition(1, new Vector3(i, j + 1, 0));
                }
            }

            
        }

    }
    
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
