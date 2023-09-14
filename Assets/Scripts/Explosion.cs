using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;
    void Start()
    {
        Destroy(this.gameObject, 1.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
