using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmartMovement : MonoBehaviour
{
    public int enemyID;
    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    private EnemyChaser _enemyChaser;
    [SerializeField]
    private GameObject _explosionPrefab;
    void Start()
    {
        if (enemyID == 0)
        {
            _enemy = GetComponentInParent<Enemy>();
            if (_enemy == null)
            {
                Debug.LogError("enemy is NULL");
            }
        }
        if(enemyID == 1)
        {
            _enemyChaser = GetComponentInParent<EnemyChaser>();
            if(_enemyChaser == null)
            {
                Debug.LogError("Chaser is NULL");
            }
        }
    }

    public void AssignID(int id)
    {
        enemyID = id;
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
                _enemyChaser.StartPowerupDestroyRoutine(powerupTranform);

            }
        }
        if (collision.CompareTag("Laser"))
        {
            if (enemyID == 0)
            {
                int dodgeChance = Random.Range(0, 3); //1 in 3
                if (dodgeChance == 0)
                {
                    Transform laserTransform = collision.GetComponent<Transform>();
                    Transform enemyTransform = GetComponent<Transform>();
                    if (laserTransform.position.x > enemyTransform.position.x) //true is left, false is right
                    {
                        _enemy.StartDodge(true);
                        //move to the left, laser is right
                    }
                    else if (laserTransform.position.x <= enemyTransform.position.x)
                    {
                        _enemy.StartDodge(false);
                        //move to the right, laser is left
                    }
                }
            }
        }
    }
}
