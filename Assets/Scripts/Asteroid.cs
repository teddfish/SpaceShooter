using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 20f;
    [SerializeField] GameObject _explosionPrefab;
    SpawnManager _spManager;

    AudioManager _audioManager;

    void Start()
    {
        _spManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); 
        if(_spManager == null)
        {
            Debug.LogError("Spawn Manager cannot be referenced!");
        }
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager cannot be referenced");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            _audioManager.TriggerExplosionSound();
            _spManager.StartEnemyWave();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
