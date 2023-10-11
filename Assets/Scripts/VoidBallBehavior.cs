using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBallBehavior : MonoBehaviour
{
    private float _speed = 2.5f;
    private bool _isBallFired;
    private bool _canBallBeFired;
    private float _ballFireAngle;
    private Vector3 _direction;
    [SerializeField]
    private Transform _playerTransform;
    private CircleCollider2D _collider;
    void Start()
    {
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _collider = GetComponentInChildren< CircleCollider2D > ();
        if (_collider == null)
        {
            Debug.LogError("The Collider is NULL");
        }
        if (_playerTransform == null)
        {
            Debug.LogError("Player is NULL");
        }
        _collider.enabled = false;
        StartCoroutine(BallDelay());

    }

    void Update()
    {
        if (_isBallFired == true)
        {            
            _collider.enabled = true;
            transform.Translate(_direction * Time.deltaTime);
        }
        else
        {
            float step = (_speed * 2) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, step);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isBallFired == false && _canBallBeFired == true)
        {
            _ballFireAngle = (Input.GetAxis("Horizontal") * 2);
            _direction = new(_ballFireAngle, _speed);
            _isBallFired = true;
            _canBallBeFired = false;
        }

        if (transform.position.y >= 7.8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);

        }
    }


    IEnumerator BallDelay()
    {
        yield return new WaitForSeconds(1.2f);
        _canBallBeFired = true;
    }

    public void BallMovement(Vector3 playerPosition, float playerSpeed)
    {
        Vector3.MoveTowards(transform.position, playerPosition, playerSpeed);
        Debug.Log(transform.position + " " + (playerPosition) + (playerSpeed));
    }

}

