using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //declaring a speed variable to give the movement speed to the player object
    [SerializeField]
    float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //positioning the position of the cube in the centre
        //taking the current position and setting it to a new position of (0, 0, 0)
        //transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //here we get inputs from the player for the horizontal axis, 1 for right and -1 for left
        float horizontalInput = Input.GetAxis("Horizontal");
        //here we get inputs from player for the vertical axis, 1 for up and -1 for down
        float verticalInput = Input.GetAxis("Vertical");

        //this vector uses our horizontalInput for the x value, verticalInput for the y value and 0 for z value
        //this will give us all our WASD inputs in one single vector3 
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //translating the position of the cube by multiplying the direction vector with the movement speed variable 
        //and by real time multiplier value
        //Time.deltaTime is the time taken by the computer to render the previous frame
        transform.Translate(direction * speed * Time.deltaTime);

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
}
