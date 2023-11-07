using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    bool _isDead = false;
    private int _score = 0;
    bool _waveActive;
    private int _enemyCount;
    private int _enemyCap = 8;

    [SerializeField]
    GameObject[] _enemy;
    [SerializeField]
    GameObject _enemyContainer;


    UIManager _uiManager;
    [SerializeField]
    private GameObject[] _powerup;

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
        StartCoroutine(SpawnNewWave());
        yield return new WaitForSeconds(3f);
        while(_isDead == false)
        {
            if (_enemyCount <= _enemyCap)
            {
                Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 9f), 0);
                GameObject newEnemy = Instantiate(_enemy[0], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyCount++;

            }
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    public void StartChaserCoroutine()
    {
        StartCoroutine(SpawnEnemyChaserRoutine());
    }

    IEnumerator SpawnEnemyTurretRoutine()
    {
        yield return new WaitForSeconds(2f);
        while(_isDead == false)
        {
            if (_enemyCount <= _enemyCap)
            {
                int doubleSpawn = Random.Range(0, 5);
                float spawnPointX = Random.Range(-9.5f, 9.5f);
                if (doubleSpawn == 0)
                {
                    float spawnPointX2 = spawnPointX * -1;
                    Vector3 posToSpawnClone = new(spawnPointX2, 8, 0);
                    GameObject newEnemy2 = Instantiate(_enemy[2], posToSpawnClone, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    _enemyCount++;
                }

                Vector3 posToSpawn = new(spawnPointX, 8, 0);
                GameObject newEnemy = Instantiate(_enemy[2], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyCount++;

            }
            yield return new WaitForSeconds(Random.Range(8f, 10f));

        }
    }

    public void StartTurretRoutine()
    {
        StartCoroutine(SpawnEnemyTurretRoutine());
    }

    IEnumerator SpawnEnemyChaserRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_isDead == false)
        {

            if (_enemyCount <= _enemyCap)
            {
                Vector3 posToSpawn = new(Random.Range(-12f, 12f), 8, 0);
                GameObject newEnemy = Instantiate(_enemy[1], posToSpawn, Quaternion.identity);
                _enemyCount++;
                newEnemy.transform.parent = _enemyContainer.transform;

            }
                yield return new WaitForSeconds(Random.Range(8f, 10f));
               
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while(_isDead == false)
        {
            Vector3 posToSpawn = new(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 7);
            
            Instantiate(_powerup[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4f, 7f)); 
        }
    }

    private void SpawnWavePowerup(int amount)
    {
        int powerup = Random.Range(5, 10); //shield, ammo, health, ammo, ammo
        Vector3 posToSpawn = new(Random.Range(-4f, 4f), 7, 0);
        Instantiate(_powerup[powerup],posToSpawn , Quaternion.identity);
        amount--;
        if(amount != 0)
        {
            SpawnWavePowerup(amount);
        }
    }

    public void NewEnemyPowerup(Vector3 enemyTransform)
    {
        int specialPowerupChance = Random.Range(0, 11);
        int randomEnemyPowerup = Random.Range(6,10);
        if(specialPowerupChance != 0)
        {
            Instantiate(_powerup[randomEnemyPowerup], enemyTransform, Quaternion.identity);
        }
        else
        {
            Instantiate(_powerup[10], enemyTransform, Quaternion.identity);

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

    IEnumerator SpawnNewWave()
    {
        if (_isDead == false)
        {
            yield return new WaitForSeconds(25f);
            SpawnWavePowerup(Random.Range(2, 4));
            _uiManager.ActivateWaveSlider();

            float spawnPointX = Random.Range(-9.5f, 9.5f);
            float spawnPointX2 = spawnPointX * -1;
            Vector3 posToSpawnClone = new(spawnPointX2, 8, 0);
            Vector3 posToSpawn = new(spawnPointX, 8, 0);

            GameObject newEnemy = Instantiate(_enemy[2], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemyCount++;
            GameObject newEnemy2 = Instantiate(_enemy[2], posToSpawnClone, Quaternion.identity);
            newEnemy2.transform.parent = _enemyContainer.transform;
            _enemyCount++;

            yield return new WaitForSeconds(3.1f);
            if (_score <= 200)
            {
                //small
                _uiManager.UpdateWaveText("Small");
                _uiManager.RunWaveUI(5f);
                _waveActive = true;
                StartCoroutine(EnemyWave());
                yield return new WaitForSeconds(5f);
                _waveActive = false;
                StartCoroutine(SpawnNewWave());
            }
            else if (_score <= 600)
            {
                //medium
                _uiManager.UpdateWaveText("Medium");
                _uiManager.RunWaveUI(8f);
                _waveActive = true;
                StartCoroutine(EnemyWave());
                yield return new WaitForSeconds(8f);
                _waveActive = false;
                StartCoroutine(SpawnNewWave());
            }
            else if (_score <= 1500)
            {
                //large
                _uiManager.UpdateWaveText("Large");
                _uiManager.RunWaveUI(10f);
                _waveActive = true;
                StartCoroutine(EnemyWave());
                yield return new WaitForSeconds(10f);
                _waveActive = false;
                StartCoroutine(SpawnNewWave());
            }
            else if (_score <= 2200)
            {
                //huge
                _uiManager.UpdateWaveText("Huge");
                _uiManager.RunWaveUI(12f);
                _waveActive = true;
                StartCoroutine(EnemyWave());
                yield return new WaitForSeconds(12f);
                _waveActive = false;
                StartCoroutine(SpawnNewWave());
            }
            else if (_score > 2200)
            {
                //danger
                _uiManager.UpdateWaveText("Danger");
                _uiManager.RunWaveUI(16f);
                _waveActive = true;
                StartCoroutine(EnemyWave());
                yield return new WaitForSeconds(16f);
                _waveActive = false;
                StartCoroutine(SpawnNewWave());
            }
        }
    }

    IEnumerator EnemyWave()
    {
        if (_enemyCount <= _enemyCap)
        {
            Vector3 posToSpawn = new(Random.Range(-8f, 8f), Random.Range(8f, 8.5f), 0);
            GameObject newEnemy = Instantiate(_enemy[Random.Range(0, 2)], posToSpawn, Quaternion.identity);
            _enemyCount++;
            newEnemy.transform.parent = _enemyContainer.transform;
        }
        if(_waveActive == true)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyWave());
        }
    }
}
