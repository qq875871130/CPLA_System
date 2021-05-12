using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using ChensDll;
public class WebCam : MonoBehaviour
{
    //通过计算像素乘数确定截屏起点与长宽
    private float x ;
    private float y ;
    private  float w1 ;
    private static float h1;
    private int w ;
    private int h ;
    //乘数计算
    void CaculData()
    {
        x = (790f / 1620f) * Screen.width;
        y = (340f / 911f) * Screen.height;
        w1 = (780f / 1620f) * Screen.width;
        h1 = (540f / 911f) * Screen.height;
        w = (int)w1;
        h = (int)h1;
    }
    
    ///初始化时打印数据信息,用于调试改进数据

   // void Start()
   // {
        //CaculData();
        //Debug.Log(Screen.width + "," + Screen.height);
        //Debug.Log(x +","+y+","+w1+","+h1+","+w+","+h );
   // }


    /// 获取并保存texture

    IEnumerator GetTexture2D()
    {
        
        //绘制矩形截取屏幕
        yield return new WaitForEndOfFrame();
        Texture2D t = new Texture2D(w,h);
        t.ReadPixels(new Rect(x, y , w , h   ), 0, 0, false);
        t.Apply();

        //把图片数据转换为byte数组
        byte[] byt = t.EncodeToPNG();

        //然后保存为图片
        System.IO.Directory.CreateDirectory(CText.PathNow + "/AyAche/");
        File.WriteAllBytes(CText.PathNow + "/AyAche/" + "ImgTemp.jpg", byt);

        //已完成拍照，开始识别分析操作
        GameObject.Find("GameManager").GetComponent<CarOCR>().ActivateAY();
    }
    //截屏函数
    public void ScreenShot()
    {
        CaculData();
        GameObject.Find("AYTip").GetComponent<Text>().text = "识别中...";
        StartCoroutine("GetTexture2D");
        
    }


    

}
