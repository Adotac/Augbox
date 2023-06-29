using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Augbox{
public class UILobbyRoom : MonoBehaviour
{
    public static UILobbyRoom Instance { get; private set; }

    [Tooltip("Player panel prefab")]
    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;

    [Tooltip("Text item for lobby name")]
    [SerializeField] private Text textLobbyName;

    [Tooltip("Text item for player counter")]
    [SerializeField] private Text playerCountText;

    // [Tooltip("Controller for list of players in lobby")]
    // [SerializeField]
    // private UIScrollViewPlayers players;

    [Tooltip("Button to back out ")]
    [SerializeField]
    private Button leaveLobbyButton;

    [Tooltip("Button to start game - host only")]
    [SerializeField]
    private Button startGameBtn;



    // user given name of room
    public string LobbyName
    {
        get => textLobbyName.text;
        set => textLobbyName.text = value;
    }

    private void Awake() {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
        });

        startGameBtn.onClick.AddListener(async () => {
            await LobbyManager.LockLobby();
            NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = true;
            NetworkManager.Singleton.SceneManager.LoadScene("Scene_Multi", LoadSceneMode.Single);
        });

        // Hide();

    }

    private void Start() {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        if(!LobbyManager.Instance.IsLobbyHost())
            startGameBtn.interactable = false;
        else
            startGameBtn.interactable = true;

        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            UIPlayerItem UIPlayerItem = playerSingleTransform.GetComponent<UIPlayerItem>();

            UIPlayerItem.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            UIPlayerItem.UpdatePlayer(player);
        }

        LobbyName = lobby.Name;
        playerCountText.text = "( " + lobby.Players.Count + " / " + lobby.MaxPlayers + " )";

        Show();
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}
}
