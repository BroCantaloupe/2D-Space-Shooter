using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private bool _isDisabled;
    private Transform _playerTransform;
    private bool _isMovingTowardPlayer;
    //0 = triple
    //1 = speed
    //2 = shield
    //3 = ammo
    //4 = health
    //5 = special fire

    //6 = slow
    //7 = teleport
    //8 = blindness

    private void Start()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        if(_playerTransform == null)
        {
            Debug.LogError("Player Transform NULL");
        }
        if (player == null)
        {
            Debug.LogError("player is NULL");
        }
        if(powerupID > 5)
        {
            _speed = 1.8f;
        }
    }

    void Update()
    {
        if (_isMovingTowardPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, ((2 * _speed) * Time.deltaTime));
        }
        else
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isMovingTowardPlayer = true;
        }
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isDisabled == false)
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
                    case 6:
                        player.SlowActive();
                        break;
                    case 7:
                        player.Teleport();
                        break;
                    case 8:
                        player.BlindnessActive();
                        break;
                    default: Debug.Log("Invalid Powerup");
                        break;

                }
            }
            Destroy(this.gameObject);
        }
        else if(other.CompareTag("Player") && _isDisabled)
        {
            Destroy(this.gameObject);
        }
        if (other.CompareTag("SmartMovement"))
        {
            _isDisabled = true;
            SpriteRenderer powerupSprite = GetComponent<SpriteRenderer>();
            powerupSprite.color = Color.blue;
            if(powerupSprite == null)
            {
                return;
            }
        }
    }

}


