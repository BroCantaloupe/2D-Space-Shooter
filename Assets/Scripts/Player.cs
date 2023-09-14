using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    private float _speed = 6f;
    private float _boostSpeed = 10f;

    private float _fireRate = 0.3f;
    private Vector3 _laserOffset = new Vector3(0, 0.5f ,0);
    private float _canFire = 0f;
    [SerializeField]
    private Object _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    [SerializeField]
    private Object _tripleShotPrefab;

    int _lives = 3;
    SpawnManager _spawnManager;
    private bool _isSpeedBoostActive;
    private bool _isTripleShotActive;
    private bool _isSheildActive;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    private int _score;
    private UIManager _uiManager;
    
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");

        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

    }
   
    void Update()
    {
        CalculateMovement();
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
           ShootLaser();
        }
       
        
    }

    void CalculateMovement()
    {
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        if(_isSpeedBoostActive == false)
        {
            transform.Translate(_speed * Time.deltaTime * direction);
        }
        else
        {
            transform.Translate(_boostSpeed * Time.deltaTime * direction);
        }

        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 0), 0);

        if(transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void ShootLaser()
    { 
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + _laserOffset, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }
    public void Damage()
    {
        
        if(_isSheildActive == false)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);

            if(_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

        }
        else
        {
            _isSheildActive = false;
            _shieldVisualizer.SetActive(false);
        }
        
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(8f);
        _isSpeedBoostActive = false;
    }

    public void ShieldsOn()
    {
        _isSheildActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
