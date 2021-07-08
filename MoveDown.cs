using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private float _speed = 3.0f;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.1f)
        {
            float randomX = Random.Range(-11.6f, 11.8f);
            transform.position = new Vector3(randomX, 6.1f, 0);

        }

    }
}
