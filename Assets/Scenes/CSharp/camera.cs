using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camera : MonoBehaviour {

    public string deviceName;
    WebCamTexture tex;
    // Use this for initialization  
    IEnumerator Start()
    {
        //获取授权  
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            tex = new WebCamTexture(deviceName, 1920, 1080, 1);
            GetComponent<Image>().canvasRenderer.SetTexture(tex);
            tex.Play();
            
        }
        
    }
}
