CALL qq
cx **

rem	CALL DebugRelease.bat /B
	CALL Release.bat /B
rem	CALL Release.bat /V 001

C:\Factory\Petra\UpdatingCopy.exe out C:\be\Web\DocRoot\*(P)\*(C)
C:\Factory\Petra\RunOnBatch.exe C:\be\Web Upload.bat
