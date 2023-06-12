using FishNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Augbox
{
    public class PlayerManager : MonoBehaviourSingletonPersistent<PlayerManager>
    {
        private List<Player> _players = new List<Player>();

        // EOS lobby id we are currently in
        public string ActiveLobbyId { get; set; } = "Experimental Lobby";

        // triggered when ever any changes are done to the players
        public event Action PlayersChanged;

        // the player info for the server
        public Player ServerPlayer => _players.FirstOrDefault(x => x.IsServer);

        // the player info for the active client
        public Player ActivePlayer => _players.FirstOrDefault(x => x.IsOwner);

        public List<Player> GetPlayers()
        {
            return _players;
        }

        public void PlayerUpdated(string userId)
        {
            PlayersChanged?.Invoke();
        }

        public void AddPlayer(Player info)
        {
            _players.Add(info);

            PlayersChanged?.Invoke();
        }

        public void RemovePlayer(string userId)
        {
            _players.RemoveAll(x => x.UserId == userId);
            PlayersChanged?.Invoke();
        }
    }
}
