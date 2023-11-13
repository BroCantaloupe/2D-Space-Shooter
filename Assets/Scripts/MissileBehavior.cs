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
    private CircleCollider2D _circleCollider;
    [SerializeField]
    private GameObject _explosion;
    private bool _isExploding;
    private int _angleModifier = 90;
    private float _turnSpeed = 3f;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
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
        if(_target != null)
        {
            FacingEnemy();
        }
    }

    void FacingEnemy()
    {
        if (!_isExploding)
        {
            Vector3 vectorToTarget = _target.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _angleModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turnSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(ExplodeRoutine());
        }
    }

    IEnumerator ExplodeRoutine()
    {
        _circleCollider.enabled = true;
        _speed = 0;
        _explosion.SetActive(true);
        _acceleration = 0;
        _isExploding = true;
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }
}
