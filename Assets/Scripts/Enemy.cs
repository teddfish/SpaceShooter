using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * 4 * Time.deltaTime);

        if (transform.position.y < -11f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8f, transform.position.z);
        }
    }
}
