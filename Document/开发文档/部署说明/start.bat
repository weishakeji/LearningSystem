REM 设置网站路径与端口
set path=WebSite
set port=8080

@echo off
echo 微厦在线学习考试系统 启动中...
echo.

REM 设置网站所在路径，默认为当前当前文件夹
set BASE_DIR=%~dp0
set SITE_PATH=%BASE_DIR%%path%

::echo %BASE_DIR%
::echo %SITE_PATH%

REM 启动web服务器
cd iis express
start /b iisexpress /path:"%SITE_PATH%" /port:%port% /clr:v4.0 

REM 打开浏览器，访问网站
start "" "http://localhost:"%port% 

pause