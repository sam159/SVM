## Features

* 4 16-bit Registers
* 254 8-bit Flag Registers
	* Start at end of memory from 0xFF00
	* See [FLAGS.md](FLAGS.md)
* 255 8-bit Ports
	* 0x00 console in/out
	* 0x01 tape in/out
	* 0x02-0x20 Reserved
	* Others Implementation Specific
* 255 Syscall routines
* Program Counter
* Call Stack 16 x 16-bit
* 64K x 8-bit Memory
* MSB / big-endian

## ASM Parameters

| Type              | Symbol | Example/Use       |
| ----------------- | ------ | ----------------- | 
| Registers         | R      | A-D               |
| Marker            | :      | :XYZ              |
| Absolute Location | #      | 0-65535 \| 0xFFFF |

## Location Parameter (@)

| Code | ASM | Type                | Values                           |
| ---- | --- | ------------------- | -------------------------------- |
| 0x0  | P	 | Port                | 0-255 \| 0xFF                    |
| 0x1  | R   | Register            | A-D                              |
| 0x2  | M   | Memory              | 0-65535 \| 0xFFFF                |
| 0x3  | L   | Immediate           | 0-65535 \| 0xFFFF \| 'a' \| "a"  |
| 0x4  | A   | Address In Register | A-D                              |

## ASM Instructions

| Hex  | ASM   | Parameters       | Notes |
| ---- | ----- | ---------------- | ----- |
| **Misc**
| 0x00 | NOP   |                  |
| **Load/Save**
| 0x10 | LOAD  | [R] [@]          |
| 0x11 | SAVE  | [R] [@]          |
| 0x12 | LOADH | [R] [@]          |
| 0x13 | LOADL | [R] [@]          |
| 0x14 | SAVEH | [R] [@]          |
| 0x15 | SAVEL | [R] [@]          |
| **Math**
| 0x20 | CLR   | [R]              |
| 0x21 | ADD   | [R] [@]          |
| 0x22 | SUB   | [R] [@]          |
| 0x23 | DIV   | [R] [@]          |
| 0x24 | MUL   | [R] [@]          |
| 0x25 | INC   | [R]              |
| 0x26 | DEC   | [R]              |
| **Logic**
| 0x30 | NOT   | [R]              |
| 0x31 | AND   | [R] [@]          |
| 0x32 | OR    | [R] [@]          |
| 0x33 | XOR   | [R] [@]          |
| 0x34 | SHL   | [R] [0-7]        | Shift Left 
| 0x35 | SHR   | [R] [0-7]        | Shift Right
| 0x36 | BTS   | [R] [0-7]        | Bit Set
| 0x37 | BTC   | [R] [0-7]        | Bit Clear
| **Stack**
| 0x40 | CALL  | [#\|:]           |
| 0x41 | RET   |                  |
| **Program Flow**
| 0x50 | HALT  |                  |
| 0x51 | JMP   | [#\|:]           |
| 0x52 | JZ    | [R] [#\|:]       |
| 0x53 | JNZ   | [R] [#\|:]       |
| 0x54 | JBS   | [R] [1-8] [#\|:] |
| 0x55 | JBC   | [R] [1-8] [#\|:] |
| **System**
| 0x60 | SYS   | [0-255]          | Syscall, parameters depend on call

### Assembler Only

| ASM    | Parameters | Notes        |
| ------ | ---------- | ------------ |
| ORIGIN | [#]        | Sets the Memory Address for the next instruction
| ALIAS  | X Y        | Replaces word X with word Y where X and Y are alpha numberic, only applies after the instruction

## Program Format

	ORIGIN 0
	:MARKERA    JMP :MARKERB    # This is a comment
	:MARKERB    INC A
	            JMP :MARKERA
    [...ASM Instrictions...]
    MEMORY
    0x100 "string abcd"
    100 0x00 00 00
