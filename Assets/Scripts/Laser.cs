using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable of 8
    [SerializeField]
    float _laserSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //translate the laser upwards
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        //if laser position is greater than 8
        //destroy game object
        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
