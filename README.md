<p align="center">
    <a href="https://corrupt.wiki/"><img src="Assets/Graphical Assets/Vanguard/icon.ico" alt="RTCV Icon" /></a>
</p>

<p align="center">
    <!-- Github action -->
    <!-- <a href="https://github.com/ircluzar/RTCV/actions?query=workflow%3ABuild+branch%505"><img src="https://github.com/ircluzar/RTCV/workflows/Build/badge.svg?branch=505" alt="Build status badge" /></a> -->
    <!-- Wiki -->
    <a href="https://corrupt.wiki/"><img src="https://img.shields.io/badge/docs-corrupt.wiki-blue.svg" alt="Docs wiki badge" /></a>
    <!-- Download -->
    <a href="https://redscientist.com/rtc"><img src="https://img.shields.io/badge/download-RTC-red.svg" alt="Download badge" /></a>
    <!-- Discord -->
    <a href="https://redscientist.com/discord"><img src="https://img.shields.io/discord/279664862836031488.svg" alt="Chat badge" /></a>
    <!-- CodeCov -->
    <!-- <a href="https://codecov.io/gh/redscientistlabs/RTCV/branch/506"><img alt="Codecov" src="https://codecov.io/gh/redscientistlabs/RTCV/branch/506/graph/badge.svg"></a> -->
</p>

# RTCV - Real-Time Corruptor Vanguard

 > Real-Time Corruptor, Vanguard, CorruptCore, NetCore2, RTC Launcher

Real-Time Corruptor Vanguard is a Dynamic Corruptor for games. It is a set of libraries that can be rigged up to any program that can load the CLR and works by corrupting data in memory to force glitches. RTCV currently comes with implementations for [Bizhawk](https://github.com/ircluzar/Bizhawk-Vanguard), [Dolphin](https://github.com/NarryG/dolphin-vanguard/), [PCSX2](https://github.com/NarryG/pcsx2-Vanguard), [melonDS](https://github.com/narryg/melonds-vanguard), and [Windows Processes](https://github.com/narryg/processstub-vanguard).

Icon graciously provided by [ShyGuyXXL](https://twitter.com/shyguyxxl)

## Features
- Corrupts in real-time
- Supports various emulators via a generic API. Currently comes with Bizhawk, Dolphin, PCSX2, melonDS, and Windows real-time implementations.
- Supports corrupting files on-disk via the FileStub implementation, as well as including specialized support for certain kinds of files (WiiU games via CemuStub).
- Many corruption engines with customizable algorithms
- Comes with a built-in Package Manager for installing plugins and other packages
- Glitch Harvester (Corruption Savestate Manager)
- Blast Editor (Corruption instruction editor)
- Blast Generator (Corruption instruction generation tool in the vein of classic rom corruptors)
- Stockpile Player (Streaming-focused corruption replayer)

## Development and support

All support, bug reports, feature requests and testing is done on our [Discord server](https://redscientist.com/RTC) 
Due to the very large amount of repositories surrounding the RTC project, it is simpler for us to centralize that information elsewhere.

Current dev status: On halt temporarily. Infrastructure and services are kept maintained.

[Dev startup guide on Corrupt.wiki](https://corrupt.wiki/rtcv/other-rtc-guides/rtcv-dev-startup-guide)

- The ongoing development of RTCV is on branch 51X
- Master is currently synced with 5.1.0, not the most recent one.

### Recommended: Visual Studio 2019 (2022 works well too)
[Visual Studio 2019: Community Edition](https://visualstudio.microsoft.com/vs/community/) is a free IDE for open-source projects, and is recommended for RTCV.

### Without Visual Studio

1. Install [chocolatey](https://chocolatey.org/install)
1. `cinst -y visualstudio2019buildtools nuget.commandline netfx-4.7.1-devpack`
1. `nuget restore RTCV.sln`
1. `msbuild.exe`
