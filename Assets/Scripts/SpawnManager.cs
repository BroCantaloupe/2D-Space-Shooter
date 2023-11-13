using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    bool _isDead = false;
    private int _score = 0;
    bool _waveActive;
    private int _enemyCount;


    //less spawning
    //slower waves, change waves to be activated through score, no longer time based but enemy based
    //wave 1 SCORE = 60, 3 chasers, 2 enemy, 2 turret and 1 enemy = 8  
    //wave 2 SCORE = 220, 2 chaser, 2 enemy, 2 enemy, 1 chaser 2 turret, 2 turret 2 enemy = 13
    //wave 3 SCORE = 440, 3 enemy, enemy and chaser, 2 turret chaser, 4 enemy, 2 enemy 2 chaser = 16
    //wave 4 SCORE = 700, 2 turret, 2 turret, 2 chaser 2 enemy, turret chaser enemy, chaser chaser chaser chaser, 2 turret, enemy x5 = 22
    //= 59.... After final wave player moves foward, fade to black then fade into next scene

    [SerializeField]
    GameObject[] _enemy;
    [SerializeField]
    GameObject _enemyContainer;


    UIManager _uiManager;
    [SerializeField]
    private GameObject[] _powerup;
    [SerializeField]
    private GameObject[] _evilPowerup;
    [SerializeField]
    private GameObject[] _superPowerup;
    [SerializeField]
    private GameObject[] _commonPowerup;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void EnemyCountMinus()
    {
        _enemyCount--;
    }

    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while(_isDead == false)
        {
            if (_waveActive == false)
            {
                Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 9f), 0);
                int randomEnemy = Random.Range(0, 3);
                GameObject newEnemy = Instantiate(_enemy[randomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyCount++;

            }
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while(_isDead == false)
        {
            int evilPowerupChance = Random.Range(0, 4);
            Vector3 posToSpawn = new(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 5);
            if (evilPowerupChance != 0)
            {
                Instantiate(_powerup[randomPowerup], posToSpawn, Quaternion.identity);
            }
            else
            {
                Instantiate(_evilPowerup[randomPowerup], posToSpawn, Quaternion.identity);
                Instantiate(_commonPowerup[randomPowerup], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(4f, 7f)); 
        }
    }

    public void NewEnemyPowerup(Vector3 enemyTransform)
    {
        int specialPowerupChance = Random.Range(0, 9);
        int randomEnemyPowerup = Random.Range(0,5);
        if(specialPowerupChance != 0)
        {
            Instantiate(_powerup[randomEnemyPowerup], enemyTransform, Quaternion.identity);
        }
        else
        {
            Instantiate(_superPowerup[0], enemyTransform, Quaternion.identity);

        }
    }


    public void OnPlayerDeath()
    {
        _isDead = true;
    }

    public void UpdateScore(int score)
    {
        _score = score;
    }

    public void Wave1()
    {
        StartCoroutine(Wave1Routine());
        _uiManager.ActivateWaveSlider();
        _waveActive = true;

    }

    public void Wave2()
    {
        StartCoroutine(Wave2Routine());
        _uiManager.ActivateWaveSlider();
        _waveActive = true;

    }

    public void Wave3()
    {
        StartCoroutine(Wave3Routine());
        _uiManager.ActivateWaveSlider();
        _waveActive = true;

    }

    public void Wave4()
    {
        StartCoroutine(Wave4Routine());
        _uiManager.ActivateWaveSlider();
        _waveActive = true;

    }

    //less spawning
    //slower waves, change waves to be activated through score, no longer time based but enemy based
    //= 59.... After final wave player moves foward, fade to black then fade into next scene & 1000 points

    IEnumerator Wave1Routine()
    {
        //wave 1 SCORE = 60, 3 chasers, 2 enemy, 2 turret and 1 enemy = 8  

        yield return new WaitForSeconds(3.1f);
        _uiManager.UpdateWaveText("Small");
        _uiManager.RunWaveUI(8);
        InstantiateChaser();
        yield return new WaitForSeconds(0.3f);
        InstantiateChaser();
        yield return new WaitForSeconds(0.3f);
        InstantiateChaser();
        yield return new WaitForSeconds(2.2f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(3f);
        InstantiateTurret();
        InstantiateTurret();
        yield return new WaitForSeconds(1f);
        InstantiateEnemy();
        _waveActive = false;
    }

    IEnumerator Wave2Routine()
    {
        //wave 2 SCORE = 220, 2 chaser, 2 enemy, 2 enemy, 1 chaser 2 turret, 2 turret 2 enemy = 13

        yield return new WaitForSeconds(3.1f);
        _uiManager.UpdateWaveText("Medium");
        _uiManager.RunWaveUI(13);
        InstantiateChaser();
        yield return new WaitForSeconds(0.45f);
        InstantiateChaser();
        yield return new WaitForSeconds(1.8f);
        InstantiateEnemy();
        yield return new WaitForSeconds(0.6f);
        InstantiateEnemy();
        yield return new WaitForSeconds(2.5f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(3f);
        InstantiateTurret();
        yield return new WaitForSeconds(1f);
        InstantiateTurret();
        InstantiateChaser();
        yield return new WaitForSeconds(2.6f);
        InstantiateTurret();
        InstantiateTurret();
        InstantiateEnemy();
        yield return new WaitForSeconds(1.2f);
        InstantiateEnemy();
        _waveActive = false;
    }

    IEnumerator Wave3Routine()
    {
        //wave 3 SCORE = 440, 3 enemy, enemy and chaser, 2 turret chaser, 4 enemy, 2 enemy 2 chaser = 16

        yield return new WaitForSeconds(3.1f);
        _uiManager.UpdateWaveText("Large");
        _uiManager.RunWaveUI(16);
        InstantiateEnemy();
        yield return new WaitForSeconds(1.3f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(3f);
        InstantiateEnemy();
        InstantiateChaser();
        yield return new WaitForSeconds(2.8f);
        InstantiateTurret();
        InstantiateTurret();
        InstantiateChaser();
        InstantiateChaser();
        yield return new WaitForSeconds(3.7f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(1.4f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(3.6f);
        InstantiateChaser();
        InstantiateChaser();
        yield return new WaitForSeconds(1.2f);
        InstantiateEnemy();
        InstantiateEnemy();
        _waveActive = false;
    }

    IEnumerator Wave4Routine()
    {
        //wave 4 SCORE = 700, 2 turret, 2 turret, 2 chaser 2 enemy, turret chaser enemy, chaser chaser chaser chaser, 2 turret, enemy x5 = 22

        yield return new WaitForSeconds(3.1f);
        _uiManager.UpdateWaveText("Danger");
        _uiManager.RunWaveUI(22);
        InstantiateTurret();
        InstantiateTurret();
        yield return new WaitForSeconds(2f);
        InstantiateTurret();
        InstantiateTurret();
        yield return new WaitForSeconds(2.6f);
        InstantiateEnemy();
        InstantiateChaser();
        yield return new WaitForSeconds(2f);
        InstantiateEnemy();
        InstantiateChaser();
        yield return new WaitForSeconds(4f);
        InstantiateEnemy();
        InstantiateChaser();
        InstantiateTurret();
        yield return new WaitForSeconds(4f);
        InstantiateChaser();
        yield return new WaitForSeconds(1f);
        InstantiateChaser();
        yield return new WaitForSeconds(1f);
        InstantiateChaser();
        yield return new WaitForSeconds(1f);
        InstantiateChaser();
        yield return new WaitForSeconds(3.4f);
        InstantiateTurret();
        yield return new WaitForSeconds(1f);
        InstantiateTurret();
        yield return new WaitForSeconds(3f);
        InstantiateEnemy();
        InstantiateEnemy();
        yield return new WaitForSeconds(1.7f);
        InstantiateEnemy();
        InstantiateEnemy();
        InstantiateEnemy();
        _waveActive = false;
    }

    private void InstantiateEnemy()
    {
        Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 9f), 0);
        GameObject newEnemy = Instantiate(_enemy[0], posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    private void InstantiateTurret()
    {
        Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 9f), 0);
        GameObject newEnemy = Instantiate(_enemy[2], posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }
    private void InstantiateChaser()
    {
        Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 9f), 0);
        GameObject newEnemy = Instantiate(_enemy[1], posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

}
