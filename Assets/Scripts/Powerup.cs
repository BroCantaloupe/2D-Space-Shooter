using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private int speed = 3;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    //0 = triple
    //1 = speed
    //2 = shield
    //3 = ammo
    //4 = health
    //5 = special fire

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
            AudioSource.PlayClipAtPoint(_clip, transform.position);
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
                    case 2:
                        player.ShieldGet();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.AddLife();
                        break;
                    case 5:
                        player.ToggleVoidBall();
                        break;
                    default: Debug.Log("Invalid Powerup");
                        break;

                }
            }
            Destroy(this.gameObject);
        }
    }

}


