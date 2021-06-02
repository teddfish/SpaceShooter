using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    GameObject _enemyParent;
    [SerializeField]
    GameObject[] powerups;

    bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyParent.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (!_stopSpawning)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }

    }

    public void WhenPlayerDies()
    {
        _stopSpawning = true;
    }
}
