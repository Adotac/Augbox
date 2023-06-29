using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

namespace Augbox
{
    public class MRTKManager : MonoBehaviour
    {
        private MixedRealityControllerVisualizationProfile controllerVisualProfile = null;

        // Start is called before the first frame update
        void Start()
        {
            controllerVisualProfile = MixedRealityToolkit.Instance.ActiveProfile.InputSystemProfile.ControllerVisualizationProfile;
            // MixedRealityControllerVisualizer test = controllerVisualProfile.DefaultControllerVisualizationType;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
