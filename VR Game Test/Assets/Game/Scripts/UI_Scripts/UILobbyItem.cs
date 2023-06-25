using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

namespace Augbox
{
    public class UILobbyItem : MonoBehaviour
    {
        [Tooltip("UI text item for name of lobby")]
        [SerializeField]
        private Text textLobbyName;

        [Tooltip("UI text item for player count status")]
        [SerializeField]
        private Text textPlayerCount;

        [Tooltip("UI button to allow joining of lobby")]
        [SerializeField]
        private Button buttonJoin;

        private Lobby lobby;

        private void Awake() {
            buttonJoin.onClick.AddListener(() => {
                Join();
            });
        }

        public void UpdateLobby(Lobby lobby) {
            // Lobby = lobby;
            this.lobby = lobby;
            textLobbyName.text = lobby.Name;

            textPlayerCount.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
            buttonJoin.interactable = lobby.AvailableSlots > 0;

        }

        public void Join()
        {
            // LobbySelected?.Invoke(Lobby);
            LobbyManager.Instance.JoinLobby(lobby);
        }
    }
}
