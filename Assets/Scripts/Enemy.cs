using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _enemySpeed = 4f;

    Player _player;
    Animator _enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player cannot be referenced");
        }
        _enemyAnim = GetComponent<Animator>();
        if (_enemyAnim == null)
        {
            Debug.LogError("Animator cannot be referenced");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -11f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if collides with player
        //damage player
        //destroy enemy
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _enemyAnim.SetTrigger("DestroyEnemy");
            _enemySpeed = 0;
            Destroy(this.gameObject, 2.5f);
        }

        //if collides with laser
        //destroy laser
        //destroy enemy
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _enemyAnim.SetTrigger("DestroyEnemy");
            _enemySpeed = 0;
            Destroy(this.gameObject, 2.5f);
        }
    }
}
