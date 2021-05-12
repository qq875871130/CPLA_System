using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RtText : MonoBehaviour {
    public string Name;
    void Start()
    {
    }
    private void OnMouseUpAsButton()
    {
        Name = GetComponentInChildren<Text>().text;
        //将选中车牌号传入GM计算出索引值
        GameObject.Find("GameManager").GetComponent<list>().CarIndex = GameObject.Find("GameManager").GetComponent<list>().FindCarIndex(Name);
        //打印至控制台
        Debug.Log(GameObject.Find("GameManager").GetComponent<list>().CarIndex);
    }


}
