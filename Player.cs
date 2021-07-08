using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public static event Action<bool, GameObject> onFireLaser;
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _fastSpeed = 5.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private float _speedBoost = 1.5f;

    private bool _isAmmoOut = false;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isFastSpeedActive = false;

    [SerializeField]
    private GameObject _playerShield;
    private bool _isShieldBoostActive = false;
    [SerializeField]
    private int _shieldLives;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;
 

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;


    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private bool _isHealthPowerUpActive = false;

    [SerializeField]
    private GameObject _deathRocket;
    private float _deathRocketCanFire = -1f;
    private float _deathRocketFireRate = 0.5f;
    [SerializeField]
    private bool _isDeathRocketActive = false;

    //Thrusters
    [SerializeField]
    private bool _areThrustersActive = false;



    // Start is called before the first frame update
    void Start()
    {
 
        transform.position = new Vector3(0, -3.0f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager.UpdateAmmoCount(_ammoCount);
        _shieldLives = 0;

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the PLayer is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (_lives == 3)
        {
            _spawnManager.PlayerDoesNotNeedLives();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            PlayerLaser(true, _laserPrefab);
            FireLaser();
        }


        if (_isDeathRocketActive == true && (Input.GetKeyDown(KeyCode.LeftControl) && Time.time > _deathRocketCanFire))
        {
            Instantiate(_deathRocket, transform.position, Quaternion.identity);
        }
        if (_areThrustersActive == true)
        {
            GoFast();
        }
        else
        {
            FastSpeedNotActive();
        }

    }

    private void PlayerLaser(bool canFire, GameObject laser)
    {
        if (canFire == true)
        {
            if (onFireLaser != null)
            {
                onFireLaser(true, laser);
            }
        }
    }

    public void DeathRocketPowerUpOn()
    {
        _deathRocketCanFire = Time.time + _deathRocketFireRate;
        _isDeathRocketActive = true;
        StartCoroutine(LasersOffCoroutine());
        StartCoroutine(DeathRocketPowerDownRoutine());
    }

    IEnumerator DeathRocketPowerDownRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        _isDeathRocketActive = false;
        _uiManager.DeathRocketTextOff();
    }

    IEnumerator LasersOffCoroutine()
    {
        while (_isDeathRocketActive == true)
        {
            _canFire = 6000000000.0f;
            yield return new WaitForSeconds(5.0f);
            _canFire = -1f;
        }
    }

    void FireLaser()
    {

        if (_ammoCount >= 1)
        {
            _uiManager.AmmoNotOut();
            _isAmmoOut = false;
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                PlayerAmmo();
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.43f, 0), Quaternion.identity);
                PlayerAmmo();
            }
            _audioSource.Play();

        }

    }

    void PlayerAmmo()
    {
        _ammoCount--;

        if (_ammoCount < 1 && _isAmmoOut == false)
        {
            _spawnManager.AmmoOut();
            _uiManager.OutOfAmmo();
        }
        _uiManager.UpdateAmmoCount(_ammoCount);
        _isAmmoOut = false;
    }
    
    public void ReloadPlayerAmmo()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    void GoFast()
    {
        FastSpeedActive();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _fastSpeed;
            StartCoroutine(FastSpeedCoolDown());
        }

    }
    void FastSpeedActive()
    {
        _areThrustersActive = true;
        _isFastSpeedActive = true;
    }
    void FastSpeedNotActive()
    {

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 4.0f;
            _isFastSpeedActive = false;
        }
    }

    IEnumerator FastSpeedCoolDown()
    {
        
        while (_areThrustersActive == true)
            
        {
            yield return new WaitForSeconds(5.0f);
            _areThrustersActive = false;
            FastSpeedNotActive();

        }
        
    }

    void CalculateMovement ()
    { 
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);


        if (_isFastSpeedActive == true)
        {
            transform.Translate(direction * _fastSpeed * Time.deltaTime);
        }

        if (_isSpeedBoostActive == true)
        {
            transform.Translate(direction * (_speed * _speedBoost) * Time.deltaTime);
        }

        if (transform.position.y >= 4)
        {
            transform.position = new Vector3(transform.position.x, 4, 0);
        }
        else if (transform.position.y <= -5.5f)
        {
            transform.position = new Vector3(transform.position.x, -5.5f, 0);
        }

        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    public void ShieldDamage()
    {
        _shieldLives--;

        if (_shieldLives == 2)
        {
            StartCoroutine(ShieldFlickerRoutine());
        }
        else if (_shieldLives == 1)
        {
            StartCoroutine(FastShieldFlickerRoutine());
        }
        if (_shieldLives < 1)
        {
            _isShieldBoostActive = false;
            _playerShield.SetActive(false);
            return;
        }

    }
    IEnumerator ShieldFlickerRoutine()
    {
        while (_shieldLives == 2)
        {
            _playerShield.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _playerShield.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }
    IEnumerator FastShieldFlickerRoutine()
    {
        while (_shieldLives == 1)
        {
            _playerShield.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _playerShield.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Damage()
    {

        if (_isShieldBoostActive == true)
        {
            ShieldDamage();
        }
        else
        {
            _lives--;
            if (_lives == 2)
            {
                PlayerHealth();
                _rightEngine.SetActive(true);
            }

            else if (_lives == 1)
            {
                PlayerHealth();
                _leftEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);
        }
    
        if (_lives < 1)
        {
            _isHealthPowerUpActive = false;
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    void PlayerHealth()
    {
        HealthPowerUpActive();
        if (_lives < 3 && _isHealthPowerUpActive == true)
        {
            _spawnManager.PlayerNeedsLives();
        }
    }
    public void AddHealth()
    {
        if(_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            if (_lives == 3)
            {
                _rightEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _leftEngine.SetActive(false);
            }
        }

    }


    public void HealthPowerUpActive()
    {
        _isHealthPowerUpActive = true;
    }
    
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        _speed *= _speedBoost;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedBoost;
    }

    public void ShieldBoostActive()
    {
        _isShieldBoostActive = true;
        _playerShield.SetActive(true);
        _shieldLives = 3;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


}

