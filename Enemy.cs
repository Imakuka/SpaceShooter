﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 3.0f;


    private Player _player;
    private Animator _anim;
    [SerializeField]
    private DeathRocket _deathRocket;
  

    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _deathRocket = GameObject.Find("Player").GetComponent<DeathRocket>();
        _deathRocket = _player.gameObject.GetComponent<DeathRocket>();

                 
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is Null");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }


        

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = UnityEngine.Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject newLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = new Laser[2];
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i] = newLaser.GetComponentInChildren<Laser>();
                lasers[i].EnemyLaser();
            }

        }
    }



    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.1f)
        {
            float randomX = UnityEngine.Random.Range(-10.0f, 10.1f);
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
                player.Damage();
            }
            EnemyDestruction();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(UnityEngine.Random.Range(5, 13));
            }
            EnemyDestruction();
            Destroy(GetComponent<Collider2D>());
        }

        if (other.tag == "DeathRocket")
        {
            Destroy(other.gameObject);
            if (_player == null)
            {
                _player.AddScore(UnityEngine.Random.Range(10, 18));
            }
            EnemyDestruction();
            Destroy(GetComponent<Collider2D>());

        }
    }
    public void EnemyDestruction()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();
        Destroy(this.gameObject, 1.5f);

    }
}
