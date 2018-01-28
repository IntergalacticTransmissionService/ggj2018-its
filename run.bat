@ECHO OFF
SET EXECPATH=%~dp0\dist\bin\DesktopGL\AnyCPU\Debug
SET EXECUTABLE=MonoGame-Desktop.exe

if not exist %EXECPATH% (
	ECHO "Game must be build...please be patient..."
	CALL .\scripts\build.bat
)

cd /d %EXECPATH%
%EXECUTABLE%
