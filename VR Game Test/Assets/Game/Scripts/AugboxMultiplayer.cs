using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augbox{
    public class AugboxMultiplayer : MonoBehaviour
    {
        public static AugboxMultiplayer Instance { get; private set; }
        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }


    }
}