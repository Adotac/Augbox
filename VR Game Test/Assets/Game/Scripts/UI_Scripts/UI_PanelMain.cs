using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Linq;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

namespace Augbox{
public class UI_PanelMain : MonoBehaviour
{
    public static UI_PanelMain Instance { get; private set; }
    [Tooltip("Input for changing player name directly")]
    [SerializeField]
    private MRTKTMPInputField inputFieldPlayerName;

    [Tooltip("Button for Multiplayer game")]
    [SerializeField] private Button buttonMulti;

    [Tooltip("Button for playing single mode")]
    [SerializeField] private Button buttonSingle;

    [Tooltip("Button for adjusting settings")]
    [SerializeField] private Button buttonSettings;

    [Tooltip("Button for exit application")]
    [SerializeField] private Button buttonExit;

    private void Awake() {
        Instance = this;
        buttonMulti.onClick.AddListener(() => {
            MultiGame();
        });
        buttonSingle.onClick.AddListener(() => {
            SinglePlayerGame();
        });
        buttonSettings.onClick.AddListener(() => {
            ShowSettings();
        });
        buttonExit.onClick.AddListener(() => {
            Exit();
        });
    }

    private void Start()
    {
        GameSettings.Instance.CurrentPlayerName = "";
        inputFieldPlayerName.onValueChanged.AddListener(delegate
        {
            GameSettings.Instance.CurrentPlayerName = inputFieldPlayerName.text;
        });

        UpdateControlState();
    }

    private void DoMultiGame()
    {
        print("GameSettings name = " + GameSettings.Instance.CurrentPlayerName);
        // is player name blank ?
        if (String.IsNullOrEmpty(GameSettings.Instance.CurrentPlayerName))
        {
            print("name empty!");
            // show panel that gets player name
            UIPanelPlayerName.Instance.Show();
            // update player name on this panel
            UpdateControlState();
        }
        else{
            LobbyManager.Instance.Authenticate(GameSettings.Instance.CurrentPlayerName);
            UILobbyList.Instance.Show();
        }


    }

    public void MultiGame(){
        DoMultiGame();
    }

    public void SinglePlayerGame(){
        // change to other scene
        Debug.Log("Changing scene to single player mode.");

#if !HOLOLENS
        if(GetComponent<WebcamDetection>().isCamDeviceAvailable()){
            SceneManager.LoadScene("Scene_SinglePlayer", LoadSceneMode.Single);
        }
        else
            Debug.Log("No camera available!");
#else
        SceneManager.LoadScene("Scene_SinglePlayer", LoadSceneMode.Single);
#endif

    }

    public void ShowSettings()
    {
        // UIPanelManager.Instance.ShowPanel<UIPanelMenuSettings>();
        UIPanelGameSettings.Instance.Show();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateControlState()
    { 
        inputFieldPlayerName.text = GameSettings.Instance.CurrentPlayerName;
        LobbyManager.Instance.UpdatePlayerName(inputFieldPlayerName.text);
    }
}
}