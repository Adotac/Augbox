using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Augbox{
public class GameManager : NetworkBehaviour

{
    [SerializeField] private ControlledPlayer _playerPrefab;
    [SerializeField] private Transform HostSpawn;
    [SerializeField] private Transform ClientSpawn;

    public override void OnNetworkSpawn() {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }   


    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        ControlledPlayer spawn = null;
        if(IsHost){
            print("SPAWNING HOST!!!");
            spawn = Instantiate(_playerPrefab, HostSpawn);
            // _playerPrefab.GetComponentInChildren<Camera>().gameObject.SetActive(IsOwner);
        }
        else if(IsClient){
            print("SPAWNING CLIENT!!!");
            spawn = Instantiate(_playerPrefab, ClientSpawn);
            // _playerPrefab.GetComponentInChildren<Camera>().gameObject.SetActive(IsOwner);
        }

        spawn.NetworkObject.SpawnWithOwnership(playerId);
    }

    public override void OnDestroy() {
        base.OnDestroy();
        LobbyManager.Instance.LeaveLobby();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }
}
}