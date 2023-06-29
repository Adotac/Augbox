using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Augbox{
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string XR_SceneUI;
    [SerializeField] private string Desktop_SceneUI;


    void Start()
    {
        if(PlayerEnvironment.build_environment == BUILD_ENVIRONMENT.HOLOLENS){
            SceneManager.LoadScene(XR_SceneUI);
        }
        else{
            SceneManager.LoadScene(Desktop_SceneUI);
        }

        
    }


}
}