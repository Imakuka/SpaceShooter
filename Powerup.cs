using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField] //0 = Triple Shot/ 1 = Speed/ 2 = Shield/ 
    private int powerupID;


    [SerializeField]
    private AudioClip _clip;

    

    // Update is called once per frame
    void Update()
    {      
        transform.Translate(Vector3.down  * _speed * Time.deltaTime);

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


            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            { 
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();                       
                        break;
                    case 1:
                        player.SpeedBoostActive();                
                        break;
                    case 2:
                        player.ShieldBoostActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
          
            }

            if (other.tag == "Shield Boost Powerup" + "Player Shield")
            {
                Destroy(this.gameObject);
            }
            Destroy(this.gameObject); 
        }

        
    }

}
