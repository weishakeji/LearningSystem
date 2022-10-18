# 开发指引 - 源码说明
##### [![](https://img.shields.io/badge/-%E5%AE%98%E6%96%B9%E7%BD%91%E7%AB%99-blue)](http://www.weishakeji.net) [![](https://img.shields.io/badge/help-%E5%9C%A8%E7%BA%BF%E5%B8%AE%E5%8A%A9-orange)](http://www.weisha100.net/) [![](https://img.shields.io/badge/upgrade-%E5%8D%87%E7%BA%A7%E6%97%A5%E5%BF%97-green)](http://www.weishakeji.net/download.html)  [![](https://img.shields.io/badge/QQ%E7%BE%A4-10237400-brightgreen)](https://qm.qq.com/cgi-bin/qm/qr?k=lL7qjJPXlfMnxo4cOd2xr-OMe-_4u8hW&jump_from=webapi&authKey=4vWIzSa9ceJ0Cn6/cDKp08SuOxv4xfGDfMn1ZI//1XG+p5nzeqW9v/PUVdI9gEh+)  [![](https://img.shields.io/badge/%E7%94%B5%E8%AF%9D-400%206015%20615-lightgrey)]()

## 项目文件说明
* Song.WebSite
   > 起始项目，系统的所有前端页面都在这里，一切从这里开始执行；

   > 第一次打开解决方案时，也许它不是超始项目，需要手工设置一下；在该项目名称上点鼠标右键，选择“设置为启动项目”

* Song.ViewData
   > 前端接口，前端页面需要的数据，都是从这里获取；基于RESTFUL API接口规范

* Song.Entities
   > 实体，系统采用了ORM框架，这里是所有的数据实体，实体对应数据库表结构；

   > 这里的代码全部由代码生成器生成，非常不建议手工更改；代码生成（WeiSha.Data.Generete.exe）可以在"/Sourcecode/Lib"文件夹获取到。

* Song.ServiceInterfaces
   > 业务层接口，系统由之前的三层架构升级而来，采用spring.net的IOC模块实现表现层与业务层的低耦合；

   > 调用接口的方法示例：Business.Do<IAccounts>().AccountsSingle(id); 其中IAccounts中接口名称，AccountsSingle为方法名；

   > 在调用接口前，需要在web.config中的“configuration/spring/objects”节点配置 
&lt;object id="IAccounts" type="Song.ServiceImpls.AccountsCom,Song.ServiceImpls" /&gt; 

* Song.ServiceImpls
   > 业务层接口的实现

### 配置数据库链接
   > 在Song.WebSite项目中的db.config填写，connectionString属性为数据库链接，可根据文档内提示填写；providerName属性为适配器名称，当前采用的是SqlServer的适配器；
* 其它适配器
   > Sqlserver：WeiSha.Data.SqlServer9.SqlServer9Provider

   > Mysql：WeiSha.Data.MySql.MySqlProvider

   > Oracle：WeiSha.Data.Oracle.OracleProvider

   > PostgreSQ：WeiSha.Data.PostgreSQL.PostgreSQLProvider, WeiSha.Data.PostgreSQL

### 接口查询（RESTFUL API）
   > 所有前端所需接口都来自Song.ViewData项目的Methods命名空间下的对象；通过~/help/api/页面查询和调试；

   > 出于安全考虑，~/help/api/页面只能在本机查询

### 数据库结构查询


## 开发指引
### 创建RESTFUL API

### 创建视图文件

### 新增数据库表或字段


## 初始信息
 * 学员账号： tester 密码1
 * 机构管理员： ~/orgadmin 账号admin 密码1
 * 超级管理员： ~/manage  账号super 密码1


## 开发环境
* 采用C#；基于.Net 4.6.2 <a href="https://download.visualstudio.microsoft.com/download/pr/8e396c75-4d0d-41d3-aea8-848babc2736a/80b431456d8866ebe053eb8b81a168b3/ndp462-kb3151800-x86-x64-allos-enu.exe" target="_blank" size=12>[下载]</a>
* 数据库采用Sqlserver2008或更高版本
* 开发工具 Microsoft Visual Studio Community 2019

## 开源地址
* Gitee ： <a href="https://gitee.com/weishakeji/LearningSystem" target="_blank">https://gitee.com/weishakeji/LearningSystem</a> 
* GitHub ：<a href="https://github.com/weishakeji/LearningSystem" target="_blank">https://github.com/weishakeji/LearningSystem</a> 
* GitCode ：<a href="https://gitcode.net/songleiming/LearningSystem" target="_blank">https://gitcode.net/songleiming/LearningSystem</a> 

