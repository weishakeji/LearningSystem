# 数据库 SQL 脚本说明
##### [![](https://img.shields.io/badge/-%E5%AE%98%E6%96%B9%E7%BD%91%E7%AB%99-blue)](http://www.weishakeji.net) [![](https://img.shields.io/badge/help-%E5%9C%A8%E7%BA%BF%E5%B8%AE%E5%8A%A9-orange)](http://www.weisha100.net/) [![](https://img.shields.io/badge/upgrade-%E5%8D%87%E7%BA%A7%E6%97%A5%E5%BF%97-green)](http://www.weishakeji.net/download.html)  [![](https://img.shields.io/badge/QQ%E7%BE%A4-10237400-brightgreen)](https://qm.qq.com/cgi-bin/qm/qr?k=lL7qjJPXlfMnxo4cOd2xr-OMe-_4u8hW&jump_from=webapi&authKey=4vWIzSa9ceJ0Cn6/cDKp08SuOxv4xfGDfMn1ZI//1XG+p5nzeqW9v/PUVdI9gEh+)  [![](https://img.shields.io/badge/%E7%94%B5%E8%AF%9D-400%206015%20615-lightgrey)]()

## 完整SQL脚本 - 初始安装时使用
> script.sql文件是当前程序版本对应的完整数据库脚本，用来初始安装时创建数据。

> script.sql文件在导出时没有带创建数据库文件的脚本，所以在执行之前，需要手工创建一个空数据库，然后执行script.sql，生成表结构和初始数据。

## SQL升级脚本
* upgrade_v1
   > 这是原有1.0版本的升级脚本，用于早期1.0版本用户升级所用，如果要升级到2.0版本，则首先要把相应的1.0升级脚本执行完。  

* upgrade_v2
   > 用于2.0版本升级，请在~/license页面查看自己系统的发布时间，当系统程序升级到最新时，原程序发布时间之后的脚本文件，都要执行。

   > 如果升级程序后，不确定缺失哪些表和字段，可以通过~/help/datas/test.htm页面查看数据库相关信息。

* v1_to_v2
   > 1.0版本的程序升到2.0时，所需要执行的SQL脚本，更多说明请在“Document/开发文档”文件夹中查看“升级说明（ver1_to_ver2.0).doc”

   > 在升级2.0之前，首先要确认已经升级到1.0的最终版本，如果不确认缺失哪些表和字段，可以通过~/help/datas/test.htm页面查看数据库相关信息。
   
* other(其它sql脚本）
   > 开发工作中临时记录的，或常用的SQL脚本

## 注意事件
   > 升级之前 <strong>一定要做好备份，一定要做好备份，一定要做好备份！</strong>重要的事情说三遍！

   > 升级脚本按时间先后顺序执行，不要乱执行

## 开发交流
>QQ交流群：10237400
