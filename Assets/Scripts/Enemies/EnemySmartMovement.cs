using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmartMovement : MonoBehaviour
{
    
    private Enemy _enemy;
    void Start()
    {
        _enemy = GameObject.Find("Enemy").GetComponentInParent<Enemy>();
        if(_enemy == null)
        {
            Debug.LogError("enemy is NULL");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
}
