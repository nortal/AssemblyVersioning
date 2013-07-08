@ECHO OFF
..\..\..\.nuget\nuget.exe pack -Outputdirectory ..\output -Properties Configuration=Release -Build ..\..\AssemblyVersioning.csproj
pause
exit