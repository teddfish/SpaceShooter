using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEnemy : Enemy
{
    [SerializeField]
    GameObject _shields;
    bool _shieldActive = true;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (_shieldActive && other.CompareTag("Laser"))
        {
            _shields.SetActive(false);
            _shieldActive = false;
            Destroy(other.gameObject);
            //print("received");
        }
        else if (_shieldActive && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _shields.SetActive(false);
            _shieldActive = false;
        }
        else
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
