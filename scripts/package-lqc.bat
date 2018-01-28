@ECHO OFF

REM SET ENVIRONMENT
SET VCPATH="D:\Coding\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build"
CALL %VCPATH%/vcvarsall x86

REM Run Build
SET GAMEDIR="D:\Projects\Other\ggj2018-its"
SET GAME=IntergalacticTransmissionService
msbuild %GAMEDIR%/

REM Copy Files
xcopy %GAMEDIR%\source\%GAME%-Desktop\bin\* %GAMEDIR%\dist\bin\ /s /i
xcopy %GAMEDIR%\source\%GAME%-Desktop\Content\* %GAMEDIR%\dist\Content\ /s /i
