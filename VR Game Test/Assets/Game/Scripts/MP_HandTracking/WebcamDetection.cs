using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augbox{
    public class WebcamDetection: MonoBehaviour
    {
        public bool isCamDeviceAvailable(){
            if(FilterVirtualCameras(WebCamTexture.devices).Length > 0)
                return true;
            else
                return false;
        }

        private WebCamDevice[] FilterVirtualCameras(WebCamDevice[] devices)
        {
            // Create a list to store the filtered devices
            List<WebCamDevice> filteredList = new List<WebCamDevice>();
            
            // Iterate through each device and check if it is a virtual camera
            foreach (WebCamDevice device in devices)
            {
                // print(device.name);
                if (!IsVirtualCamera(device))
                {
                    // Add the device to the filtered list if it is not a virtual camera
                    filteredList.Add(device);
                }
            }
            
            // Convert the filtered list to an array and return it
            return filteredList.ToArray();
        }
        
        private bool IsVirtualCamera(WebCamDevice device)
        {
            // TODO: Implement your logic to check if the device is a virtual camera
            // You can check the device name, device properties, or use platform-specific APIs
            
            // Placeholder logic: Assume any device starting with "Virtual" is a virtual camera
            if (device.name.Contains("Virtual"))
            {
                return true;
            }
            
            return false;
        }
    }
}
