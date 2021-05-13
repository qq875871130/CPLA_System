# CPLA_System
基于Unity UGUI的桌面端应用尝试-车牌识别与追踪系统

如何使用:
打开CarLisencesAnalyzSystem.exe即可进入系统

系统功能:
1.主要功能
通过输入框录用文字信息，当被激活时，通过摄像头拍照进行文字识别，发现与录取信息不匹配时即发出警报。
该功能主要用于模拟小区私人停车位的外来车牌识别与警报系统
（1）输入并记录文字信息
（2）文字识别
（3）文字匹配
（4）警报系统
 
2.扩展功能
（1）对录取信息进行增加，删除与清空操作，并实时生成一个文本文档作为存档，在下次初始化应用时自动读取
（2）对不匹配的图片以时间格式保存，方便车主事后取证与查看记录
  
目前已知问题:
由于unity原生截屏功能依赖像素坐标截取，故目前仅适配16:9屏幕比例下的拍照功能
