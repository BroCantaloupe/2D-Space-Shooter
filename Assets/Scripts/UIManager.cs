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
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprite[currentLives];
        if(currentLives == 0)
        {
            GameOverSequence();
        }
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
}
