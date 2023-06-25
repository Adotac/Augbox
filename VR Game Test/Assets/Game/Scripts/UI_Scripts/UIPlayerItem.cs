using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

namespace Augbox
{
    public class UIPlayerItem : MonoBehaviour
    {
        [Tooltip("UI text item for name of player")]
        [SerializeField]
        private Text textPlayerName;

        [Tooltip("UI button to allow kick of user")]
        [SerializeField]
        private Button buttonKick;

        private Player player;

        private void Awake() {
            buttonKick.onClick.AddListener(() => {
                KickPlayer();
            });
        }

        public void SetKickPlayerButtonVisible(bool visible) {
            buttonKick.gameObject.SetActive(visible);
        }

        public void UpdatePlayer(Player player) {
            this.player = player;
            textPlayerName.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        }

        private void KickPlayer() {
            if (player != null) {
                LobbyManager.Instance.KickPlayer(player.Id);
            }
        }

    }
}
