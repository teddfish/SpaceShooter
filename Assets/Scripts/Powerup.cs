using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] Powerups _whichPowerup;
    AudioManager _audioManager;

    public enum Powerups
    {
        TripleShot,
        Speed,
        Shield
    }

    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager cannot be referenced");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_whichPowerup)
                {
                    case Powerups.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case Powerups.Speed:
                        player.ActivateSpeedBoost();
                        break;
                    case Powerups.Shield:
                        player.ActivateShields();
                        break;
                }
                
            }

            _audioManager.TriggerPowerupSound();

            Destroy(this.gameObject);
        }
    }
}
