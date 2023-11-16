using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    private int _angleModifier = 270;
    private float _speed = 10f;
    private float _rotateSpeed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Transform _playerTransform;
    [SerializeField]
    private Object _explosionPrefab;
    private bool _isDead;
    [SerializeField]
    private GameObject _turretBase;
    private float _acceleration;
    int _lasersFired = 2;

    //laser offset of y -0.7

    void Start()
    {
        _acceleration = Random.Range(8f, 18f);
        _playerTransform = GameObject.Find("Player").transform.GetComponent<Transform>();
        if (_playerTransform == null)
        {
            Debug.LogError("Player Transform is NULL");
        }
        StartCoroutine(ShootLaser());

    }

    void Update()
    {
        if(_speed > 0.01f)
        {
            MoveOnScreen();
        }
        else
        {
            FacingPlayer();
        }

    }

    void MoveOnScreen()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        _speed -= _acceleration * Time.deltaTime;
    }

    void FacingPlayer()
    {
        Vector3 vectorToTarget = _playerTransform.transform.position - transform.position;
        if (_playerTransform != null)
        {
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _angleModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotateSpeed);
        }
        _turretBase.transform.rotation = Quaternion.Euler(0, 0, 0);
    } 

    IEnumerator ShootLaser()
    {
        while(_isDead == false)
        {
            yield return new WaitForSeconds(3f);
            if(_lasersFired == 0)
            {
                _lasersFired = 2;
            }
            StartCoroutine(LasersFired());
        }
    }

    IEnumerator LasersFired()
    {
        while(_lasersFired != 0)
        {
            GameObject turretLaser = Instantiate(_laserPrefab, transform.position, transform.rotation);
            LaserBehavior laser = turretLaser.GetComponent<LaserBehavior>();
            if (laser != null)
            {
                laser.AssignEnemyLaser();
            }
            _lasersFired--;
            yield return new WaitForSeconds(0.3f);
        }
    }

    
}
