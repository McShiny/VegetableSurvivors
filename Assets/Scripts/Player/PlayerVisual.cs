using UnityEngine;
using System.Collections.Generic;

public class PlayerVisual : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private List<GameObject> playerVisualList;
    [SerializeField] private List<GameObject> playerDamagedVisualList;

    private float damageVisualTime = 0.5f;
    private bool startDamageTime = false;
    private int index = 0;

    private void Start() {
        player.OnPlayerDamaged += Player_OnPlayerDamaged;
        player.OnStateChanged += Player_OnStateChanged;

        foreach (GameObject go in playerVisualList) {
            Hide(go);
        }

        foreach(GameObject go in playerDamagedVisualList) {
            Hide(go);
        }

        Show(playerVisualList[index]);
    }

    private void Player_OnStateChanged(object sender, System.EventArgs e) {
        foreach (GameObject go in playerVisualList) {
            Hide(go);
        }

        if (player.GetPlayerState() == Player.State.Baby) {
            index = 0;
        }
        if (player.GetPlayerState() == Player.State.Teen) {
            index = 1;
        }
        if (player.GetPlayerState() == Player.State.Adult) {
            index = 2;
        }
        if (player.GetPlayerState() == Player.State.Elder) {
            index = 3;
        }

        Show(playerVisualList[index]);
    }

    private void Player_OnPlayerDamaged(object sender, System.EventArgs e) {
        Show(playerDamagedVisualList[index]);
        startDamageTime = true;
    }

    private void Update() {

        if (startDamageTime) {
            if (damageVisualTime <= 0f) {
                Hide(playerDamagedVisualList[index]);
                startDamageTime = false;
                damageVisualTime = 0.5f;
            }
            damageVisualTime -= Time.deltaTime;
        }

        if(!startDamageTime) {
            foreach (GameObject go in playerDamagedVisualList) {
                if (go != null) {
                    Hide(go);
                }
            }
        }
    }

    private void Hide(GameObject visual) {
        visual.SetActive(false);
    }

    private void Show(GameObject visual) {
        visual.SetActive(true);
    }

}
