using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour {

    

    [SerializeField] private Button damageButton;
    [SerializeField] private Button healthButton;

    private void Awake() {

        damageButton.onClick.AddListener(() => {
            Upgrade.Instance.DamageUpgradeInvoke();
            VegetableGameManager.Instance.TogglePauseGameUpgrade();
            Hide();
        });
        healthButton.onClick.AddListener(() => {
            Upgrade.Instance.HealthUpgradeInvoke();
            VegetableGameManager.Instance.TogglePauseGameUpgrade();
            Hide();
        });
    }

    private void Start() {

        Hide();
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
