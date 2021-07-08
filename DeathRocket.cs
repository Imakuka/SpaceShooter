using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DeathRocket : MonoBehaviour
{
    [SerializeField]
    private float _rocketSpeed = 1000.0f;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _rotateSpeed = 1500.0f;
    [SerializeField]
    Rigidbody2D rb;


    // Start is called before the first frame update
    void OnEnable()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        rb.velocity = transform.up * _rocketSpeed * Time.deltaTime;
        Vector3 targetVector = _target.position - transform.position;
        float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;
        rb.angularVelocity = -2 * rotatingIndex * _rotateSpeed * Time.deltaTime;

        if (transform.position.y >= 7.0f)
        {
            float randomX = Random.Range(-10.0f, 10.1f);
            transform.position = new Vector3(randomX, -6.1f, 0);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit" + other.transform.tag);

        if (other.tag == "Enemy")
        {
           Enemy enemy = other.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.EnemyDestruction();
            }
            Destroy(this.gameObject);
        }

    }

}
