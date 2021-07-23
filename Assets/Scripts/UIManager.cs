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
    Image barImage;
    [SerializeField]
    Slider _thrustBar;
    float _minThrust = 0;
    float _maxThrust = 100;
    float _currentThrust;
    float _thrustRegenAmount = 100f;


    // Start is called before the first frame update
    void Start()
    {
         player = GameObject.Find("Player").GetComponent<Player>();
        _currentThrust = _maxThrust;
        _thrustBar.minValue = _minThrust;
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
        _currentThrust -= amount * Time.deltaTime;
        _thrustBar.value = _currentThrust;
        if (_thrustBar.minValue != 0)
        {
            _thrustBar.value = 0;
        }
    }

    public void StartThrustRegen()
    {
        //_currentThrust += _thrustRegenAmount * Time.deltaTime;
        //_thrustBar.value = _currentThrust;
        StartCoroutine(RegenThrust());
    }

    IEnumerator RegenThrust()
    {
        yield return new WaitForSeconds(1);
        _currentThrust += _thrustRegenAmount * Time.deltaTime;
        _thrustBar.value = _currentThrust;
        if (player._canThrust == true && barImage.fillAmount != 1)
        {
            barImage.fillAmount = 1;
        }
    }

}
