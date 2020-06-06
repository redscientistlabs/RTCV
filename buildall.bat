@echo OFF 

call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\VC\Auxiliary\Build\vcvarsall.bat" x64
echo "Building all RTCV Projects"
echo .  
devenv "RTCV.sln" /build "Release|AnyCPU"
devenv "../FileStub-Vanguard/FileStub-Vanguard.sln" /build "Release|AnyCPU"
devenv "../CemuStub-Vanguard/CemuStub-Vanguard.sln" /build "Release|AnyCPU"
devenv "../RTC3/Real-Time Corruptor/BizHawk_RTC/BizHawk.sln" /build "Release|AnyCPU"
devenv "../DolphinNarrysMod/Source/dolphin-emu.sln" /build "Release|x64"
echo . 
echo "All builds completed." 
pause