;       ----------------------------------------------------

;    VegaPlay - version 0.1
;    Copyright 2010 Don Miller
;    For more information, visit: http://www.no-carrier.com

;    This program is free software: you can redistribute it and/or modify
;    it under the terms of the GNU General Public License as published by
;    the Free Software Foundation, either version 3 of the License, or
;    (at your option) any later version.

;    This program is distributed in the hope that it will be useful,
;    but WITHOUT ANY WARRANTY; without even the implied warranty of
;    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;    GNU General Public License for more details.

;    You should have received a copy of the GNU General Public License
;    along with this program.  If not, see <http://www.gnu.org/licenses/>.

;       ----------------------------------------------------

NewButtons = $c1
OldButtons = $c2
JustPressed = $c3

SongNumber = $c4
OldSong = $c5

LoadAddy = $A457
InitAddy = $A679
PlayAddy = $A67C

;       ----------------------------------------------------

        .ORG $7ff0
Header:                         ; 16 byte .NES header (iNES format)
	.DB "NES", $1a
	.DB $02                 ; size of PRG ROM in 16kb units
	.DB $01			; size of CHR ROM in 8kb units
	.DB #%00000000		; mapper 0
	.DB #%00000000		; mapper 0
        .DB $00
        .DB $00
        .DB $00
        .DB $00
        .DB $00
        .DB $00
        .DB $00
        .DB $00

;       ----------------------------------------------------

        .org LoadAddy
        .incbin "temp.nsf"      ; NSF filename

        .org $fa00              ; start of code

;       ----------------------------------------------------

Reset:                          ; reset routine
        SEI
        CLD
	LDX #$00
	STX $2000
	STX $2001
	DEX
	TXS
  	LDX #0
  	TXA
ClearMemory:
	STA 0, X
	STA $100, X
	STA $200, X
	STA $300, X
	STA $400, X
	STA $500, X
	STA $600, X
	STA $700, X
        STA $800, X
        STA $900, X
        INX
	BNE ClearMemory

;       ----------------------------------------------------

	LDX #$02                ; warm up
WarmUp:
	BIT $2002
	BPL WarmUp
	DEX
	BNE WarmUp

       	LDA #$3F
	STA $2006
	LDA #$00
	STA $2006
        TAX
LoadPal:                        ; load palette
        LDA palette, x
        STA $2007
        INX
        CPX #$20
        BNE LoadPal

	LDA #$20
	STA $2006
	LDA #$00
	STA $2006

	LDY #$04                ; clear nametables
ClearName:
	LDX #$00
	LDA #$00
PPULoop:
	STA $2007
	DEX
	BNE PPULoop

	DEY
	BNE ClearName

;       ----------------------------------------------------

        LDA #$00                ; set up variables
        STA SongNumber

;       ----------------------------------------------------

        JSR DrawScreen          ; draw initial nametable
        JSR InitSprites
        JSR InitMusic
        JSR Vblank              ; turn on screen

;       ----------------------------------------------------

InfLoop:                        ; loop forever, program now controlled by NMI routine

        JMP InfLoop

;       ----------------------------------------------------

InitMusic:

	lda #$00
	ldx #$00
Clear_Sound:
	sta $4000,x
	inx
	cpx #$0F
	bne Clear_Sound

	lda #$10
	sta $4010
	lda #$00
	sta $4011
	sta $4012
	STA $4013

	lda #%00001111
	STA $4015

	lda #$C0
	STA $4017

	LDA SongNumber		; song number
	ldx #$00		; 00 for NTSC or $01 for PAL
	jsr InitAddy		; init address
        rts

;       ----------------------------------------------------

DrawScreen:

        LDA #<pic              ; load low byte of first picture
        STA $10
        LDA #>pic              ; load high byte of first picture
        STA $11

   	LDA #$20                ; set to beginning of first nametable
    	STA $2006
    	LDA #$00
    	STA $2006

        LDY #$00
        LDX #$04

NameLoop:                       ; loop to draw entire nametable
        LDA ($10),y
        STA $2007
        INY
        BNE NameLoop
        INC $11
        DEX
        BNE NameLoop

        RTS

;       ----------------------------------------------------

InitSprites:
      LDA #$ff
      LDX #$00
ClearSprites:
      STA $500, x
      INX
      BNE ClearSprites

      LDA #$00
      STA $2003                 ; set the low byte (00) of the RAM address
      LDA #$05
      STA $4014                 ; set the high byte (05) of the RAM address

LoadSprites:
      LDX #$00
LoadSpritesLoop:
      LDA sprites, x            ; load data from address
      STA $0500, x              ; store into RAM address
      INX
      CPX #4
      BNE LoadSpritesLoop
      RTS

sprites:
           ;vert tile attr horiz
        .db $37, $2D, $00, $17  ; sprite

;       ----------------------------------------------------

UpdateSprites:
        LDA #$00
        STA $2003
        LDA #$05
        STA $4014
        RTS

;       ----------------------------------------------------

ChangeSprite:
        LDX SongNumber
        LDA Line,x
        STA $0500
        RTS

Line:
        .db $37,$47,$57,$67,$77,$87,$97,$A7,$B7,$C7

;       ----------------------------------------------------

Vblank:                         ; turn on the screen and start the party
	BIT $2002
	BPL Vblank

        LDX #$00
        STX $2005
        STX $2005

	LDA #%10001000
	STA $2000
        LDA #%00011110
	STA $2001

        RTS

;       ----------------------------------------------------

ControllerTest:

        LDA NewButtons
	STA OldButtons

        LDX #$00
	LDA #$01		; strobe joypad
	STA $4016
	LDA #$00
	STA $4016
ConLoop:
	LDA $4016		; check the state of each button
	LSR
	ROR NewButtons
        INX
        CPX #$08
        bne ConLoop

	LDA OldButtons          ; invert bits
	EOR #$FF
	AND NewButtons
	STA JustPressed

	LDA SongNumber          ; save old song number for later compare
	STA OldSong

CheckDown:
	LDA #%00100000
	AND JustPressed
	BEQ CheckUp

	INC SongNumber          ; increment song number here
        LDA SongNumber
	CMP #10	                ; equal to total # of songs +1, starting from 0
	BNE CheckUp
	LDA #0
	STA SongNumber

CheckUp:
	LDA #%00010000
	AND JustPressed
	BEQ EndDrawChk

        DEC SongNumber          ; decrement song number here
        BPL EndDrawChk
	LDA #9	                ; equal to total # of songs, starting from 0
	STA SongNumber

EndDrawChk:
	LDA SongNumber          ; has song number changed? if so, load next song
	CMP OldSong
	BEQ CheckOver
	JSR ChangeSprite
        JSR InitMusic

CheckOver:

        RTS

;       ----------------------------------------------------

NMI:
        JSR UpdateSprites
        JSR ControllerTest      ; check for user input
        jsr PlayAddy            ; play the music

        RTI
IRQ:
        RTI

;       ----------------------------------------------------

palette:                        ; palette data
        .byte $0F,$10,$20,$30,$0F,$10,$20,$30,$0F,$10,$20,$30,$0F,$10,$20,$30
        .byte $0F,$10,$20,$30,$0F,$10,$20,$30,$0F,$10,$20,$30,$0F,$10,$20,$30

;       ----------------------------------------------------

pic:
        .INCBIN "screen.nam"

;       ----------------------------------------------------

	.ORG $fffa              ; vectors
	.DW NMI
	.DW Reset
	.DW IRQ