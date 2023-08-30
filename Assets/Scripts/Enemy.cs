using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        
    }
    void Update()
    {
        
        transform.Translate(Vector3.down * 4 * Time.deltaTime);

        if(transform.position.y <= -7)
        {
            transform.position = (new Vector3(Random.Range(-8f,8f),8 , 0));
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            Object.Destroy(gameObject);
            
            if(_player != null)
            {
                _player.Damage();
            }
        }
        else if(other.gameObject.CompareTag("Laser"))
        {
            Object.Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            Object.Destroy(this.gameObject);

        }
        
        
    }

    
}
