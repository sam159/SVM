# Flags

Locations shown below are relative to the start of flags, typically 0xFF00.

Bits marked N/A read as zero and writing values will not have an effect.

## Console

### Status | CONSTS | 0x00

|                 | 7   | 6   | 5   | 4   | 3   | 2   | 1   | 0   |
| --------------- | --- | --- | --- | --- | --- | --- | --- | --- |
| **Read/Write**  |     |     |     |     |     |     | R/W | R   |
| **Defaults**    |     |     |     |     |     |     | 1   | X   |
| **Name**        | N/A | N/A | N/A | N/A | N/A | N/A | Enabled | Available |

### Cursor X | CONPOSX | 0x01

* 8-bit unsigned number
* Read/Write

### Cursor Y | CONPOSY | 0x02

* 8-bit unsigned number
* Read/Write

### Width | CONX | 0x03

* 8-bit unsigned number
* Read/Write

### Height | CONY | 0x04

* 8-bit unsigned number
* Read/Write

## ALU

Flags here for logic/math results

## Tape

Tape control and status

## Traps

Trap status and settings


<!-- 
### Name | ALIAS | 0x00

|                 | 7   | 6   | 5   | 4   | 3   | 2   | 1   | 0   |
| --------------- | --- | --- | --- | --- | --- | --- | --- | --- |
| **Read/Write**  |     |     |     |     |     |     |     |     |
| **Defaults**    |     |     |     |     |     |     |     |     |
| **Name**        |     |     |     |     |     |     |     |     |
-->