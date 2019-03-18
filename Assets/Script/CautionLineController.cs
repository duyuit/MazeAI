using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionLineController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 0 || transform.position.x > GameController.row || transform.position.y < 0 || transform.position.y > GameController.row)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
