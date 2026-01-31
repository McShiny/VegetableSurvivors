using TMPro;
using UnityEngine;

public class WaveInformationUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    private int enemyCount = 0;

    private void Start() {
        EnemySpawner.Instance.OnWaveChanged += EnemySpawner_OnWaveChanged;
        EnemySpawner.Instance.OnEnemyCountChanged += EnemySpawner_OnEnemyCountChanged;
        CabbageAI.OnCabbageEnemyKilled += CabbageAI_OnCabbageEnemyKilled;
        CucumberAI.OnCucumberEnemyKilled += CucumberAI_OnCucumberEnemyKilled;
        CarrotAI.OnCarrotEnemyKilled += CarrotAI_OnCarrotEnemyKilled;

        waveText.text = "Wave: " + 1;
        enemyCountText.text = "Enemies Alive: " + enemyCount;
    }

    private void EnemySpawner_OnWaveChanged(object sender, EnemySpawner.OnWaveChangedEventArgs e) {
        waveText.text = "Wave: " + e.wave;
    }

    private void CarrotAI_OnCarrotEnemyKilled(object sender, System.EventArgs e) {
        enemyCount -= 1;
        enemyCountText.text = "Enemies Alive: " + enemyCount;
    }

    private void CucumberAI_OnCucumberEnemyKilled(object sender, System.EventArgs e) {
        enemyCount -= 1;
        enemyCountText.text = "Enemies Alive: " + enemyCount;
    }

    private void CabbageAI_OnCabbageEnemyKilled(object sender, System.EventArgs e) {
        enemyCount -= 1;
        enemyCountText.text = "Enemies Alive: " + enemyCount;
    }

    private void EnemySpawner_OnEnemyCountChanged(object sender, System.EventArgs e) {
        enemyCount++;
        enemyCountText.text = "Enemies Alive: " + enemyCount;
    }
}
