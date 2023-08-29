using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    bool _isDead = false;
    [SerializeField]
    GameObject _enemy;
    [SerializeField]
    GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    
    void Update()
    {
        
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        while(_isDead == false)
        {
            Vector3 posToSpawn = new(Random.Range(-8f, 8f), 8, 0);
            GameObject newEnemy = Instantiate(_enemy, posToSpawn , Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(.5f, 4f));
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while(_isDead == false)
        {
            Vector3 posToSpawn = new(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_tripleShotPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4f, 7f));
        }
    }

    public void OnPlayerDeath()
    {
        _isDead = true;
    }
}