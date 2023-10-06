@echo off

SET ReportGeneratorFolderName=5.1.11

rem dotnet tool install -g coverlet.console
rem dotnet tool install -g dotnet-reportgenerator-globaltool

CD %~dp0

coverlet ..\tests\Lyrida.Tests\bin\Debug\net7.0\Lyrida.Tests.dll --target "dotnet" --targetargs "test --no-build" --output ..\tests\GeneratedReports\
dotnet test ..\tests\Lyrida.Tests\ --collect:"XPlat Code Coverage" --results-directory ..\tests\GeneratedReports\ --logger:"console;verbosity=quiet" --verbosity:minimal --nologo /clp:ErrorsOnly

REM ** Generate the report output based on the test results
if %errorlevel% equ 0 (
	For /F "delims=" %%D in ('Dir /B/AD/ON "..\tests\GeneratedReports\*"') do If not defined FirstDir Set "FirstDir=%%D"
	call :RunReportGeneratorOutput
)

REM ** Launch the report
if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%

:RunReportGeneratorOutput
"%USERPROFILE%\.nuget\packages\reportgenerator\%ReportGeneratorFolderName%\tools\netcoreapp3.1\ReportGenerator.exe" ^
-reports:"..\tests\GeneratedReports\%FirstDir%\coverage.cobertura.xml" ^
-targetdir:"..\tests\GeneratedReports\%FirstDir%\"
exit /b %errorlevel%

:RunLaunchReport
start "report" "..\tests\GeneratedReports\%FirstDir%\index.htm"
exit /b %errorlevel%
