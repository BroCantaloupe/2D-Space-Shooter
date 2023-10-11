using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private float _speed = 6f;
    private float _boostSpeed = 10f;
    private float _shiftSpeed = 0f;
    private float _shiftTime = 1.0f;

    private float _fireRate = 0.3f;
    private Vector3 _laserOffset = new(0, 0.5f ,0);
    private float _canFire = 0f;
    [SerializeField]
    private Object _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    [SerializeField]
    private Object _tripleShotPrefab;
    private int _laserAmmo = 15;

    int _lives = 3;
    SpawnManager _spawnManager;
    private bool _isSpeedBoostActive;
    private bool _isTripleShotActive;
    private bool _isSheildActive;
    private bool _isVoidBallActive;
    private bool _isVoidBallShot;
    private int _shieldLives;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _voidBallPrefab;
    private Animator _cameraAnimator;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();

        if(_cameraAnimator == null)
        {
            Debug.LogError("Camera Animator is NULL");
        }
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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _laserAmmo != 0 && _isVoidBallActive == false)
        {
            ShootLaser();
            _laserAmmo--;
            _uiManager.UpdateAmmo(_laserAmmo);
        }
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isVoidBallActive == true && _isVoidBallShot == false)
        {
            _isVoidBallShot = true;
            Instantiate(_voidBallPrefab, transform.position, Quaternion.identity);
            StartCoroutine(VoidBallRespawn());
        }

        
    }

    IEnumerator VoidBallRespawn()
    {
        yield return new WaitForSeconds(1.2f);
        _isVoidBallShot = false;
    }

    void CalculateMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(_isSpeedBoostActive == true)
            {
                _shiftTime -= Time.deltaTime;
            }
            else
            {
                _shiftTime -= (Time.deltaTime * 2);
            }
            if(_shiftTime != 0)
            {
                _shiftSpeed = 5f;
            }
            if(_shiftTime <= 0)
            {
                _shiftTime = 0f;
                _shiftSpeed = -1.3f;
            }
            _uiManager.ThrusterSlider(_shiftTime);
        }
        else
        {
            _shiftSpeed = 0f;
            _shiftTime += (Time.deltaTime / 3);
            if(_shiftTime >= 1.0f)
            {
                _shiftTime = 1.0f;
            }
            _uiManager.ThrusterSlider(_shiftTime);
        }

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new(_horizontalInput, _verticalInput, 0);
        
        if(_isSpeedBoostActive == false)
        {
            transform.Translate((_speed + _shiftSpeed) * Time.deltaTime * direction);
        }
        else
        {
            transform.Translate((_boostSpeed + _shiftSpeed) * Time.deltaTime * direction);
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
            UpdateDamageVisual();
            _cameraAnimator.SetTrigger("playerHurt");
        }
        else
        {
            _shieldLives--;
            _uiManager.SubtractShieldLives(_shieldLives);
            if(_shieldLives <= 0)
            {
            _isSheildActive = false;
            _shieldVisualizer.SetActive(false);
            }
        }

        if (_lives < 1)
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

    public void ShieldGet()
    {
        if (_shieldLives < _lives)
        {
            _shieldLives++;
        }
        _isSheildActive = true;
        _shieldVisualizer.SetActive(true);
        _uiManager.AddShieldLives(_shieldLives);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AddLife()
    {
        _lives++;
        if(_lives > 3)
        {
            _lives = 3;
        }
        _uiManager.UpdateLives(_lives);
        UpdateDamageVisual();
    }

    public void AddAmmo()
    {
        _laserAmmo += 5;
        if(_laserAmmo > 30)
        {
            _laserAmmo = 30;
        }
        _uiManager.UpdateAmmo(_laserAmmo);

    }

    public void ToggleVoidBall()
    {
        _isVoidBallActive = true;
        StartCoroutine(VoidBallPowerDownRoutine());
        Instantiate(_voidBallPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator VoidBallPowerDownRoutine()
    {
        yield return new WaitForSeconds(6f);
        _isVoidBallActive = false;
    }

    private void UpdateDamageVisual()
    {
        switch (_lives)
        {
            case 1:
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(true);
                break;
            case 2:
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
                break;
            case 3:
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(false);
                break;
        }

    }
    
}
