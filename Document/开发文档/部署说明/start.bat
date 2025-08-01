cd iis express

set BASE_DIR=%~dp0
set SITE_PATH=%BASE_DIR%WebSite

echo %BASE_DIR%
echo %SITE_PATH%
start /b iisexpress /path:"%SITE_PATH%" /clr:v4.0 /port:8080 

start "" "http://localhost:8080/"

pause