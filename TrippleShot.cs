﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippleShot : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 7)
        {
            Destroy(this.gameObject);
        }
        
    }
}