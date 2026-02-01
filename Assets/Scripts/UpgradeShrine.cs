using UnityEngine;

public class UpgradeShrine : MonoBehaviour
{

    private bool isActive = false;

    private void Start() {
        EnemySpawner.Instance.OnWaveChanged += EnemySpawner_OnWaveChanged;
        Player.Instance.OnGotUpgrade += Player_OnGotUpgrade;
    }

    private void Player_OnGotUpgrade(object sender, System.EventArgs e) {
        isActive = false;
    }

    private void EnemySpawner_OnWaveChanged(object sender, EnemySpawner.OnWaveChangedEventArgs e) {
        isActive = true;
    }

    public bool IsShrineActive() {
        return isActive;
    }
}
