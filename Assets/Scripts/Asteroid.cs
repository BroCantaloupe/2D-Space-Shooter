using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Object explosionPrefab;
    private float rotateSpeed = 10f;
    private SpawnManager _spawnManager;
    private bool _isHit;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
    }

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") || other.CompareTag("Void Ball") && _isHit == false)
        {
            _spawnManager.StartSpawning();
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
            }
            Destroy(this.gameObject, 0.4f);
            _isHit = true;
        }
    }
}
