using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThruster : MonoBehaviour
{
    private Enemy _enemy;
    void Start()
    {
        _enemy = GameObject.Find("Enemy").GetComponentInParent<Enemy>();
        if(_enemy == null)
        {
            Debug.Log("Enemy is NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _enemy.PlayerDamage();
        }
    }
}
