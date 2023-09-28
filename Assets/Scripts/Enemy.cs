using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Animator _explosionAnim;
    private float _speed = 4f;
    private bool _explosionSequence;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private bool _isDead;
    private SpawnManager _spawnManager;

    private float _fireRate = 3f;
    private float _canFire = -1;

    private void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        _explosionAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_explosionAnim == null)
        {
            Debug.LogError("Animator is NULL");
        }
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("powerup is NULL");
        }
    }
    void Update()
    {
        CalculateMovement();
    
        if(Time.time > _canFire && _isDead == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            LaserBehavior[] lasers = enemyLaser.GetComponentsInChildren<LaserBehavior>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            transform.position = (new Vector3(Random.Range(-8f, 8f), 8, 0));
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _speed = 0;
            _audioSource.Play();
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(gameObject, 1.1f);
            _isDead = true;
            
            if(_player != null)
            {
                _player.Damage();
            }
        }
        else if(other.gameObject.CompareTag("Laser") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _audioSource.Play();
            _speed = 0;
            Object.Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(this.gameObject, 1.1f);
            _isDead = true;
            int powerupChance = Random.Range(0, 4);
            if(powerupChance == 0)
            {
                StartCoroutine(EnemyPowerup());
            }
        }
        
    }

    IEnumerator EnemyPowerup()
    {
        yield return new WaitForSeconds(0.5f);
        _spawnManager.NewEnemyPowerup(transform.position);
    }
    
}
