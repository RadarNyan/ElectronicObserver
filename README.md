![七四式电子观测仪 ( 汉化版 )](https://gist.githubusercontent.com/RadarNyan/0e24f29bcf8be522c972f1621669e00c/raw/674df2250a4ed9aa9d8bdbf9117b77079d90b67d/74EO-RN-BANNER.png)

[![Build status](https://ci.appveyor.com/api/projects/status/c5b6vry20magv8ro/branch/master?svg=true)](https://ci.appveyor.com/project/RadarNyan/electronicobserver-icfh9/branch/master)
#### 本项目 fork 自 andanteyk 的 [七四式電子観測儀](https://github.com/andanteyk/ElectronicObserver)
七四式电子观测仪 ( 「七四式電子観測儀」，以下简称 "74EO" ) 是 [andanteyk](https://github.com/andanteyk) 积极开发中的开源『艦これ』辅助浏览器。

#### 74EO 相比其他浏览器的优势
* 低内存占用 ( 本体不足 100 MB，浏览器视游戏内容一般占用 100~200 MB ) 
* 布局紧凑，排列自由 ( 可能是唯一能够实现环绕游戏画面布局的辅助浏览器，1366x768 福音 )
* 设计上完全不能修改游戏通信内容 ( 与使用 FiddlerCore 的项目不同，Nekoxy 只在通信完成后返回信息 )

#### 本项目的主要目的
* 汉化 ( 代码上区分了中日文的字体，如舰娘名、舰队名等信息会使用日文字体，确保字形准确 )
* 改造 ( 本版本新增功能在足够稳定后会申请合并到原版，本项目的目的旨在汉化而不在制造分歧 )

#### 其他信息
[本项目 Wiki](https://github.com/RadarNyan/ElectronicObserver/wiki)
[原版 Wiki ( 日文 )](https://github.com/andanteyk/ElectronicObserver/wiki)

#### 许可声明
本项目与原版 74EO 相同，使用 [MIT 许可协议](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)  

#### 使用的库
* [DynamicJson](http://dynamicjson.codeplex.com/) ( 读写 JSON 数据 ) - [Ms-PL](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Ms-PL.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) ( 窗口布局 ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) ( 通信捕获 ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Apache.txt)
* [SwfExtractor](https://github.com/andanteyk/SwfExtractor) ( 从 swf 提取图片 ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/SwfExtractor.txt)
	* [LZMA SDK (Software Development Kit)](http://www.7-zip.org/sdk.html) - Public Domain
