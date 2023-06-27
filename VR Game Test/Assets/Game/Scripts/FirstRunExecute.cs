
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augbox
{
    // do some stuff when app is started
    public class FirstRunExecute : MonoBehaviour
    {
        private void Awake()
        {
            // get the settings
            GameSettings.Instance.Load();
        }

        private void Start()
        {
            // make sure resolution is set to the settings resolution
            // var resolutionIndex = GameSettings.Instance.GetBestMatchIndexToAvailableResolutions();

            // if (resolutionIndex != -1)
            // {
            Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(GameSettings.Instance.CurrentResolution.width, GameSettings.Instance.CurrentResolution.height, 
            GameSettings.Instance.CurrentFullScreenMode, GameSettings.Instance.CurrentResolution.refreshRate);
            // }
        }

        // private void Start() {
        //     Screen.fullScreen = !Screen.fullScreen;
        // }
    }
}
