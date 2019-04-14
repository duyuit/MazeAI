using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float deltaTime = 0;
    bool isLeft = false;
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > 0.5f)
        {
            deltaTime = 0;
            isLeft = !isLeft;
        }
        if (isLeft)
            transform.Rotate(Vector3.forward * 200 * Time.deltaTime, Space.Self);
        else
            transform.Rotate(Vector3.back * 200 * Time.deltaTime, Space.Self);
    }
}
