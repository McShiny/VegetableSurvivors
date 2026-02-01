using UnityEngine;

public class UpgradeShrine : MonoBehaviour
{

    public static UpgradeShrine Instance { get; private set; }

    private bool isActive = false;
    private bool playerClamed = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        EnemySpawner.Instance.OnWaveCleared += EnemySpawner_OnWaveCleared;
        EnemySpawner.Instance.OnWaveChanged += EnemySpawner_OnWaveChanged;
        Player.Instance.OnGotUpgrade += Player_OnGotUpgrade;
    }

    private void EnemySpawner_OnWaveChanged(object sender, EnemySpawner.OnWaveChangedEventArgs e) {
        playerClamed = false;
    }

    private void EnemySpawner_OnWaveCleared(object sender, EnemySpawner.OnWaveClearedEventArgs e) {
        if (!playerClamed) {
            isActive = true;
        }

    }

    private void Player_OnGotUpgrade(object sender, System.EventArgs e) {
        isActive = false;
    }


    public bool IsShrineActive() {
        return isActive;
    }

    public void SetShrineInactive() {
        isActive = false;
    }
}
