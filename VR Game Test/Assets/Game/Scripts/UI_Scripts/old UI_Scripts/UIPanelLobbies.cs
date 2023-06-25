
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Services.Lobbies.Models; 

// namespace Augbox
// {
//     public class UIPanelLobbies : UIPanel<UIPanelLobbies>, IUIPanel
//     {
//         [Tooltip("Controller for list of lobbies")]
//         // [SerializeField] private UIScrollViewLobbies lobbies; 

//         [SerializeField] private UILobbyItem _lobbyPanelPrefab;
//         [SerializeField] private Transform _lobbyParent;
//         [SerializeField] private GameObject _noLobbiesText;
//         [SerializeField] private float _lobbyRefreshRate = 2;
//         private readonly List<UILobbyItem> _currentLobbySpawns = new();
//         private float _nextRefreshTime;

//         private void Awake() {
            
//         }

//         private void Update()
//         {
//         }

//         public void Back()
//         {
//             UIPanelManager.Instance.HidePanel<UIPanelLobbies>(false);
//         }

//         private async void FetchLobbies() {
//             try {
//                 _nextRefreshTime = Time.time + _lobbyRefreshRate;

//                 // Grab all current lobbies
//                 var allLobbies = await MatchmakingService.GatherLobbies();

//                 // Destroy all the current lobby panels which don't exist anymore.
//                 // Exclude our own homes as it'll show for a brief moment after closing the room
//                 var lobbyIds = allLobbies.Where(l => l.HostId != Authentication.PlayerId).Select(l => l.Id);
//                 var notActive = _currentLobbySpawns.Where(l => !lobbyIds.Contains(l.Lobby.Id)).ToList();

//                 foreach (var panel in notActive) {
//                     Destroy(panel.gameObject);
//                     _currentLobbySpawns.Remove(panel);
//                 }

//                 // Update or spawn the remaining active lobbies
//                 foreach (var lobby in allLobbies) {
//                     var current = _currentLobbySpawns.FirstOrDefault(p => p.Lobby.Id == lobby.Id);
//                     if (current != null) {
//                         current.UpdateDetails(lobby);
//                     }
//                     else {
//                         var panel = Instantiate(_lobbyPanelPrefab, _lobbyParent);
//                         panel.Init(lobby);
//                         _currentLobbySpawns.Add(panel);
//                     }
//                 }

//                 _noLobbiesText.SetActive(!_currentLobbySpawns.Any());

//                 // there is a brief windows after the host leaves the lobby that it is left open
//                 // we check for this state as the owner will be null and the AvailableSlots will be equal to max slots
//                 // if (outLobbyDetailsInfo.Value.LobbyOwnerUserId != null && outLobbyDetailsInfo.Value.AvailableSlots != outLobbyDetailsInfo.Value.MaxMembers)
//                 // {
//                 //     var lobbyItem = lobbies.AddLobby(outLobbyDetails, outLobbyDetailsInfo.Value, lobbyName);
//                 //     lobbyItem.JoinRequest += LobbyItem_JoinRequest;
//                 // }
//             }
//             catch (Exception e) {
//                 Debug.LogError(e);
//             }
//         }



//         protected override void OnShowing()
//         {
//         }
//     }
// }
