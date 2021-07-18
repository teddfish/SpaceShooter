using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _enemyPrefab;
    [SerializeField]
    GameObject _enemyParent;
    [SerializeField]
    GameObject[] powerups;

    bool _stopSpawning = false;

    public void StartEnemyWave()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2f);

        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 6.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Length)], spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyParent.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(2f);

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
