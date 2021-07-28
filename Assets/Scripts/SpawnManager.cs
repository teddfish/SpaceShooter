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
    [SerializeField]
    GameObject _beamLaser;

    bool _stopSpawning = false;

    //spawning more enemies with each wave
    float _canSpawn = -1;
    float _spawnRate = 5;

    public void StartEnemyWave()
    {
        //if (Time.time > _canSpawn)
        //{
        //    StartCoroutine(SpawnEnemy());
        //}
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnBeamLaser());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2f);

        while (!_stopSpawning)
        {
            //_canSpawn = Time.time + _spawnRate;
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 6.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Length)], spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyParent.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(2f);

        while (!_stopSpawning)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomPowerup = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerup], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
            print(randomPowerup);
        }

    }

    IEnumerator SpawnBeamLaser()
    {
        yield return new WaitForSeconds(20f);

        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 6.5f, 0);
            GameObject newEnemy = Instantiate(_beamLaser, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyParent.transform;
            yield return new WaitForSeconds(Random.Range(40, 80));
        }
    }

    public void WhenPlayerDies()
    {
        _stopSpawning = true;
    }
}
