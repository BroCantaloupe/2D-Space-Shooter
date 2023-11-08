using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmartMovement : MonoBehaviour
{
    public int enemyID;
    private Enemy _enemy;
    private EnemyChaser _enemyChaser;
    [SerializeField]
    private GameObject _explosionPrefab;
    void Start()
    {
        if (enemyID == 0)
        {
            _enemy = GameObject.Find("Enemy").GetComponentInParent<Enemy>();
            if (_enemy == null)
            {
                Debug.LogError("enemy is NULL");
            }
        }
        if(enemyID == 1)
        {
            _enemyChaser = GameObject.Find("Enemy_Chaser").GetComponentInParent<EnemyChaser>();
            if(_enemyChaser == null)
            {
                Debug.LogError("Chaser is NULL");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyID == 0)
            {
                Transform playerTransform = collision.GetComponent<Transform>();
                Transform enemyTransform = GetComponent<Transform>();
                if (playerTransform != null)
                {
                    if (enemyTransform.position.y >= playerTransform.position.y)
                    {
                        _enemy.StartRamRoutine();
                    }
                    if (enemyTransform.position.y < playerTransform.position.y)
                    {
                        _enemy.RearFireSetup();
                    }

                }
                Destroy(this.gameObject);
            }
        }
        if (collision.CompareTag("Powerup"))
        {
            if(enemyID == 1)
            {
                Transform powerupTranform = collision.GetComponent<Transform>();
                Transform enemyTransform = GetComponent<Transform>();
                _enemyChaser.StartPowerupDestroyRoutine(powerupTranform);

            }
        }
    }
}
