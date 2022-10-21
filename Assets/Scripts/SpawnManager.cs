using PolygonWar;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public int EnemiesPerWave;
    public Enemys[] Enemys;
}

[System.Serializable]
public class Enemys
{
    //public int EnemyTypes;
    public GameObject Enemy;
}



public class SpawnManager : MonoBehaviour
{
    public Wave[] Waves; // class to hold information per wave
    public Transform[] SpawnPoints;
    public float TimeBetweenEnemies = 0.5f;
    [Header("UI Components")]
    public Text waveCount;
    public Text enemyCount;
    public Slider HealthBar;
    public AudioClip LaughSfxClip;
    public AudioClip NextWaveSfxClip;



    private AudioSource _audioSource;
    private GameManager _gameManager;
    private GameObject _enemyContainer;
    private GameObject _itemsContainer;
    private GameObject _Nuke;



    private int _totalEnemiesInCurrentWave;
    private int _totalEnemieskilled;
    private int _enemiesInWaveLeft;
    private int _spawnedEnemies;

    private int _currentWave;
    private int _totalWaves;

    private bool _isSpawning;
    private bool _isSpawningWave = false;

    Objective m_Objective;

    void Start()
    {
        _itemsContainer = GameObject.Find("Items");
        _Nuke = GameObject.Find("FX_Nuke");
        _Nuke.SetActive(false);



        _gameManager = GetComponentInParent<GameManager>();
        _currentWave = -1; // avoid off by 1
        _totalWaves = Waves.Length - 1; // adjust, because we're using 0 index
        _enemyContainer = new GameObject("Enemy Container");
        _isSpawning = true;
        SetupSound();
        StartNextWave();
    }

    //private void Awake()
    //{
    //    _Nuke.SetActive(false);
    //}

    void StartNextWave()
    {
        _currentWave++;
        _totalEnemieskilled = 0;
   
        // win
        if (_currentWave > _totalWaves)
        {
            _gameManager.Victory();

            return;
        }

        if (_currentWave == _totalWaves)
        {
            _Nuke.SetActive(true);
           // GameObject.Find("FX_Nuke").GetComponent<ParticleSystem>().Play();
            //GameObject.Find("FX_Nuke").GetComponent<AudioSource>().Play();
        }

           
        m_Objective = gameObject.AddComponent<Objective>();
        m_Objective.title = "Sobrevive la oleada "+ (_currentWave+1) + " de " + (_totalWaves + 1);
        m_Objective.description = "Elimina " + Waves[_currentWave].EnemiesPerWave + " enemigos";
        //m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), string.Empty);
        

        _totalEnemiesInCurrentWave = Waves[_currentWave].EnemiesPerWave;
        _enemiesInWaveLeft = 0;
        _spawnedEnemies = 0;

        //Activar todos los items al inicio de cada ronda

        enableAllItems();

        StartCoroutine(SpawnEnemies());
    }

    // Coroutine to spawn all of our enemies
    IEnumerator SpawnEnemies()
    {
        _isSpawningWave = true;
        //int enemyIndex = 0;
        int spawnPointIndex = -1;
        
        while (_spawnedEnemies < _totalEnemiesInCurrentWave)
        {

            if (spawnPointIndex == SpawnPoints.Length - 1)
            {
                spawnPointIndex = -1;

            }
            //print(spawnPointIndex);
            //print(SpawnPoints.Length);
            int enemyIndex = Random.Range(0, Waves[_currentWave].Enemys.Length);
            GameObject enemy = Waves[_currentWave].Enemys[enemyIndex].Enemy;
            _spawnedEnemies++;
            _enemiesInWaveLeft++;
            spawnPointIndex++;

            //int spawnPointIndex = Random.Range(0, SpawnPoints.Length);
            if (_isSpawning)
            {
                // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                GameObject newEnemy = Instantiate(enemy, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
                newEnemy.transform.SetParent(_enemyContainer.transform);
                print("_spawnedEnemies :" + _spawnedEnemies + " de " + _totalEnemiesInCurrentWave);
            }
            yield return new WaitForSeconds(TimeBetweenEnemies);
        }
        _isSpawningWave = false;
        yield return null;
    }

    // called by an enemy when they're defeated
    public void EnemyDefeated()
    {
        _enemiesInWaveLeft--;
        _totalEnemieskilled++;
        //enemyCount.text = "Enemigos matados : " +_totalEnemieskilled+"/" + _totalEnemiesInCurrentWave;
        // We start the next wave once we have spawned and defeated them all




        //if (targetRemaning == 1)
        //{
        //    string notificationText = notificationEnemiesRemainingThreshold >= targetRemaning ? "One enemy left" : string.Empty;
        //    m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
        //}
        //else if (targetRemaning > 1)
        //{
        // create a notification text if needed, if it stays empty, the notification will not be created
        //string notificationText = notificationEnemiesRemainingThreshold >= targetRemaning ? targetRemaning + " enemies to kill left" : string.Empty;
        //string notificationText = "Elimina " + Waves[_currentWave].EnemiesPerWave + " enemigos";
        m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), "");
            //}
        


        if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave)
        {
            _audioSource.clip = NextWaveSfxClip;
            _audioSource.Play();
            m_Objective.CompleteObjective(string.Empty, GetUpdatedCounterAmount(), "Objectivo completo : " + m_Objective.title);
            StartNextWave();
        }
    }

    public void DisableAllEnemies()
    {
        _isSpawning = false;
        // cycle through all of our enemies
        for (int i = 0; i < _enemyContainer.transform.childCount; i++)
        {
            Transform enemy = _enemyContainer.transform.GetChild(i);
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
            NavMeshAgent nav = enemy.GetComponent<NavMeshAgent>();


            // if the enemy is still alive, we want to disable it
            //if (health != null && health.Health > 0 && movement != null)
            //{

            if (health.Health > 0)
            {
                movement.PlayVictory();
                health.enabled = false;
                movement.enabled = false;
                nav.enabled = false;
                
            }
            // }
        }
    }

    public void UpdateWaveEnemies(int amount)
    {
        if (_isSpawningWave)
        {
            _totalEnemiesInCurrentWave += amount;
            string notificationText = "Dispararme tiene sus consecuencias, te aumento en  " + amount + " los enemigos";
            m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            //enemyCount.text = "Enemigos matados : " + _totalEnemieskilled + "/" + _totalEnemiesInCurrentWave;
            _audioSource.clip = LaughSfxClip;
            _audioSource.Play();
        }
           
    }

    private void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.9f;
    }

    string GetUpdatedCounterAmount()
    {
        return _totalEnemieskilled + " / " + _totalEnemiesInCurrentWave;
    }

    void enableAllItems()
    {
        for (int i = 0; i < _itemsContainer.transform.childCount; i++)
        {
            Transform item = _itemsContainer.transform.GetChild(i);
            item.gameObject.SetActive(true);
            print("Activo " + item.gameObject);
        }

    }
}