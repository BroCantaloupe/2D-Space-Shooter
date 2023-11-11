using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    private float _speed = -4f;
    private float _acceleration = 20f;
    private GameObject[] _enemies;
    private float _distance;
    private float _closestTarget = Mathf.Infinity;
    private GameObject _target;

    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in _enemies)
        {
            _distance = (enemy.transform.position - this.transform.position).sqrMagnitude;
            if(_distance < _closestTarget)
            {
                _distance = _closestTarget;
                _target = enemy;
            }
        }
    }
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        _speed +=  _acceleration * Time.deltaTime;

        if (transform.position.y >= 8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(""))
        {

        }
    }
}
