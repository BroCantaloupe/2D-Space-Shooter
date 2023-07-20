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

    private Quaternion _spawnAngle;
    void Start()
    {
        _spawnAngle = new Quaternion (0, 180, 0, 0);
        StartCoroutine(SpawnRoutine());
    }

    
    void Update()
    {
        
    }
    
    IEnumerator SpawnRoutine()
    {
        while(_isDead == false)
        {
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8f,8f), 8, 0), _spawnAngle);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(.5f, 4f));
        }
    }

    public void OnPlayerDeath()
    {
        _isDead = true;
    }
}
