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
    public override void Update()
    {
        base.Update();

        if (Time.time > _canFire && this.GetComponent<BoxCollider2D>().enabled)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = _fireRate + Time.time;
            Vector3 _spawnPosition = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            GameObject enemyLaser = Instantiate(_laserPrefab, _spawnPosition, Quaternion.identity);
        }
    }
}
