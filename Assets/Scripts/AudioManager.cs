using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSrc;
    [SerializeField] AudioClip _explosionSound, _powerupPickupSound;

    private void Start()
    {
        _audioSrc = GetComponent<AudioSource>();
        if (_audioSrc == null)
        {
            Debug.LogError("Audio Source on audio manager cannot be referenced!");
        }
    }

    public void TriggerExplosionSound()
    {
        _audioSrc.PlayOneShot(_explosionSound);
    }

    public void TriggerPowerupSound()
    {
        _audioSrc.PlayOneShot(_powerupPickupSound);
    }
}
