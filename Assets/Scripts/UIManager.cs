using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private GameObject[] _shieldsImage;
    [SerializeField]
    private TMP_Text _ammoText;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Slider _waveSlider;
    [SerializeField]
    private TMP_Text _waveText;
    private float _waveTime;
    private bool _startWaveTimer;
    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        _thrusterSlider.value = 1f;
    }

    void Update()
    {
        _waveSlider.value = _waveTime;
        if (_waveTime != 0f && _startWaveTimer == true)
        {
            _waveTime -= Time.deltaTime;
        }
        if(_waveTime <= 0f && _startWaveTimer == true)
        {
            _startWaveTimer = false;
            _waveSlider.gameObject.SetActive(false);
        }
        if (_startWaveTimer == false)
        {
            _waveTime += Time.deltaTime;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprite[currentLives];
        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void AddShieldLives(int currentShieldLives)
    {
        switch (currentShieldLives - 1)
        {
            case 0: _shieldsImage[0].gameObject.SetActive(true);
                break;
            case 1: _shieldsImage[1].gameObject.SetActive(true);
                break;
            case 2: _shieldsImage[2].gameObject.SetActive(true);
                break;
            default: Debug.Log("Invalid Case");
                break;
        }
    }

    public void SubtractShieldLives(int currentShieldLives)
    {
        switch (currentShieldLives)
        {
            case 0:
                _shieldsImage[0].gameObject.SetActive(false);
                break;
            case 1:
                _shieldsImage[1].gameObject.SetActive(false);
                break;
            case 2:
                _shieldsImage[2].gameObject.SetActive(false);
                break;
            default:
                Debug.Log("Invalid Case");
                break;
        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoText.text = ("Ammo: " + ammoCount + "/30");
    }

    public void GameOverSequence()
    {
        StartCoroutine(GameOver());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOver()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void ThrusterSlider(float sliderValue)
    {
        _thrusterSlider.value = sliderValue;
    }

    public void RunWaveUI(float t)
    {
        StartCoroutine(WaveText());
        _waveSlider.maxValue = t;
        _waveTime = t;
        _startWaveTimer = true;
    }

    IEnumerator WaveText()
    {
        while (_waveTime != 0)
        {
            _waveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            _waveText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void UpdateWaveText(string wavetext)
    {
        _waveText.text = "Wave Warning: " + wavetext;
    }

    public void ActivateWaveSlider()
    {
        _waveSlider.gameObject.SetActive(true);
        _waveTime = 0.01f;
        _waveSlider.maxValue = 3f;
        _waveText.text = "Wave warning: ...";
    }

    
}
