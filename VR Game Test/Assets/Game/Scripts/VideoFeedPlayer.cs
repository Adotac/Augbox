using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityEngine.UI;

public class VideoFeedPlayer : MonoBehaviour
{
    static WebCamTexture bcam;

    private void Start() {
        if(bcam == null){
            bcam = new WebCamTexture();
            Texture.allowThreadedTextureCreation = true;
        }

        if (TryGetComponent<Image>(out Image img)){
            img.material.mainTexture = bcam;
        }
        else if(TryGetComponent<Renderer>(out Renderer r)){
            r.material.mainTexture = bcam;
        }
        else if(TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)){
            sr.material.mainTexture = bcam;
        }

        

        if(!bcam.isPlaying)
            bcam.Play();


    }

}
