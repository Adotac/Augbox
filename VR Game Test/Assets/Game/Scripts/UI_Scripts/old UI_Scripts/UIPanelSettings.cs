
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Augbox
{
    public class UIPanelSettings : UIPanel<UIPanelSettings>, IUIPanel
    {
        [Tooltip("Drop down for screen resolution")]
        [SerializeField]
        private Dropdown resolutionDropDown;

        [Tooltip("Drop down for display mode")]
        [SerializeField]
        private Dropdown displayModeDropDown;

        protected override void OnShowing()
        {
            resolutionDropDown.options.Clear();

            foreach (var setting in Screen.resolutions)
            {
                resolutionDropDown.options.Add(new Dropdown.OptionData { text = $"{setting.width} x {setting.height} ({setting.refreshRate} hz)" });
            }

            resolutionDropDown.value = GameSettings.Instance.GetBestMatchIndexToAvailableResolutions();
            resolutionDropDown.RefreshShownValue();

            resolutionDropDown.onValueChanged.AddListener(HandleResolutionDropDown);

            displayModeDropDown.options.Clear();

            foreach (FullScreenMode fullScreenOption in Enum.GetValues(typeof(FullScreenMode)))
            {
                string fullScreenOptionText;

                switch (fullScreenOption)
                {
                    case FullScreenMode.ExclusiveFullScreen:
                        fullScreenOptionText = "Exclusive Full Screen";
                        break;
                    case FullScreenMode.FullScreenWindow:
                        fullScreenOptionText = "Full Screen Windowed";
                        break;
                    case FullScreenMode.MaximizedWindow:
                        fullScreenOptionText = "Maximized Windowed";
                        break;
                    case FullScreenMode.Windowed:
                        fullScreenOptionText = "Windowed";
                        break;
                    default:
                        throw new Exception("Unknown full screen option");
                }

                displayModeDropDown.options.Add(new Dropdown.OptionData { text = fullScreenOptionText });
            }

            displayModeDropDown.value = (int)GameSettings.Instance.CurrentFullScreenMode;
            displayModeDropDown.RefreshShownValue();

            displayModeDropDown.onValueChanged.AddListener(HandleDisplayModeDropDown);
        }

        private void HandleResolutionDropDown(int selected)
        {
            var currentResolution = GameSettings.Instance.CurrentResolution;

            if (Screen.resolutions[selected].width != currentResolution.width || Screen.resolutions[selected].height != currentResolution.height || Screen.resolutions[selected].refreshRate != currentResolution.refreshRate)
            {
                Screen.SetResolution(Screen.resolutions[selected].width, Screen.resolutions[selected].height, GameSettings.Instance.CurrentFullScreenMode, Screen.resolutions[selected].refreshRate);
                GameSettings.Instance.CurrentResolution = Screen.resolutions[selected];
            }
        }

        private void HandleDisplayModeDropDown(int selected)
        {
            if ((FullScreenMode)selected != GameSettings.Instance.CurrentFullScreenMode)
            {
                var resolution = GameSettings.Instance.CurrentResolution;
                Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)selected, resolution.refreshRate);
                GameSettings.Instance.CurrentFullScreenMode = (FullScreenMode)selected;
            }
        }
    }
}