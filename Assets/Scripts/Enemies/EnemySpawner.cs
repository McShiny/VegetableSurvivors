using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Transform enemy;
    
    private float enemySpawnTime;
    private float enemySpawnTimeMax = 1.5f;
    private float enemySpawnMinRadius = 5f;
    private float enemySpawnMaxRadius = 30f;

    private void Update() {
        if (enemySpawnTime >= enemySpawnTimeMax) {
            SpawnEnemy();
            enemySpawnTime = 0f;
        }
        enemySpawnTime += Time.deltaTime;
    }

    private void SpawnEnemy() {
        
        Instantiate(enemy, GenerateSpawnPosition(), Quaternion.identity);

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

}
