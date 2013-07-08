REM assumed nuget location: <solution>/.nuget/
@ECHO OFF
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Debug -Build ..\..\AssemblyVersioning.csproj
pause
exit
