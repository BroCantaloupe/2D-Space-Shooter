using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Animator _explosionAnim;
    private float _speed = 4f;
    private bool _explosionSequence;
    private void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        _explosionAnim = GetComponent<Animator>();
        if(_explosionAnim == null)
        {
            Debug.LogError("Animator is NULL");
        }
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }
    void Update()
    {
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -7)
        {
            transform.position = (new Vector3(Random.Range(-8f,8f),8 , 0));
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _speed = 0;
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(gameObject, 1.1f);
            
            if(_player != null)
            {
                _player.Damage();
            }
        }
        else if(other.gameObject.CompareTag("Laser") && _explosionSequence == false)
        {
            _explosionSequence = true;
            _speed = 0;
            Object.Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Object.Destroy(this.gameObject, 1.1f);

        }
        
        
    }

    
}
