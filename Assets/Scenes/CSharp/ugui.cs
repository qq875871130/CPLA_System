using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 调用摄像机并自适应大小的脚本
/// </summary>
public class ugui : MonoBehaviour
{

    WebCamTexture camTexture;
    CanvasScaler CanScaler;
    Camera ca;
    Image img;

    void Start()
    {

        img = GetComponentInChildren<Image>();

        CanScaler = GetComponentInChildren<CanvasScaler>();
        CanScaler.referenceResolution = new Vector2(1920, 1080);

        ca = GetComponentInChildren<Camera>();
        ca.orthographicSize = Screen.width / 100.0f / 2.0f;

        //img.transform.localScale = new Vector3(-1, -1, -1);

        img.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        img.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        img.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);

        // 设备不同的坐标转换
#if UNITY_IOS || UNITY_IPHONE
        img.transform.Rotate (new Vector3 (0, 180, 90));
#elif UNITY_ANDROID
        img.transform.Rotate (new Vector3 (0, 0, 90));
#endif

        StartCoroutine(CallCamera());
    }

    IEnumerator CallCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (camTexture != null)
                camTexture.Stop();

            WebCamDevice[] cameraDevices = WebCamTexture.devices;
            string deviceName = cameraDevices[0].name;

            camTexture = new WebCamTexture(deviceName, Screen.height, Screen.width, 60);
            img.canvasRenderer.SetTexture(camTexture);

            camTexture.Play();
        }
    }
}
