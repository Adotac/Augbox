using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Augbox{
public class UILobbyCreate : MonoBehaviour
{
    public static UILobbyCreate Instance { get; private set; }

    [SerializeField] private InputField inputFieldLobbyName;

    [SerializeField] private Button buttonSave;
    [SerializeField] private Button buttonCancel;

    private bool isPrivate = false;
    private int maxPlayers = 2;

    // room that has been entered by the user
    public string lobbyName { get; set; }

    private void Awake() {
        Instance = this;

        buttonSave.onClick.AddListener(() => {
            Save();
        });

        buttonCancel.onClick.AddListener(() => {
            Cancel();
        });

        Hide();
    }

    private void Start()
    {
        inputFieldLobbyName.onValueChanged.AddListener(delegate 
        {
            UpdateControlState();
        });            
    }

    private void UpdateControlState()
    {
        buttonSave.interactable = !String.IsNullOrEmpty(inputFieldLobbyName.text);
    }
    private void UpdateText() {
        inputFieldLobbyName.text = lobbyName;
    }

    
    private void Save()
    {
        lobbyName = inputFieldLobbyName.text;

        LobbyManager.Instance.CreateLobby(
                lobbyName,
                maxPlayers,
                isPrivate
            );
        // UILobbyRoom.Instance.Show();
        UpdateText();
        Hide();
    }

    private void Cancel()
    {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);

        lobbyName = "";
        UpdateText();
    }
}
}