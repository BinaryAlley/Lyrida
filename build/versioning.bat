@echo off

setlocal EnableDelayedExpansion

rem account for days that start with a zero, they are not parsed as valid number
if "%date:~7,1%" == "0" (
	set /a day=%date:~8,1%
) else (
	set /a day=%date:~7,2%
)

rem account for months that start with a zero, they are not parsed as valid number
if "%date:~4,1%" == "0" (
	set /a month=%date:~5,1%
) else (
	set /a month=%date:~4,2%
)

set /a year=(%date:~10,4% - 2023) 

rem Read the assembly version
set /P "version=" < version.txt

rem Increment the last number
set "i=1" & set "v1=%version:.=" & set /A i+=1 & set /A "v!i!=%+1"

if %v1% == %year% (
	if %v2% == %month% (
		if %v3% == %day% (
			goto :skip
		) else (
			goto condFalse
		)
	) else (
		goto condFalse
	)
) else (
	goto condFalse
)

goto :skip
:condFalse
set /a v1=%year%
set /a v2=%month%
set /a v3=%day%
set /a v4=1
:skip

rem Update the version file
> version.txt echo %v1%.%v2%.%v3%.%v4%
rem write the assembly info file that will be used during compilation
cd..\src\server\
> CommonAssemblyInfo.cs echo using System.Reflection; 
>> CommonAssemblyInfo.cs echo.[assembly: AssemblyVersion("%v1%.%v2%.%v3%.%v4%")]
>> CommonAssemblyInfo.cs echo.[assembly: AssemblyFileVersion("%v1%.%v2%.%v3%.%v4%")]
cd..\client\
> CommonAssemblyInfo.cs echo using System.Reflection; 
>> CommonAssemblyInfo.cs echo.[assembly: AssemblyVersion("%v1%.%v2%.%v3%.%v4%")]
>> CommonAssemblyInfo.cs echo.[assembly: AssemblyFileVersion("%v1%.%v2%.%v3%.%v4%")]