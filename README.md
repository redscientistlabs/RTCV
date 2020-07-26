<p align="center">
    <a href="https://corrupt.wiki/"><img src="Assets/Graphical Assets/Vanguard/icon.ico" alt="RTCV Icon" /></a>
</p>

<p align="center">
    <!-- Github action -->
    <a href="https://github.com/ircluzar/RTCV/actions?query=workflow%3ABuild+branch%3Amaster"><img src="https://github.com/ircluzar/RTCV/workflows/Build/badge.svg?branch=master" alt="Build status badge" /></a>
    <!-- Wiki -->
    <a href="https://corrupt.wiki/"><img src="https://img.shields.io/badge/docs-corrupt.wiki-blue.svg" alt="Docs wiki badge" /></a>
    <!-- Download -->
    <a href="https://redscientist.com/rtc"><img src="https://img.shields.io/badge/download-RTC-red.svg" alt="Download badge" /></a>
    <!-- Discord -->
    <a href="https://corrupt.wiki/corruptors/rtc/expert#rtc-dev-discord"><img src="https://img.shields.io/discord/279664862836031488.svg" alt="Chat badge" /></a>
    <!-- Trello -->
    <a href="https://trello.com/b/9QYo50OC/rtcv"><img src="https://img.shields.io/badge/planning-Trello-blue.svg" alt="Trello badge" /></a>
</p>

# RTCV - Real-Time Corruptor Vanguard

 > Real-Time Corruptor, Vanguard, CorruptCore, NetCore2, RTC Launcher

Real-Time Corruptor Vanaguard is a Dynamic Corruptor for games. It is a set of libraries that can be rigged up to any program that can load the CLR and works by corrupting data in memory to force glitches. RTCV currently comes with implementations for [Bizhawk](https://github.com/ircluzar/Bizhawk-Vanguard), [Dolphin](https://github.com/NarryG/dolphin-vanguard/), [PCSX2](https://github.com/NarryG/pcsx2-Vanguard), [melonDS](https://github.com/narryg/melonds-vanguard), and [Windows Processes](https://github.com/narryg/processstub-vanguard).

Icon graciously provided by [ShyGuyXXL](https://twitter.com/shyguyxxl)

## Features
- Corrupts in real-time
- Supports various emulators via a generic API. Currently comes with Bizhawk, Dolphin, PCSX2, melonDS, and Windows real-time implementations.
- Supports corrupting files on-disk via the FileStub implementation, as well as including specialized support for certain kinds of files (WiiU games via CemuStub & Unity games via UnityStub).
- Many corruption engines with customizable algorithms
- Easy start option for autocorrupt
- Glitch Harvester (Corruption Savestate Manager)
- Blast Editor (Corruption instruction editor)
- Blast Generator (Corruption instruction generation tool in the vein of classic rom corruptors)
- Stockpile Player (Corruption replayer)
- As of 3.x, can survive BizHawk crashes using Detached Mode
- The Nightmare Engine will corrupt random data
- The Hellgenie Engine will corrupt random data by applying "cheats" in the vein of a game-genie code to emulated memory
- The Freeze Engine will corrupt by freezing memory to its current value
- The Distortion Engine will corrupt by taking a backup of memory and re-applying that backup a number of frames later.
- The Pipe Engine can corrupt by simulating digital short circuits
- The Vector Engine is a specialty corruption engine designed for 3D systems. The included lists target systems that support IEEE754 Floating Point values
- The Custom Engine allows you to create your own engine using the various corruption parameters.

## Development
### Recommended: Visual Studio 2019
[Visual Studio 2019: Community Edition](https://visualstudio.microsoft.com/vs/community/) is a free IDE for open-source projects, and is recommended for RTCV.

### Without Visual Studio

1. Install [chocolatey](https://chocolatey.org/install)
1. `cinst -y visualstudio2019buildtools nuget.commandline netfx-4.7.1-devpack`
1. `nuget restore RTCV.sln`
1. `msbuild.exe`
