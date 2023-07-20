using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
        transform.Translate(Vector3.down * 4 * Time.deltaTime);

        if(transform.position.y <= -5)
        {
            transform.position = (new Vector3(Random.Range(-8f,8f),8 , 0));
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            Object.Destroy(gameObject);
            
            if(player != null)
            {
                player.Damage();
            }
        }
        else if(other.gameObject.tag == "Laser")
        {
            Object.Destroy(other.gameObject);
            Object.Destroy(this.gameObject);
            print("Tarhiel is dead");
        }
        
        
    }

    
}
