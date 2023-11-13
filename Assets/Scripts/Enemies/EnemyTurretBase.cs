using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretBase : MonoBehaviour
{
    [SerializeField]
    private Object _explosionPrefab;
    private Player _player;
    private SpawnManager _spawnManager;
    private bool _isShieldActive;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("spawn manager is NULL");
        }
        int shieldChance = Random.Range(0, 4);
        if (shieldChance == 0)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Laser") && _isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);

        }
        else if (other.CompareTag("Laser"))
        {
            _spawnManager.EnemyCountMinus();
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject, 0.2f);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
        if (other.CompareTag("Void Ball"))
        {
            _spawnManager.EnemyCountMinus();
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject, 0.2f);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
