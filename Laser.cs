using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyLaser;
    private void OnEnable()
    {
        Player.onFireLaser += PlayerLaser;
        
    }
    // Update is called once per frame
    void Update()
    {


        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }

    private void PlayerLaser(bool canFire, GameObject laser)
    {
        if (canFire == true && gameObject == laser)
        {
            _isEnemyLaser = false;
            
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit" + other.transform.tag);
        Player player = other.transform.GetComponent<Player>();

        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            if (player != null)
            {
                player.Damage();
            }
        }

    }


    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }

    void MoveDown()
    {
        Debug.Log("move Down");
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void EnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnDisable()
    {
        Player.onFireLaser -= PlayerLaser;
    }

}
