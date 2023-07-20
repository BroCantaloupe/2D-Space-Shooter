using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
public float horizontalInput;
public float verticalInput;
private float _speed = 10f;
private Vector3 _laserOffset = new Vector3(0, 0.5f ,0);
[SerializeField]
private float _fireRate = 0.3f;
private float _canFire = 0f;
[SerializeField]
private Object _laserPrefab;
[SerializeField]
int _lives = 3;
SpawnManager _spawnManager;
    
    void Start()
    {
    transform.position = new Vector3(0, 0, 0);
    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    if(_spawnManager == null)
    {
        Debug.LogError("The Spawn Manager is NULL");
    }

    }
   
    void Update()
    {
        CalculateMovement();
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
           ShootLaser();
        }
       
        
    }

    void CalculateMovement()
    {
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if(transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void ShootLaser()
    { 
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        
    }
    public void Damage()
    {
        _lives--;
        
        if(_lives < 1)
        {
            //add stop spawning
            
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }
}
