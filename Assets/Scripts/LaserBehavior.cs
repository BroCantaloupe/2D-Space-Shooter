using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    
    private float bulletspeed = 40f;
    private float enemyBulletSpeed = -8f;
    private bool isEnemyLaser = false;

    void Update()
    {
        if(isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }


    private void MoveUp()
    {
        Vector3 direction = new Vector3(0,1);
        transform.Translate (direction * bulletspeed * Time.deltaTime);
        if(transform.position.y >= 7.5f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
            
        }

    }

    private void MoveDown()
    {
        Vector3 direction = new Vector3(0, 1);
        transform.Translate(direction * enemyBulletSpeed * Time.deltaTime);
        if (transform.position.y <= -7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);

        }
    }

    public void AssignEnemyLaser()
    {
        isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if(other != null)
            {
                player.Damage();
            }
        }
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}
