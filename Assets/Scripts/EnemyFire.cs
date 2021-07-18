using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : Enemy
{
    [SerializeField]
    GameObject _laserPrefab;

    float _fireRate = 3f;
    float _canFire = -1f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = _fireRate + Time.time;
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Debug.Break();
        }
    }
}
