using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBallBehavior : MonoBehaviour
{
    private float _speed = 1.5f;
    private bool _isBallFired;
    private bool _canBallBeFired;
    private float _ballFireAngle;
    private Vector3 _direction;
    void Start()
    {
        StartCoroutine(BallDelay());
    }

    void Update()
    {
        if (_isBallFired == true)
        {            
            transform.Translate(_direction * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isBallFired == false && _canBallBeFired == true)
        {
            _ballFireAngle = (Input.GetAxis("Horizontal") * 2);
            _direction = new(_ballFireAngle, _speed);
            _isBallFired = true;
            _canBallBeFired = false;
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

