using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.ActivateTripleShot();
            }

            Destroy(this.gameObject);
        }
    }
}
