using System;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public static Upgrade Instance { get; private set; }

    public event EventHandler OnHealthUpgrade;
    public event EventHandler OnDamageUpgrade;

    [SerializeField] private GameObject upgradeUI;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnGotUpgrade += Player_OnGotUpgrade;
    }

    private void Player_OnGotUpgrade(object sender, EventArgs e) {
        VegetableGameManager.Instance.TogglePauseGameUpgrade();
        upgradeUI.SetActive(true);
        UpgradeShrine.Instance.SetShrineInactive();

    }

    public void DamageUpgradeInvoke() {
        OnDamageUpgrade?.Invoke(this, EventArgs.Empty);
    }

    public void HealthUpgradeInvoke() {
        
        OnHealthUpgrade?.Invoke(this, EventArgs.Empty);
    }

}
