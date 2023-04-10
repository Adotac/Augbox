using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using FishNet.Plugins.FishyEOS.Util;
using FishNet.Transporting.FishyEOSPlugin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Augbox
{
    public sealed class Player : NetworkBehaviour
    {
        public static Player Instance { get; private set; }

        [SyncVar(OnChange = nameof(OnPlayerName))]
        [HideInInspector]
        public string PlayerName;

        [SyncVar]
        [HideInInspector]
        public string UserId;
        [SyncVar] public bool isReady;
        //[SyncVar] public Pawn controlledPawn;

        private void OnPlayerName(string prev, string next, bool asServer)
        {
            PlayerManager.Instance.PlayerUpdated(UserId);
        }

        //override fishnet callbacks
        public override void OnStartServer()
        {
            base.OnStartServer();
            
            // GameManager.instance.players.Add(this);
            var fishy = InstanceFinder.NetworkManager.GetComponent<FishyEOS>();
            UserId = fishy.GetRemoteConnectionAddress(Owner.ClientId);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            // if (!IsOwner) return;

            // Instance = this;

            if (IsOwner)
            {
                SetPlayerName(GameSettings.Instance.CurrentPlayerName);
            }

            PlayerManager.Instance.AddPlayer(this);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            // we have dropped a connection - try to leave the EOS lobby we were in - we might have already left it
            // in which case this will just warn it cannot leave
            CleanUpEOS();
        }

        private void CleanUpEOS()
        {
            if (IsOwner)
            {
                if (EOS.GetCachedLobbyInterface() != null)
                {
                    var updateLobbyModificationOptions = new LeaveLobbyOptions { LocalUserId = ProductUserId.FromString(UserId), LobbyId = PlayerManager.Instance.ActiveLobbyId };

                    // as we have a bit of a race condition depending whether the server kicks or we leave
                    // just report the failure as a normal log
                    EOS.GetCachedLobbyInterface().LeaveLobby(ref updateLobbyModificationOptions, null, delegate (ref LeaveLobbyCallbackInfo data)
                    {
                        if (data.ResultCode != Result.Success)
                        {
                            Debug.Log($"User {UserId} failed to leave EOS lobby: {data.ResultCode}");
                        }
                        else
                        {
                            Debug.Log($"User {UserId} left EOS lobby");
                        }
                    });
                }

            }
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            // GameManager.instance.players.Remove(this);
             // extra check here as this could occur when we are shutting down the game also
            if (EOS.GetCachedLobbyInterface() != null)
            {
                // fallback in case user has timed out or some such

                var kickMemberOptions = new KickMemberOptions { TargetUserId = ProductUserId.FromString(UserId), LocalUserId = EOS.LocalProductUserId, LobbyId = PlayerManager.Instance.ActiveLobbyId };

                EOS.GetCachedLobbyInterface().KickMember(ref kickMemberOptions, null, delegate (ref KickMemberCallbackInfo data)
                {
                    // as we have a bit of a race condition depending whether the server kicks or we leave
                    // just report the failure as a normal log
                    if (data.ResultCode != Result.Success)
                    {
                        Debug.Log($"User {UserId} failed to kick from EOS lobby: {data.ResultCode}");
                    }
                    else
                    {
                        Debug.Log($"User {UserId} kicked from EOS lobby");
                    }
                });
            }
        }

        

        [Client]
        private void Update()
        {
            if (!IsOwner)
            { //check if this is the owner of the object in the client or host side
                Debug.Log($"IsOwner: {IsOwner}");
                return;
            }

            //if (Input.GetKeyDown(KeyCode.R)) {
            //    ServerSetIsReady(!isReady);
            //    Debug.Log($"Ready: {isReady}"); 
            //}

            if (Input.GetKeyDown(KeyCode.T))
            {
                ServerSpawnPawn();
                Debug.Log("Should Spawn");
            }

        }

        [ServerRpc(RequireOwnership = false)]//this is a field that allows to execute the code on the serverside
        private void ServerSetIsReady(bool val) {
            isReady = val;   
        }

        [ServerRpc]

        private void ServerSpawnPawn()
        {
            GameObject pawnPrefab = Addressables.LoadAssetAsync<GameObject>("Pawn").WaitForCompletion();

            GameObject pawnInstance = Instantiate(pawnPrefab);

            Spawn(pawnInstance, Owner); //assign the instance to the owner
        }

                public override void OnStopNetwork()
        {
            base.OnStopNetwork();

            PlayerManager.Instance?.RemovePlayer(UserId);
        }

        [ServerRpc]
        private void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        public void SendStartingGame()
        {
            if (IsServer)
            {
                DoStartingGame();
            }
        }

        [ObserversRpc]
        private void DoStartingGame()
        {
            // ...
        }
    }
}