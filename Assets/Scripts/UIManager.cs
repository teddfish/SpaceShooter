using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _scoreText, _gameOverText, _restartText;
    [SerializeField]
    Image _livesDisplayImage;
    [SerializeField]
    Sprite[] _livesSprites;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
         player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            _scoreText.text = "Score: " + player.GetScore();
        }
        if (_restartText.IsActive() && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UpdateLivesDisplay(int livesCounter)
    {
        _livesDisplayImage.sprite = _livesSprites[livesCounter];

        if (livesCounter == 0)
        {
            //this handles everything that happens after game over
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(StartFlicker());
        }
    }



    IEnumerator StartFlicker()
    {
        yield return new WaitForSeconds(0.5f);
        _gameOverText.gameObject.SetActive(false);
        StartCoroutine(StopFlicker());
    }

    IEnumerator StopFlicker()
    {
        yield return new WaitForSeconds(0.5f);
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(StartFlicker());
    }
}
