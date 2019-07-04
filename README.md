# RTCV - Real-Time Corruptor Vanguard
Real-Time Corruptor, Vanguard, CorruptCore, NetCore2, RTC Launcher

Real-Time Corruptor Vanaguard is a Dynamic Corruptor for emulated games. It is a set of libraries that can be rigged up to various emulators and works by corrupting data into virtual memory chips of emulated systems. RTCV currently comes with implementations for [Bizhawk](https://github.com/ircluzar/Bizhawk-Vanguard), [Dolphin](https://github.com/NarryG/dolphin-vanguard/), [PCSX2](https://github.com/NarryG/pcsx2-Vanguard), and [melonDS](https://github.com/narryg/melonds-vanguard).

Features:
- Corrupts in real-time 
- Supports various emulators via a generic API. Currently comes with Bizhawk, Dolphin, PCSX2, and melonDS real-time implementations. 
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


Please consult the Official Wiki for software documentation. https://corrupt.wiki

Download link: http://redscientist.com/rtc
Trello: https://trello.com/b/9QYo50OC/rtcv


Icon graciously provided by (ShyGuyXXL)[https://twitter.com/shyguyxxl]
