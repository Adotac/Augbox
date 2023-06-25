using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Augbox
{
    public class UIPanelPlayerName : MonoBehaviour
{
        public static UIPanelPlayerName Instance { get; private set; }
        [SerializeField] private InputField inputFieldPlayerName;
        [SerializeField] private Button buttonSave;
        [SerializeField] private Button buttonCancel;

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
            inputFieldPlayerName.onValueChanged.AddListener(delegate 
            {
                UpdateControlState();
            });            
        }

        private void UpdateControlState()
        {
            buttonSave.interactable = !String.IsNullOrEmpty(inputFieldPlayerName.text);
        }

        // protected override void OnShowing()
        // {
        //     UpdateControlState(); 
            
        //     inputFieldPlayerName.text = GameSettings.Instance.CurrentPlayerName;
        // }

        // protected override void OnShown()
        // {
        //     inputFieldPlayerName.ActivateInputField();
        // }

        public void Save()
        {
            GameSettings.Instance.CurrentPlayerName = inputFieldPlayerName.text;
            Hide();
            UI_PanelMain.Instance.UpdateControlState();
            // UIPanelManager.Instance.HidePanel<UIPanelPlayerName>(true);
        }

        public void Cancel()
        {
            // UIPanelManager.Instance.HidePanel<UIPanelPlayerName>(false);
            Hide();
        }

        private void Hide() {
            this.gameObject.SetActive(false);
        }

        public void Show() {
            this.gameObject.SetActive(true);
        }
    }
}
