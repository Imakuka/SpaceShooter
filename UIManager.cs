using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _outOfAmmoText;
    [SerializeField]
    private Text _deathRocketActiveText;

    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _playerRestart;
    private GameManager _gameManager;
    private bool _isOutOfAmmo = false;
    private bool _isDeathRocketActive = false;


    [SerializeField]
    private Image _thrusterSpriteImg;
    [SerializeField]
    private Sprite[] _thrusterSpritesArray;



    private Player _player;

 

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCountText.text = "Ammo Count: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _outOfAmmoText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        GetComponent<SpriteRenderer>();


        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    public void OutOfAmmo()
    {
        _isOutOfAmmo = true;
        _outOfAmmoText.gameObject.SetActive(true);
        _outOfAmmoText.text = "Ammo Is Out!!!";
        StartCoroutine(OutOfAmmoFlickerRoutine());
    }
    IEnumerator OutOfAmmoFlickerRoutine()
    {

        while (_isOutOfAmmo == true)
        {
            _outOfAmmoText.text = "Ammo Is Out!!!";
            yield return new WaitForSeconds(0.5f);
            _outOfAmmoText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DeathRocketText()
    {
        _isDeathRocketActive = true;
        StartCoroutine(DeathRocketTextRoutine());

    }

    IEnumerator DeathRocketTextRoutine()
    {
        while (_isDeathRocketActive == true)
        {
            _deathRocketActiveText.gameObject.SetActive(true);
            _deathRocketActiveText.text = "DEATH ROCKET ACTIVE PRESS 'Left CNTRL' to FIRE!";
            yield return new WaitForSeconds(5.5f);
        }
    }

    public void DeathRocketTextOff()
    {
        _isDeathRocketActive = false;
        _deathRocketActiveText.gameObject.SetActive(false);
    }

    public void AmmoNotOut()
    {
        _isOutOfAmmo = false;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmoCount(int currentAmmo)
    {
        _ammoCountText.text = "Ammo Count: " + currentAmmo.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            _playerRestart.text = "PRESS THE 'R' KEY TO GO TO THE MAIN MENU";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            _playerRestart.text = "";
            yield return new WaitForSeconds(0.5f);
        }

    }


}
