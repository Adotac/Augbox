using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace Augbox{
    public class PlayerEnvironment : MonoBehaviour
    {
        public static BUILD_ENVIRONMENT build_environment;
        private void Awake() {
            #if HOLOLENS
                build_environment = BUILD_ENVIRONMENT.HOLOLENS;
            #else
                build_environment = BUILD_ENVIRONMENT.DESKTOP;
            #endif

            print("RUNNING ON ENVIRONMENT: " + build_environment.ToString());
        }
    }

    public enum BUILD_ENVIRONMENT{
        HOLOLENS,
        DESKTOP,
    }
}