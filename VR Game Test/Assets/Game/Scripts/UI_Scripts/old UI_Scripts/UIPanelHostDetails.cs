using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Augbox
{
    public class UIPanelHostDetails : MonoBehaviour
{
        public static UIPanelHostDetails Instance { get; private set; }

        [SerializeField]
        private InputField inputFieldLobbyName;

        [SerializeField]
        private Button buttonSave;

        // room that has been entered by the user
        public string LobbyName { get; set; }

        private void Awake() {
            Instance = this;
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

        // protected override void OnShowing()
        // {
        //     UpdateControlState();

        //     inputFieldLobbyName.text = "";
        // }

        // protected override void OnShown()
        // {
        //     inputFieldLobbyName.ActivateInputField();
        // }

        public void Save()
        {
            LobbyName = inputFieldLobbyName.text;
            Hide();
            // UIPanelManager.Instance.HidePanel<UIPanelHostDetails>(true);
        }

        public void Cancel()
        {
            Hide();
            // UIPanelManager.Instance.HidePanel<UIPanelHostDetails>(false);
        }

        private void Hide() {
            this.gameObject.SetActive(false);
        }

        public void Show() {
            this.gameObject.SetActive(true);
        }
    }
}
