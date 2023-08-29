using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    
    public float bulletspeed = 40f;

    void Update()
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
}
