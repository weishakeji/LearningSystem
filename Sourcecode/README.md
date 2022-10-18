# 开发指引 - 源码说明
##### [![](https://img.shields.io/badge/-%E5%AE%98%E6%96%B9%E7%BD%91%E7%AB%99-blue)](http://www.weishakeji.net) [![](https://img.shields.io/badge/help-%E5%9C%A8%E7%BA%BF%E5%B8%AE%E5%8A%A9-orange)](http://www.weisha100.net/) [![](https://img.shields.io/badge/upgrade-%E5%8D%87%E7%BA%A7%E6%97%A5%E5%BF%97-green)](http://www.weishakeji.net/download.html)  [![](https://img.shields.io/badge/QQ%E7%BE%A4-10237400-brightgreen)](https://qm.qq.com/cgi-bin/qm/qr?k=lL7qjJPXlfMnxo4cOd2xr-OMe-_4u8hW&jump_from=webapi&authKey=4vWIzSa9ceJ0Cn6/cDKp08SuOxv4xfGDfMn1ZI//1XG+p5nzeqW9v/PUVdI9gEh+)  [![](https://img.shields.io/badge/%E7%94%B5%E8%AF%9D-400%206015%20615-lightgrey)]()

## 开发环境
* C#（dotNet 4.6.2 ), 开发工具 Microsoft Visual Studio Community 2019
* 数据库Sqlserver2008或更高版本

## 初始信息
 * 学员账号： tester 密码1
 * 机构管理员： ~/orgadmin 账号admin 密码1
 * 超级管理员： ~/manage  账号super 密码1

## 项目文件说明
* Song.WebSite
   > 起始项目，系统的所有前端页面都在这里，一切从这里开始执行；

   > 第一次打开解决方案时，也许它不是超始项目，需要手工设置一下；在该项目名称上点鼠标右键，选择“设置为启动项目”

* Song.ViewData
   > 前端接口，前端页面需要的数据，都是从这里获取；基于RESTFUL API接口规范

* Song.Entities
   > 实体，系统采用了ORM框架，这里是所有的数据实体，实体对应数据库表结构；

   > 这里的代码全部由代码生成器生成，非常不建议手工更改；代码生成器（WeiSha.Data.Generete.exe）可以在"/Sourcecode/Lib"文件夹获取到。

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

   > 接口的说明信息来自于Song.ViewData项目代码中的类和方法的注释；

   > 双击右侧上方的接口方法名称，可以复制接口到粘贴板；

   > 出于安全考虑，~/help/api/页面只能在本机查询，供开发时使用

### 数据实体查询
   > 通过~/help/datas/页面查询数据库表结构和字段的信息，包括表的说明与注释，字段的类型、长度、关联表、说明与注释

   > 双击表名或字段即可复制到粘贴板，方便开发时使用；双击备注和说明可以进行编辑（必须在超级管理登录状态）；

   > 出于安全考虑，~/help/datas/页面只能在本机查询，供开发时使用

## 开发指引
   > 由于系统是自主开发，从早期1.0版本的三层架构、WebForm视图方式升级而来，为了尽可能复用原有代码，架构层面的优化也自主编写了，这导致我们开发上有些区别于其它的成熟框架。例如我们虽然用了MVC，却摘除了Razor模板引擎；前端大量采用js，也用了Vue.js，却没有用npm的方式，仍然用页面引用的方式。

### 创建RESTFUL API
   > 在Song.ViewData项目的Methods命名空间下的创建类，且继承IViewAPI接口，该接没没有任何方法，仅作为标识；也可以选择继承抽象类ViewMethod，该类用于接收客户端传递的数据。示例：public class Account : ViewMethod, IViewAPI

   * 命名规范
   > 接口名称需采用不同字符。由于在C#中是大小写敏感的，例如 TestApi和TestAPI是两个不同的名称，但是在前端调用接口时，url路径是大小写不敏感的，会导致系统无法区分是哪个接口，所以在编写接口时，不同接口请采用不同的单词，而不是仅靠大小写区分。

   > 同名接口的形参名称要不同。例如在C#代码中 func(int a) 和func(DateTime a) 是两个方法，但由于js的类型没有C#丰富，且在传递过程中Json是以字符串形式传送到服务器，然后再解析为C#所需的值和类型，上述两个方法是无法区分的，所以要通过不同的形参名称区分，例如 func(int a) 和func(DateTime d) ,前者形参名称是a，后者形参名称为d，就可以区分了。简单来说，接口方法名称可以相同，但相同名称下形参名称不可以相同。

### 创建视图文件
   > 视图文件为*.html，全部在Song.WebSite项目的Templates文件夹。该文件夹下第一级文件夹为视图的路径，例如web、mobi等，第二级文件夹为视图的模板库名称，机构管理员可以在“界面风格”的管理菜单中选择不同的模板库作为当前界面风格；其中_Public为公共模板库，放置一些多个模板库公用的内容。一般Default是默认模板库，Default文件夹下即是视图文件，可以在这里新建html页。

   > 例如，地址栏路径/web/course/detail.132，视图文件在/Templates/Web/Default/文件夹下的course/detail.html文件；点后面的是参数（一般是id），和视图无关。

   > 例如，在/Templates/Web/Default/文件夹下创建test.html文件，则通过/web/test访问；只要有视图文件，即可通过路由访问。

   > 更多说明信息请进入 Song.WebSite 项目源码路径下查看 readme.rd 相应信息。

### 新增数据库表或字段
   > 在数据库新增或修改表和字段后，需要用代码生成器（WeiSha.Data.Generete.exe）重新生成 Song.Entities 项目的实体类。

   > 代码生成器（WeiSha.Data.Generete.exe）可以在"/Sourcecode/Lib"文件夹获取到。

   > 生成步骤：1、链接数据库；2、选择要生成的表；3、生成接口、4、生成实体；5、生成文件（路径选择Song.Entities 项目的文件夹）



