using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augbox{
    public class HandTrackingWrapper : MonoBehaviour
    {
        [SerializeField] private HandTrackingSolution HandTrackingSolutionScript;
        [SerializeField] private HandTracking HandTrackingScript;
        [SerializeField] private HandTrackingGraph HandTrackingGraphScript;
        [SerializeField] private TextureFramePool TextureFramePoolScript;
        [SerializeField] private Bootstrap BootstrapScript;
        [SerializeField] private WebCamSource WebCamSourceScript;
        [SerializeField] private StaticImageSource StaticImageSourceScript;
        [SerializeField] private VideoSource VideoSourceScript;

        
        private void Awake() {
            HandTrackingSolutionScript = new HandTrackingSolution();
            HandTrackingScript = new HandTracking();
            HandTrackingGraphScript = new HandTrackingWrapper();
            TextureFramePoolScript = new TextureFramePool();
            BootstrapScript = new Bootstrap();
            WebCamSourceScript = new WebCamSource();
            StaticImageSourceScript = new StaticImageSource();
            VideoSourceScript = new VideoSource();

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
