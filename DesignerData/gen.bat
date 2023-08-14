set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-simple-json ^
    --schemaPath %CONF_ROOT%\Defines\__root__.xml ^
    -x inputDataDir=%CONF_ROOT%\Datas  ^
    -x outputCodeDir=%WORKSPACE%\Assets\_GameMain\Code\HotUpdate\Data\Gen^
    -x outputDataDir=%WORKSPACE%\Assets\_GameMain\Data

pause