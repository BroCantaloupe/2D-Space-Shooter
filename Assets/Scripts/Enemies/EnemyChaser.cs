using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private Object _explosionPrefab;
    private float _speed = 5f;
    private bool _explosionSequence;
    private SpawnManager _spawnManager;
    private Transform _playerTransform;
    
    private float _rotateSpeed = 3f;
    private float _angleModifier = 270;
    private bool _isShieldActive;
    [SerializeField]
    private GameObject _shieldVisualizer;

    private void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        _playerTransform = GameObject.Find("Player").transform.GetComponent<Transform>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_playerTransform == null)
        {
            Debug.LogError("Player Transform is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("powerup is NULL");
        }
        int shieldChance = Random.Range(0, 11);
        if (shieldChance == 0)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);

        }
    }
    void Update()
    {
        CalculateMovement();
        FacingPlayer();
        
    }

    void FacingPlayer()
    {
        Vector3 vectorToTarget = _playerTransform.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _angleModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotateSpeed);
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Laser") && _isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            if(_player != null)
            {
                _player.Damage();
                _player.StartInvincibility();
            }
        }
        else if (other.gameObject.CompareTag("Player") && _explosionSequence == false)
        {
            _spawnManager.EnemyCountMinus();

            _explosionSequence = true;
            _speed = 0;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.4f);

            if (_player != null)
            {
                _player.Damage();
                _player.StartInvincibility();
            }
        }
        else if (other.gameObject.CompareTag("Laser") && _explosionSequence == false)
        {
            _spawnManager.EnemyCountMinus();

            _explosionSequence = true;
            _speed = 0;
            Object.Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.4f);
            int powerupChance = Random.Range(0, 4);
            if (powerupChance == 0)
            {
                StartCoroutine(EnemyPowerup());
            }
        }
        else if (other.gameObject.CompareTag("Void Ball") && _explosionSequence == false)
        {
            _spawnManager.EnemyCountMinus();

            _explosionSequence = true;
            _speed = 0;
            if (_player != null)
            {
                _player.AddScore(20);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.4f);
            int powerupChance = Random.Range(0, 6);
            if (powerupChance == 0)
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
