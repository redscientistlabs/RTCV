--------------------------------------------------------------
VegaPlay 0.1 by NO CARRIER
8bitpeoples Research & Development - http://www.8bitpeoples.com
--------------------------------------------------------------

Copyright 2010 Don Miller
For more information, visit: http://www.no-carrier.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

--------------------------------------------------------------
INCLUDED FILES
--------------------------------------------------------------

asm6.exe         - ASM6 assembler by loopy - http://home.comcast.net/~olimar/NES/
compile.bat      - Batch file to compile VegaPlay
edit_screen.bat  - Batch file to edit the main screen
geo.chr          - CHR file by Alex Mauer  - http://www.headlessbarbie.com
geo.pal          - Default palette file
gpl.txt          - GNU General Public License
readme.txt       - You're reading it
screen.nam       - Default NES screen / nametable file
vegaplay.asm     - Source code for VegaPlay

--------------------------------------------------------------
FILES YOU'LL NEED IN THE SAME FOLDER
--------------------------------------------------------------

name.exe       - From Pin Eight Software's NES Tools, used to edit screens
               - http://www.pineight.com/pc/p8nes.zip
alleg40.dll    - Required for running Allegro applications (like name.exe)
               - http://www.pineight.com/pc/alleg40.zip

--------------------------------------------------------------
RECOMMENDED SOFTWARE
--------------------------------------------------------------

YY-CHR   - Tile editor  - http://www.briansemu.com/yymarioed/
Context  - Text editor  - http://www.contexteditor.org/
Nestopia - NES emulator - http://nestopia.sourceforge.net/
HEXEdit  - Hex editor   - http://www.mitec.cz/hex.html

--------------------------------------------------------------
RECOMMENDED HARDWARE
--------------------------------------------------------------

PowerPak - NES flash cart         - http://www.retrousb.com
ReproPak - NES reproduction board - http://www.retrousb.com

--------------------------------------------------------------
USAGE
--------------------------------------------------------------

Back in 2007 I created Vegavox for Alex Mauer. Long out of print,
it was the first NES album ever released on a cart. Now you can do
the same.

VegaPlay is an NES ROM image. It will work in Nestopia (see above)
and other NES emulators. It has also been tested on NTSC NES hardware
and a Subor famiclone using both EEPROM development carts and the
RetroZone PowerPak.

Up and down cycle through the songs. The default number of songs is 10. 
However, this can be changed. See the source code for details. I
recommend Context with the 6502 highlighter for easy source code
manipulation. 

You can use YY-CHR or a tile editor of your choice to edit the geo.chr
file. You do not have to use the included tiles, but they may be a good
start if you don't feel like drawing your own. Replacing the tiles in
the ROM will completely alter the graphics, but the program will remain
the same.

The include screen / nametable (screen.nam) can be edited by running
edit_screen.bat. However, make sure you have name.exe and alleg40.dll
in the same folder. See the name.exe documentation that comes with the
NES Tools package for more info on using the nametable / screen editor.
Don't worry about the palettes you see in the program, as they are
controlled by the vegaplay.asm file. For more information on palettes,
see my documentation for galleryNES.

The most important part is adding your own music. First, get your NSF
in the same folder as everything else. This can be an NSF you made, or
one ripped from an old game. However, I can't guarantee all NSF's will
work with this software. I can tell you right now that it won't work
with expansion chips. We'll use the Batman NSF as an example.

Open up your NSF in Nestopia. Next, go to VIEW and then choose IMAGE
INFO from the pulldown menu. Look at the last three lines:

Load Address:   0x8000
Init Address:   0x8003
Play Address:   0x8000

Now, open up vegaplay.asm. Go to line 29. It says LoadAddy. Put in
$8000 after the equals sign. Next line is InitAddy. Put in $8003.
Then put in $8000 after PlayAddy. Finally, go down to line 54. Put
your filename in quotes here, replacing temp.nsf.

Don't compile yet! You need to get rid of the NSF header. Open up
the NSF in a hex editor, like the one I recommend above: HEXEdit

Delete everything from 00 - 7F (the 128 byte NSF header). If you
are using HEXEdit, that will be the first eight lines. The last
complete line you delete should start with 0x00070 on the left hand
side.

Now, a little bit about how this software works and how the NSF is
stored. This is a 32k NROM-256 image, with 8k for CHR space. The CHR
holds the graphics, so we can forget about that. Let's focus on the
32k of storage you have for your NSF and my code.

NROM-256 - Overview of available ROM

$8000 - First 16 KB of ROM (not used)
|
|
|
|
$C000 - Second 16 KB of ROM (not used)
|
|
|
$FA00 - The VegaPlay code
$FFFF - The end of the ROM

So, you have from $8000 to $F9FF (over 30k) to store your music.
That is good, since a lot of commerical and MML created NSF's
start at $8000. If you use FamiTracker with DPCM samples, they
are usually stored at $C000. I tried to push my code to the very
bottom to allow as much space as possible for NSF's. However, be
aware: all NSF's are different, and have different playback
routines. This code simply uses the available playback routine in
the NSF itself. It is made to fit most, but not all NSF's.

Good luck and enjoy!

--------------------------------------------------------------
(POSSIBLE) SPRITE RAM CONFLICTS
--------------------------------------------------------------

One common problem I forsee is some NSF routines conflicting
with the sprite RAM. Look at lines 210 and 217. In these lines
the sprite RAM is defined at $500. You can change line 210 to
"LDA #$02" and line 217 to "STA $0200, x" (without quotes). This
will change the sprite RAM from $500 to $200. If you still have
glitched sprites, try moving it to another location. Trial and
error isn't the best way to do this, so look up your NSF playback
routine and see where it uses RAM to fix this problem.

--------------------------------------------------------------
VERSION HISTORY
--------------------------------------------------------------

0.1 - 01.18.2010 - Initial release