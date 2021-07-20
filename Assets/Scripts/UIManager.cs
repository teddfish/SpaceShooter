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

    bool canRestart = false;

    [SerializeField]
    Slider _thrustBar;
    float _maxThrust = 100;
    float _currentThrust;
    WaitForSeconds _regenTime = new WaitForSeconds(4f);


    // Start is called before the first frame update
    void Start()
    {
         player = GameObject.Find("Player").GetComponent<Player>();
        _currentThrust = _maxThrust;
        _thrustBar.maxValue = _maxThrust;
        _thrustBar.value = _maxThrust;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

    public void UseThrust(float amount)
    {
        if (_currentThrust - amount >= 0)
        {
            _currentThrust -= amount;
            _thrustBar.value = _currentThrust;
        }
    }

    public void StartThrustRegen()
    {
        StartCoroutine(RegenThrust());
    }

    IEnumerator RegenThrust()
    {
        yield return new WaitForSeconds(3.8f);

        while (_currentThrust < _maxThrust)
        {
            _currentThrust += _maxThrust / 100;
            _thrustBar.value = _currentThrust;

            yield return _regenTime;
        }
    }
}
