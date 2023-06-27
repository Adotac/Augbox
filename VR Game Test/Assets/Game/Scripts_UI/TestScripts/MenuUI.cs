using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Augbox{
    public class MenuUI : MonoBehaviour {
        [SerializeField] private Button multiBtn;
        [SerializeField] private Button singleBtn;
        [SerializeField] private Button exitBtn;

        private void Awake() {
            multiBtn.onClick.AddListener(() => {
                string playerName = EditPlayerName.Instance.GetPlayerName();

                // is player name blank ?
                if (String.IsNullOrEmpty(playerName)){
                    UI_InputWindow.Show_Static("Player Name", playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
                    () => {
                        // Cancel
                    },
                    (string newName) => {
                        playerName = newName;

                        EditPlayerName.Instance.playerNameText.text = playerName;

                    });
                }
                else{
                    LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
                    Hide();
                    LobbyListUI.Instance.Show();
                }
            });

            singleBtn.onClick.AddListener(()=>{
                // change to other scene
                Debug.Log("Changing scene to single player mode.");

                if(GetComponent<WebcamDetection>().isCamDeviceAvailable()){
                    SceneManager.LoadScene("Scene_SinglePlayer", LoadSceneMode.Single);
                }
                else
                    Debug.Log("No camera available!");
            });

            exitBtn.onClick.AddListener(()=>{
                Application.Quit();
            });
        }

        private void Hide() {
            gameObject.SetActive(false);
        }

    }
}