@ECHO OFF

REM SET ENVIRONMENT
SET VCPATH="D:\Coding\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build"
CALL %VCPATH%/vcvarsall x86

REM Run Build
SET GAMEDIR="D:\Projects\Other\ggj2018-its"
SET GAME=IntergalacticTransmissionService
%GAMEDIR%\scripts\nuget restore %GAMEDIR%
msbuild /t:Rebuild /p:Configuration=Debug /t:restore %GAMEDIR%/

REM Copy Files
xcopy %GAMEDIR%\source\%GAME%-Desktop\bin\* %GAMEDIR%\dist\bin\ /s /i
xcopy %GAMEDIR%\source\%GAME%-Content\Content\* %GAMEDIR%\dist\bin\DesktopGL\AnyCPU\Debug\Content\ /S /I /Y
