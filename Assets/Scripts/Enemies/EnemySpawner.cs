    using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner Instance { get; private set; }

    public event EventHandler<OnWaveChangedEventArgs> OnWaveChanged;
    public class OnWaveChangedEventArgs : EventArgs {
        public int wave;
    }

    public event EventHandler OnEnemyCountChanged;

    [SerializeField] private List<Transform> enemies;
    
    private float enemySpawnTime;
    private float enemySpawnTimeMax = 1f;
    private float enemySpawnMinRadius = 5f;
    private float enemySpawnMaxRadius = 50f;

    private int wave = 1;
    private bool waveInactive = false;
    private bool waveCleared = false;
    private int enemiesAlive = 0;
    private float pauseBetweenWaves = 5f;
    private int waveMaxEnemies;
    private int spawnedEnemies = 0;


    private void Awake() {
        Instance = this;

        waveMaxEnemies = GenerateNumEnemiesSpawned();
    }

    private void Start() {
        CabbageAI.OnCabbageEnemyKilled += CabbageAI_OnCabbageEnemyKilled;
        CarrotAI.OnCarrotEnemyKilled += CarrotAI_OnCarrotEnemyKilled;
        CucumberAI.OnCucumberEnemyKilled += CucumberAI_OnCucumberEnemyKilled;
    }

    private void CucumberAI_OnCucumberEnemyKilled(object sender, EventArgs e) {
        enemiesAlive--;
    }

    private void CarrotAI_OnCarrotEnemyKilled(object sender, EventArgs e) {
        enemiesAlive--;
    }

    private void CabbageAI_OnCabbageEnemyKilled(object sender, EventArgs e) {
        enemiesAlive--;
    }

    private void Update() {
        if ((enemySpawnTime >= enemySpawnTimeMax) && (spawnedEnemies < waveMaxEnemies) && !waveInactive) {
            SpawnEnemy();
            enemySpawnTime = 0f;
        } else if ((spawnedEnemies < waveMaxEnemies) && !waveInactive) {
            enemySpawnTime += Time.deltaTime;
        } else if (waveCleared) {
            waveInactive = true;
        }
        
        if (enemiesAlive <= 0 && spawnedEnemies >= waveMaxEnemies) {
            waveCleared = true;
        }

        if (waveInactive) {
            if(pauseBetweenWaves <= 0) {
                waveInactive = false;
                wave++;

                OnWaveChanged?.Invoke(this, new OnWaveChangedEventArgs {
                    wave = wave,
                });

                waveMaxEnemies = GenerateNumEnemiesSpawned();

                waveCleared = false;
                spawnedEnemies = 0;
                enemySpawnTime = 0f;
                pauseBetweenWaves = 5f;
            }
            pauseBetweenWaves -= Time.deltaTime;
        }

    }
    
    private void SpawnEnemy() {
        if (VegetableGameManager.Instance.IsGamePlaying()) {
            Instantiate(enemies[Random.Range(0, enemies.Count)], GenerateSpawnPosition(), Quaternion.identity);

            spawnedEnemies++;
            enemiesAlive++;

            OnEnemyCountChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private Vector3 GenerateSpawnPosition() {
        Vector3 spawnPosition = Vector3.zero;

        do {
            spawnPosition = new Vector3(Random.Range(-120,120),Random.Range(-120, 120), 0);

        } while (!(Mathf.Abs(Player.Instance.transform.position.y - spawnPosition.y) >= enemySpawnMinRadius &&
        Mathf.Abs(Player.Instance.transform.position.y - spawnPosition.y) <= enemySpawnMaxRadius &&
        Mathf.Abs(Player.Instance.transform.position.x - spawnPosition.x) >= enemySpawnMinRadius &&
        Mathf.Abs(Player.Instance.transform.position.x - spawnPosition.x) <= enemySpawnMaxRadius));

        return spawnPosition;
    }

    private int GenerateNumEnemiesSpawned() {
        return (int) Mathf.Round(0.5f * Mathf.Pow((float)System.Math.E, wave)) + 5;
    }

}
