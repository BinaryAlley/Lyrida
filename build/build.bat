@echo off

rem generate version info
call versioning.bat

rem path to msbuild.exe
path=%path%;%ProgramFiles%\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin

rem go to current folder
cd %~dp0

set /P "version=" < version.txt

msbuild build.proj /p:VersionAssembly="%version%" /p:CollectCoverage=true /p:IncludeTestAssembly=true /p:CoverletOutputFormat=cobertura /consoleloggerparameters:NoSummary /verbosity:detailed

if "%1" == "1" goto exit

pause

:exit