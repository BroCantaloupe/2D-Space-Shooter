using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private float _speed = 6f;
    private float _boostSpeed = 10f;
    private float _slowSpeed = 3.5f;
    private float _shiftSpeed = 0f;
    private float _shiftTime = 1.0f;
    private BoxCollider2D _collider;


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
    private bool _spawnChasers;
    private bool _spawnTurrets;
    SpawnManager _spawnManager;
    private bool _isSpeedBoostActive;
    private bool _isSlowActive;
    private bool _isTripleShotActive;
    private bool _isSheildActive;
    private bool _isVoidBallActive;
    private bool _isVoidBallShot;
    private int _shieldLives;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _blindness;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _voidBallPrefab;
    private Animator _cameraAnimator;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        if(_collider == null)
        {
            Debug.LogError("Collider is NULL");
        }
        if(_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer NULL");
        }
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

        if(_score >= 100 && _spawnChasers == false)
        {
            _spawnManager.StartChaserCoroutine();
            _spawnChasers = true;
        }
        if(_score >= 700 && _spawnTurrets == false)
        {
            _spawnManager.StartTurretRoutine();
            _spawnTurrets = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _spawnManager.StartTurretRoutine();

        }
    }

    IEnumerator VoidBallRespawn()
    {
        yield return new WaitForSeconds(1.2f);
        _isVoidBallShot = false;
    }

    void CalculateMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _isSlowActive == false)
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
        
        if(_isSlowActive == true)
        {
            transform.Translate((_slowSpeed + _shiftSpeed) * Time.deltaTime * direction);
        }
        else if(_isSpeedBoostActive == false)
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

    public void SlowActive()
    {
        _isSlowActive = true;
        StartCoroutine(SlowPowerDownRoutine());
        _spriteRenderer.color = new Color(0.25f, 0.75f, 1, 1);
    }

    IEnumerator SlowPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSlowActive = false;
        _spriteRenderer.color = Color.white;
    }

    public void Teleport()
    {
        float randomX = Random.Range(-11f, 11f);
        float randomY = Random.Range(-5f, 0f);
        transform.position = new Vector3(randomX, randomY, 0);
    }

    public void BlindnessActive()
    {
        _blindness.SetActive(true);
        StartCoroutine(BlindnessPowerDownRoutine());
    }

    IEnumerator BlindnessPowerDownRoutine()
    {
        yield return new WaitForSeconds(12f);
        _blindness.SetActive(false);
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
        _spawnManager.UpdateScore(_score);
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
        _laserAmmo += 10;
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

    public void StartInvincibility()
    {
        StartCoroutine(InvincibiltyRoutine());
        _collider.enabled = false;
    }

    IEnumerator InvincibiltyRoutine()
    {
        yield return new WaitForSeconds(1f);
        _collider.enabled = true;
    }
    //
    //
    //
    //
    //
    //
    //
    //
    //homing projectile - rare homing missile powerup
    //boss AI - final wave - after the score reaches a certain point, the text will turn red indicating the next wave is the boss wave
    //boss AI part 2 - boss wave will be designed to spawn specific enemies as opposed to RNG, when enemies are all cleared, boss
    //boss AI part 3 - design the boss...................

    //bonus? - new player type, redesign
    //story scene after pressing start, controls as well

}
