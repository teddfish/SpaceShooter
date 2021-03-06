using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    //declaring a speed variable to give the movement speed to the player object
    [SerializeField]
    float _speed = 10f, _boostSpeed = 15f, _defaultSpeed = 10f, _thrusterModifier = 5f;
    [SerializeField]
    GameObject _laserPrefab, _tripleShotPrefab;
    Vector3 _laserSpawnOffset;
    [SerializeField]
    float _fireRate = 0.2f;
    float _canFire = 0f;

    [SerializeField]
    int _lives = 3, _shieldStrength = 3;

    //adding different visuals for different shield strength
    [SerializeField]
    GameObject _shieldObject, _shield50, _shield25;

    [SerializeField]
    GameObject  _lEngine, _rEngine;

    SpawnManager _spawnManager;

    bool _isSpeedBoostActive = false;
    bool _isTripleShotActive = false;
    bool _isShieldActive = false;
    bool _isBeamLaserActive = false;    
    bool _isStunActive = false;              //stun powerup (more like a power down :p)

    [SerializeField]
    int _score;

    UIManager _uiManager;

    AudioSource _audioSrc;
    [SerializeField]
    AudioClip _laserSound, _outOfAmmo;

    //ammo count mechanic
    int _ammoAvailable = 15;

    //for laser beam
    [SerializeField]
    GameObject _laserBeamObject;

    //thrusting
    public bool _canThrust = true;

    void Start()
    {
        //positioning the position of the cube in the centre
        //taking the current position and setting it to a new position of (0, 0, 0)
        //transform.position = new Vector3(0, 0, 0);
        _laserSpawnOffset = new Vector3(0, 1f, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSrc = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager cannot be referenced!");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager cannot be referenced!");
        }
        if (_audioSrc == null)
        {
            Debug.LogError("Audio Source on player cannot be referenced!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isStunActive)
        {
            PlayerMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoAvailable > 0 && !_isBeamLaserActive)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoAvailable < 1 && !_isBeamLaserActive)
        {
            _audioSrc.PlayOneShot(_outOfAmmo);
        }
        //print(_canThrust);
    }
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //adding thruster boost if left shift pressed
        if (Input.GetKey(KeyCode.LeftShift) && _canThrust)
        {
            transform.Translate(direction * (_speed * _thrusterModifier) * Time.deltaTime);
            _uiManager.UseThrust(100f);
            StartCoroutine(DeactivateThrusting());
        }
        else
        {
            //normal movement code
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        #region vertical bounds method1
        //an if statement is fairly straightforward, here we need to check if the player position is inside 
        //the given limits, if not, we need to set it to the limits
        //if y > 9 then y should be 9 and if y < -9 then y should be -9
        //NOTE: here 5.5f can be and should be replaced by variables for upper screen bounds
        //      all screen bounds should be turned into variables

        /*
        if (transform.position.y > 6f)
        {
            //here you cannot just set only y position
            //you need to set the x and z positions as well
            //so we create a new vector like this (original x, 0, original z)
            transform.position = new Vector3(transform.position.x, 6f, transform.position.z);
        }
        //use else if for checking the same parameter for different condition
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, transform.position.z);
        }
        */
        #endregion

        #region vertical bounds method2

        //here, instead of an if statement check, the y value is simply clamped to the 
        //upper and lower screen bounds using Mathf.Clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0f), transform.position.z);

        #endregion

        //different if statement for a different parameter
        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, transform.position.z);
        }
    }

    void Shoot()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else 
        {
            Instantiate(_laserPrefab, transform.position + _laserSpawnOffset, Quaternion.identity);
        }

        _audioSrc.PlayOneShot(_laserSound);

        if (_ammoAvailable != 0)
        {
            _ammoAvailable -= 1;
        }
        
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldStrength -= 1;

            if (_shieldStrength == 2)
            {
                _shieldObject.SetActive(false);
                _shield50.SetActive(true);
            }
            else if (_shieldStrength == 1)
            {
                _shield25.SetActive(true);
                _shield50.SetActive(false);
            }

            if (_shieldStrength < 1)
            {
                _shield25.SetActive(false);
                _isShieldActive = false;

                CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, .5f);

                return;
            }

            CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, .5f);

            return;
        }

        _lives -= 1;

        UpdateWings();

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            _spawnManager.WhenPlayerDies();
            Destroy(this.gameObject);
        }

        CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, .5f);
    }

    void UpdateWings()
    {
        if (_lives == 2)
        {
            _lEngine.SetActive(true);
            _rEngine.SetActive(false);
        }
        else if (_lives == 1)
        {
            _rEngine.SetActive(true);
        }
        else if (_lives == 3)
        {
            _lEngine.SetActive(false);
            _rEngine.SetActive(false);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(DeactivateTripleShot());
    }

    IEnumerator DeactivateTripleShot()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speed = _boostSpeed;
        StartCoroutine(DeactivateSpeedBoost());
    }

    IEnumerator DeactivateSpeedBoost()
    {
        yield return new WaitForSeconds(5f);
        _speed = _defaultSpeed;
        _isSpeedBoostActive = false;
    }

    public void ActivateStun()
    {
        _isStunActive = true;
        CameraShaker.Instance.ShakeOnce(4f, 1f, 2f, 1f);
        StartCoroutine(DeactivateStun());
    }

    IEnumerator DeactivateStun()
    {
        yield return new WaitForSeconds(2f);
        _isStunActive = false;
    }

    public void ActivateShields()
    {
        _shieldObject.SetActive(true);
        _isShieldActive = true;
        _shieldStrength = 3;
    }

    public void ActivateAmmoRefill()
    {
        _ammoAvailable = 15;
    }

    public void ActivateHealthPickup()
    {
        int _totalLives = 3;
        if (_lives < _totalLives)
        {
            _lives += 1;
            _uiManager.UpdateLivesDisplay(_lives);
            UpdateWings();
        }
    }
    public void ActivateBeamLaser()
    {
        _isBeamLaserActive = true;
        _laserBeamObject.SetActive(true);
        StartCoroutine(DeactivateBeamLaser());
    }

    IEnumerator DeactivateBeamLaser()
    {
        yield return new WaitForSeconds(5f);
        _isBeamLaserActive = false;
        _laserBeamObject.SetActive(false);
    }

    IEnumerator DeactivateThrusting()
    {
        yield return new WaitForSeconds(1f);
        _canThrust = false;
        _uiManager.StartThrustRegen();
        StartCoroutine(ActivateThrusting());
    }

    IEnumerator ActivateThrusting()
    {
        yield return new WaitForSeconds(2f);
        _canThrust = true;
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetAmmoCount()
    {
        return _ammoAvailable;
    }
}
