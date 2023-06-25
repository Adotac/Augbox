// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// using System.Linq;


// using UnityEngine.SceneManagement;

// namespace Augbox
// {
//     public class UIPanelMain : UIPanel<UIPanelMain>, IUIPanel
//     {

//         [Tooltip("Input for changing player name directly")]
//         [SerializeField]
//         private InputField inputFieldPlayerName;

//         [Tooltip("Button for hosting game")]
//         [SerializeField]
//         private Button buttonHost;

//         [Tooltip("Button for joining game")]
//         [SerializeField]
//         private Button buttonJoin;

//         [Tooltip("Button for playing single mode")]
//         [SerializeField]
//         private Button buttonSingle;

//         [Tooltip("Button for adjusting settings")]
//         [SerializeField]
//         private Button buttonSettings;

        

//         private void Start()
//         {
//             inputFieldPlayerName.onValueChanged.AddListener(delegate
//             {
//                 GameSettings.Instance.CurrentPlayerName = inputFieldPlayerName.text;
//             });

//             UpdateControlState();
//         }

//         private void OnDestroy()
//         {

//         }

//         private void UpdateControlState()
//         { 
//             inputFieldPlayerName.text = GameSettings.Instance.CurrentPlayerName;
//         }

//         protected override void OnShowing()
//         {
//             UpdateControlState();
//         }

//         private IEnumerator DoHostGame()
//         {
//             // is player name blank ?
//             if (String.IsNullOrEmpty(GameSettings.Instance.CurrentPlayerName))
//             {
//                 // show panel that gets player name
//                 yield return UIPanelManager.Instance.ShowPanelAndWaitTillHidden<UIPanelPlayerName>();

//                 // update player name on this panel
//                 UpdateControlState();
//             }

//             // only allow host if we have player name
//             if (!String.IsNullOrEmpty(GameSettings.Instance.CurrentPlayerName))
//             {
//                 yield return UIPanelManager.Instance.ShowPanelAndWaitTillHidden<UIPanelHostDetails>();

//                 // did we create a valid room name ?
//                 if (UIPanelHostDetails.Instance.UIResult)
//                 {
//                     UIPanelLobby.Instance.IsHost = true;
//                     UIPanelManager.Instance.ShowPanel<UIPanelLobby>();
//                 }
//             }
//         }

//         private IEnumerator DoJoinGame()
//         {
//             // is player name blank ?
//             if (String.IsNullOrEmpty(GameSettings.Instance.CurrentPlayerName))
//             {
//                 // show panel that gets player name
//                 yield return UIPanelManager.Instance.ShowPanelAndWaitTillHidden<UIPanelPlayerName>();

//                 // update player name on this panel
//                 UpdateControlState();
//             }

//             // only allow join if we have player name
//             if (!String.IsNullOrEmpty(GameSettings.Instance.CurrentPlayerName))
//             {
//                 // show panel showing list of lobbies
//                 UIPanelManager.Instance.ShowPanel<UIPanelLobbies>();
//             }
//         }

//         public void HostGame()
//         {
//             StartCoroutine(DoHostGame());
//         }

//         public void JoinGame()
//         {
//             StartCoroutine(DoJoinGame());
//         }

//         public void SinglePlayerGame(){
//             // change to other scene
//             Debug.Log("Changing scene to single player mode.");

//             if(GetComponent<WebcamDetection>().isCamDeviceAvailable()){
//                 SceneManager.LoadScene("Scene_SinglePlayer", LoadSceneMode.Single);
//             }
//             else
//                 Debug.Log("No camera available!");


//         }
//         public void ShowSettings()
//         {
//             UIPanelManager.Instance.ShowPanel<UIPanelMenuSettings>();
//         }

//         public void Exit()
//         {
//             Application.Quit();
//         }
//     }
// }
