using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //declaring a speed variable to give the movement speed to the player object
    [SerializeField]
    float _speed = 10f;
    [SerializeField]
    GameObject _laserPrefab;
    Vector3 _laserSpawnOffset;
    [SerializeField]
    float _fireRate = 0.2f;
    float _canFire = 0f;

    [SerializeField]
    int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        //positioning the position of the cube in the centre
        //taking the current position and setting it to a new position of (0, 0, 0)
        //transform.position = new Vector3(0, 0, 0);
        _laserSpawnOffset = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Shoot();
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

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
        //if pressed space bar
        //spawn a laser
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _laserSpawnOffset, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives -= 1;

        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
