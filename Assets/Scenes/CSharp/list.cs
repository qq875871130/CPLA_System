using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using ChensDll;
public class list : MonoBehaviour {
    //获取输入字符串
    public Text GetNum;
    //建立字符串列表存放车牌号
    public List<string> CarNum = new List<string>();
    //声明实例化预制体
    public GameObject ListPF;
    //声明父物体
    public GameObject ListPA;
    //设置计序器
    private int index = 0;
    //声明选中车牌的索引
    public int CarIndex = -1;
    //声明消息提示文字
    public Text Tiptext;
    //声明音频源
    public AudioSource Sounds;
    //设置警报持续时间
    public float AudioTime = 8;
    //设置计时器
    private float  Timer=0;
    
    void Start()
    {

        //初始化音频源
        Sounds = GameObject.Find("Camera").GetComponent<AudioSource>();
        //初始化路径
        string txtpath = CText.PathNow + "/Car text/" + "Numbers.txt";
        if (File.Exists(txtpath) == false)
            File.Create(txtpath);
        else
        {
            //读取txt写入字符串数组
                string[] n= File.ReadAllLines(txtpath);
            //限制数组长度，防止UI溢出
            int length = n.Length;
             length = length > 8 ? 8 : n.Length;
            //遍历数组添加进list
            for (int i = 0; i < length   ; i++)
            {
                if (n[i] != null&n[i]!="")         //非空验证
                    CarNum.Add(n[i]);
            }
        }
        //刷新UI列表
        OnForeach();

    }
    void Update()
    {
        //开始计时
        Timer += Time.deltaTime;

        if (Timer > AudioTime & Sounds.isPlaying)
            Sounds.Stop();
        //捕捉快捷键
        else
        if (Input.GetKeyUp(KeyCode.Return))
        {
            OnAdd();
            OnForeach();
        }
        else if (Input.GetKeyUp(KeyCode.Delete))
        {
            OnRemove();
            OnForeach();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            onClear();
            OnForeach();
        }
    }
    //提示消息
    public void Log(string text)
    {
        Tiptext.text = text;
    }

    //添加元素
    public void OnAdd()
    {
        //当有空间时添加，并刷新一次UI列表，写入文件
        if (GetNum.text != "" & index < 8)
        {
            index++;
            CarNum.Add(GetNum.text);
            OnForeach();
            OnWrite();   
            Log("<color=yellow>添加成功!</color>");
        }
        else if (index >= 8)
            Log("<color=red>加入车牌已达上限！</color>");
        else
            Log("<color=red>请先输入车牌号！</color>");
    }
    //遍历List中元素
    public void OnForeach()
    {
        //清空列表所有GUI按钮
        ButtonClear();
        for (int i = 0; i < CarNum .Count; i++)
        {
            //Count获得List中元素数目
            //控制台打印列表所有元素
            Debug .Log (CarNum[i]);
            //显示更新后列表元素
            ListPF.GetComponentInChildren<Text>().text = CarNum[i];
            //实例化GUI按钮至父物体
            GameObject L = Instantiate(ListPF, new Vector3(179.32f, 307.65f - 32 *i , 0f), transform.rotation);
            L.transform.SetParent(ListPA.transform);
       }
        
    }

    //查找元素，返回索引值
    public int FindCarIndex(string name)
    {
        return CarNum.BinarySearch(name);
    }

    //删除元素
    public void OnRemove()
    {
        if (CarIndex != -1)
        {
            index--;
            //mPos.Remove(mPos[index]);
            CarNum.RemoveAt(CarIndex);
            CarIndex = -1;
            Log("<color=yellow>删除成功!</color>");
        }
        else
            Log("<color=red>请先选中需要删除的车牌号！</color>");
    }
    


    //清空
    public void onClear()
    {
        CarNum .Clear();
        index = 0;
        CarIndex = -1;
        Log("<color=yellow>清空成功！</color>");
    }



    /// <summary>
    /// 扩展功能函数
    /// </summary>

    //清除所有列表按钮
    public void ButtonClear()
    {
        for (int j = 0; j < ListPA.transform.childCount; j++)
        {
            Destroy(ListPA.transform.GetChild(j).gameObject );
        }
    }

    //匹配元素
    public void OnMatch(string Keyword)
    {
        if (CarNum.Contains(Keyword) == true)
        {
            GetComponent <CarOCR >().MatchText .text  = "<color=yellow><b>匹配成功！欢迎回来！</b></color>";
            GameObject.Find("MTTip").GetComponent<Text>().text = "完成！";
        }
        else if (CarNum.Contains(Keyword) == false)
        {
            GetComponent<CarOCR>().MatchText.text = "<color=red><b>匹配失败！请离开该私人车位！</b></color>";
            GameObject.Find("MTTip").GetComponent<Text>().text = "完成！";
            //报警并将车牌图片保存
            OnWarn();
            OnCopy();
            
        }
    }

    //持续报警
    public void OnWarn()
    {
        Timer = 0;
        Sounds.Play();
    }


    //将录入车牌数据写入文本并保存，以便下次打开时读入
    public void OnWrite()
    {
        //创建文件夹存放车牌文档
        System.IO.Directory.CreateDirectory(CText.PathNow + "/Car text/");
        //开辟txt路径
        string txtPath = Path.Combine(CText.PathNow + "/Car text/", "Numbers.txt");
        //如果文档不存在，就新建一个
        if (File.Exists(txtPath)==false )
        {
            File.Create(txtPath);
        }
        //写入文件
        StringBuilder sb = new StringBuilder();
        foreach (string num in CarNum)  //遍历list所有元素并分行写入
            sb.AppendLine(num);
        File.WriteAllText(txtPath, sb.ToString());

        //读取文件打印出来
        Debug.Log(File.ReadAllText(txtPath));
    }


    //保存不匹配车牌至指定文件夹，并以时间格式命名
    public void OnCopy()
    {
        //创建拷贝文件夹
        System.IO.Directory.CreateDirectory(CText .PathNow + "/NonMatchCars/");
        //执行拷贝
        File.Copy(CText.PathNow + "/AyAche/" + "ImgTemp.jpg", CText.PathNow + "/NonMatchCars/" + System.DateTime.Now.ToString("yyyy.MM.dd-HH_mm_ss") + ".jpg", true);

    }

    //浏览文件位置
    public void OnOpenFiles(string path)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = CText.PathNow + path;
        p.Start();
    }
}
