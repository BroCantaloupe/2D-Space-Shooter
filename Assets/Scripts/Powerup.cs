using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private int speed = 3;
    [SerializeField]
    private int powerupID;
    //0 = triple
    //1 = speed
    //2 = shield

    private void Start()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("player is NULL");
        }
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (player != null)
            {
                switch(powerupID)
                {
                    case 0: player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2: Debug.Log("Shield pwerup");
                        break;
                    default: Debug.Log("Invalid Powerup");
                        break;

                }
            }
            Destroy(this.gameObject);
        }
    }
}


