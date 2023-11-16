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
    private bool _isShieldActive;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _smartMovementPrefab;

    private float _fireRate = 3f;
    private float _canFire = -1;
    private bool _isRamActive;

    private float _acceleration = -8f;
    private bool _isSlowdownActive;
    private bool _isAimActive;
    private bool _isLaunchActive;
    private float _launchSpeed = 18f;
    private bool _isDodgeLeft;
    private bool _isDodgeRight;

    private Transform _playerTransform;
    private float _rotateSpeed = 20f;
    private float _angleModifier = 270;

    private void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        _playerTransform = GameObject.Find("Player").transform.GetComponent<Transform>();
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
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("powerup is NULL");
        }
        GameObject enemyMovement = Instantiate(_smartMovementPrefab, transform.position, Quaternion.identity);
        enemyMovement.transform.parent = this.transform;
        int shieldChance = Random.Range(0, 7);
        if(shieldChance == 0)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);

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
        if(_isRamActive)
        {
            FacingPlayer();
        }
        if(_isSlowdownActive)
        {
            _speed += _acceleration * Time.deltaTime;
            if(_speed <= 0)
            {
                _speed = 0;
            }
        }
        if(_isAimActive)
        {
            FacingPlayer();
            _angleModifier = 90;
        }
        if (_isLaunchActive)
        {
            transform.Translate(Vector3.down * _launchSpeed * Time.deltaTime);
            _thruster.SetActive(true);
        }
        if (_isDodgeLeft)
        {
            transform.Translate(Vector3.left * _launchSpeed * Time.deltaTime);
        }
        if (_isDodgeRight)
        {
            transform.Translate(Vector3.right * _launchSpeed * Time.deltaTime);
        }
    }

    private void FacingPlayer()
    {
        Vector3 vectorToTarget = _playerTransform.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _angleModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotateSpeed);
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            transform.position = (new Vector3(Random.Range(-8f, 8f), 8, 0));
            _speed = 4f;
            _thruster.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _speed = 0;
            _audioSource.Play();
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(gameObject, 1.1f);
            _isDead = true;

            if (_player != null)
            {
                _player.Damage();
                _player.StartInvincibility();
            }
            _spawnManager.EnemyCountMinus();
        }
        else if (other.gameObject.CompareTag("Laser") && _explosionSequence == false)
        {
            if (_isShieldActive)
            {
                Object.Destroy(other.gameObject);
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
            }
            else
            {
                Object.Destroy(other.gameObject);

                _explosionSequence = true;
                _audioSource.Play();
                _speed = 0;
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _explosionAnim.SetTrigger("OnEnemyDeath");
                Object.Destroy(this.gameObject, 1.1f);
                _isDead = true;
                int powerupChance = Random.Range(0, 5);
                if (powerupChance == 0)
                {
                    StartCoroutine(EnemyPowerup());
                }
                _spawnManager.EnemyCountMinus();
            }
        }
        else if (other.gameObject.CompareTag("Void Ball") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _audioSource.Play();
            _speed = 0;
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(this.gameObject, 1.1f);
            _isDead = true;
            int powerupChance = Random.Range(0, 6);
            if (powerupChance == 0)
            {
                StartCoroutine(EnemyPowerup());
            }
            _spawnManager.EnemyCountMinus();
        }

    }

    IEnumerator EnemyPowerup()
    {
        yield return new WaitForSeconds(0.5f);
        _spawnManager.NewEnemyPowerup(transform.position);
    }

    IEnumerator RamRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (_explosionSequence == false)
        {
            _speed = 4f;
        }
        transform.rotation = new Quaternion(0, 0, 0, 0);
        _isRamActive = false;
    }

    public void StartRamRoutine()
    {
        _isRamActive = true;
        _speed = 7f;
        StartCoroutine(RamRoutine());
    }

    public void RearFireSetup()
    {
        _isSlowdownActive = true;
        StartCoroutine(RearFireRoutine());
    }

    IEnumerator RearFireRoutine()
    {
        yield return new WaitForSeconds(0.4f);
        _isSlowdownActive = false;
        _isAimActive = true;
        yield return new WaitForSeconds(0.3f);
        _isAimActive = false;
        _isLaunchActive = true;
        yield return new WaitForSeconds(0.3f);
        _isLaunchActive = false;
        _thruster.SetActive(false);
        _speed = 4f;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void PlayerDamage()
    {
        if (_player != null)
        {
            _player.Damage();
            _player.StartInvincibility();
        }
    }

    public void StartDodge(bool isDodgeLeft)
    {
        if (_explosionSequence == false)
        {
            if (isDodgeLeft == true)
            {
                _isDodgeLeft = true;
            }
            else
            {
                _isDodgeRight = true;
            }
            StartCoroutine(DodgePowerDownRoutine());
        }
    }

    IEnumerator DodgePowerDownRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _isDodgeLeft = false;
        _isDodgeRight = false;
    }
}
