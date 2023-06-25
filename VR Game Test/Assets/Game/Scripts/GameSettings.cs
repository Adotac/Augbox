using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Augbox
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameSettings : Singleton<GameSettings>
    {
        private Resolution _currentResolution = new Resolution { width = 1920, height = 1080, refreshRate = 60 };
        private FullScreenMode _currentFullScreenMode = FullScreenMode.FullScreenWindow;
        private string _currentPlayerName = "";

        private string GetPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), Application.productName, "settings.json");
        }


        public void Load()
        {
            try
            {
                if (File.Exists(GetPath()))
                {
                    JsonConvert.PopulateObject(File.ReadAllText(GetPath()), this);
                }
                else if (File.Exists(GetPath() + ".bak"))
                {
                    File.Move(GetPath() + ".bak", GetPath());
                    JsonConvert.PopulateObject(File.ReadAllText(GetPath()), this);
                }
            }
            catch
            {
                // just use defaults if can't read for some reason
            }
        }

        [JsonProperty]
        public Resolution CurrentResolution
        {
            get => _currentResolution;
            set
            {
                if (value.width != _currentResolution.width || value.height != _currentResolution.height || value.refreshRate != _currentResolution.refreshRate)
                {
                    _currentResolution = value;
                    Save();
                }
            }
        }

        public int GetBestMatchIndexToAvailableResolutions()
        {
            var bestMatch = Array.FindIndex(Screen.resolutions, x => x.width == _currentResolution.width && x.height == _currentResolution.height && x.refreshRate == _currentResolution.refreshRate);

            if (bestMatch == -1)
            {
                // sometimes refresh can be 59hz
                bestMatch = Array.FindIndex(Screen.resolutions, x => x.width == _currentResolution.width && x.height == _currentResolution.height && x.refreshRate >= 55 && x.refreshRate <= 65);
            }

            if (bestMatch == -1)
            {
                // just pick the first one we can
                bestMatch = Array.FindIndex(Screen.resolutions, x => x.width == _currentResolution.width && x.height == _currentResolution.height);
            }

            return bestMatch;
        }

        [JsonProperty]
        public FullScreenMode CurrentFullScreenMode
        {
            get => _currentFullScreenMode; 
            set
            {
                if (value != _currentFullScreenMode)
                {
                    _currentFullScreenMode = value;
                    Save();
                }
            }
        }


        [JsonProperty]
        public string CurrentPlayerName
        {
            get => _currentPlayerName;
            set
            {
                if (value != _currentPlayerName)
                {
                    _currentPlayerName = value;
                    Save();
                }
            }
        }

        private void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GetPath()));

            if (File.Exists(GetPath()))
            {
                if (File.Exists(GetPath() + ".bak"))
                {
                    File.Delete(GetPath() + ".bak");
                }

                File.Move(GetPath(), GetPath() + ".bak");
            }

            File.WriteAllText(GetPath(), JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    public enum QualityLevel
    {
        High,
        Medium,
        Low
    }
}
