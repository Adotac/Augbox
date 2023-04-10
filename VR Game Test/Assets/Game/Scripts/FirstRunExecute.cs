using FishNet.Plugins.FishyEOS.Util;
using PlayEveryWare.EpicOnlineServices;
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

            // this inits as well as gets
            if (EOS.GetManager() == null)
            {
                Debug.LogError("Failed to find EOSManager.");
            }
        }

        private void Start()
        {
            // make sure resolution is set to the settings resolution
            var resolutionIndex = GameSettings.Instance.GetBestMatchIndexToAvailableResolutions();

            if (resolutionIndex != -1)
            {
                Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, GameSettings.Instance.CurrentFullScreenMode, Screen.resolutions[resolutionIndex].refreshRate);
            }
        }
    }
}
