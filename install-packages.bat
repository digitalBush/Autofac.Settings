set current=%~dp0
"%current%tools\nuget\nuget.exe" install "%current%packages.config" -o "%~dp0lib" -ExcludeVersion