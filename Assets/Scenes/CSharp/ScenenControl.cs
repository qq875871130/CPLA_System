using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using ChensDll;

public class ScenenControl : MonoBehaviour
{
    /// <summary>
    /// 场景资源管理
    /// </summary>
    //用于读取并显示的说明文字
    public Text Usertext;

    

    //读取说明文档
    public void ReadUserText()
    {
        Debug.Log("Read");
        string path = System.Environment.CurrentDirectory +"/Readme.txt";    //开辟路径
        //存在文件即读入
        if (File.Exists(path))
        {
            Usertext.text = File.ReadAllText(path);
            Debug.Log("Read Successfull");
        }
        else
        {
            Usertext.text = System.Environment.CurrentDirectory;
        }
    }
    //退出游戏
    public  void AppQuit()
    {
        Application.Quit();
    }
}
