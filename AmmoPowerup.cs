using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerup : MonoBehaviour
{
    private float _speed = 2.0f;
    private Player _player;


    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();

        if (_player != null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            
        }
        else
        {
            Debug.LogError("Player is Null");
        }
        
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.1f)
        {
            float randomX = Random.Range(-10.0f, 10.1f);
            transform.position = new Vector3(randomX, 6.1f, 0);

        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit" + other.transform.tag);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.ReloadPlayerAmmo();
            }
            Destroy(this.gameObject);
        }
    }
    

}
