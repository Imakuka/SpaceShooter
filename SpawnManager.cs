using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject [] powerups;
    [SerializeField]
    private GameObject _ammoReload;
    private bool _isAmmoOut = false;
    private bool _isHealthLow = false;
    [SerializeField]
    private GameObject _playerHealth;
    [SerializeField]
    private GameObject _deathRocketPowerUp;



    private bool _stopSpawning = false;


    // Update is called once per frame
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(DeathRocketPowerUpRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.1f), 6.1f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
        
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.1f), 6.1f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }

    }

    IEnumerator AmmoReloadRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (_isAmmoOut == true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.1f), 6.1f, 0);
            Instantiate(_ammoReload, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
            AmmoNotOut();
        }

    }
    IEnumerator HealthReloadRoutine()
    {
        yield return new WaitForSeconds(Random.Range(10.0f, 20.0f));

        while (_isHealthLow == true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.1f), 6.1f, 0);
            Instantiate(_playerHealth, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }

    }
    IEnumerator DeathRocketPowerUpRoutine()
    {
        yield return new WaitForSeconds(20.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.8f, 10.8f), 6.1f, 0);
            Instantiate(_deathRocketPowerUp, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20.0f, 30.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    public void AmmoOut()
    {
        _isAmmoOut = true;
        StartCoroutine(AmmoReloadRoutine());
    }
    public void AmmoNotOut()
    {
        _isAmmoOut = false;
    }

    public void PlayerNeedsLives()
    {
        _isHealthLow = true;
        StartCoroutine(HealthReloadRoutine());
    }
    public void PlayerDoesNotNeedLives()
    {
        _isHealthLow = false;
    }



}
