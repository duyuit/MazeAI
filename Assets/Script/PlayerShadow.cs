﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    // Start is called before the first frame update
    private int countDestroy = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDestroy++;
        if (countDestroy == 6)
            Destroy(gameObject);
    }
}
