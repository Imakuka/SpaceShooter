using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRocketPowerup : MonoBehaviour
{
    private float _speed = 2.0f;

    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.1f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit" + other.transform.tag);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            player.DeathRocketPowerUpOn();
            _uiManager.DeathRocketText();       
        }
        Destroy(this.gameObject);
    }
}
