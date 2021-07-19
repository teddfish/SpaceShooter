using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int _lives = 3;

    [SerializeField]
    GameObject _shieldObject;

    [SerializeField]
    GameObject  _lEngine, _rEngine;

    SpawnManager _spawnManager;

    bool _isSpeedBoostActive = false;
    bool _isTripleShotActive = false;
    bool _isShieldActive = false;

    [SerializeField]
    int _score;

    UIManager _uiManager;

    AudioSource _audioSrc;
    [SerializeField]
    AudioClip _laserSound;

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
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //adding thruster boost if left shift pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * (_speed * _thrusterModifier) * Time.deltaTime);
        }
        else
        {
            //normal movement code
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        //checking if git still works

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
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldObject.SetActive(false);
            _isShieldActive = false;
            return;
        }

        _lives -= 1;

        DamageWings();

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            _spawnManager.WhenPlayerDies();
            Destroy(this.gameObject);
        }
    }

    void DamageWings()
    {
        if (_lives == 2)
        {
            _lEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rEngine.SetActive(true);
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

    public void ActivateShields()
    {
        _shieldObject.SetActive(true);
        _isShieldActive = true;
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public int GetScore()
    {
        return _score;
    }
}
