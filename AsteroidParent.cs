using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidParent : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f;


    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 7f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -10)
        {
            transform.position = new Vector3(transform.position.x, 7, 0);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }

        Player player = other.transform.GetComponent<Player>();

        if (player != null)
        {
            player.Damage();
        }


    }

    
}