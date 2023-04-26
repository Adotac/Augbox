using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;

public class Holocamera : MonoBehaviour
{
    public RawImage outputImage;

    private WebCamTexture webCamTexture;
    private WebCamDevice[] devices;

    void Start()
    {
        devices = WebCamTexture.devices;
        StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            for (int i = 0; i < devices.Length; i++)
            {
                Debug.Log(devices[i].name);
                // Check if the device name contains "HoloLens" to select the correct camera.
                if (devices[i].isFrontFacing && devices[i].name.Contains("HoloLens"))
                {
                    webCamTexture = new WebCamTexture(devices[i].name);
                    outputImage.texture = webCamTexture;
                    webCamTexture.Play();
                    break;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}
