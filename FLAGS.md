# Flags

Locations shown below are relative to the start of flags, typically 0xFF00.

Bits not defined read as zero and writing values will not have an effect.

## Console | 0x0*

### Status | CONSTS | 0x00

|                 | 8   | 7   | 6   | 5   | 4   | 3   | 2   | 1   |
| --------------- | --- | --- | --- | --- | --- | --- | --- | --- |
| **Read/Write**  |     |     |     |     | R   | R/W | R/W | R   |
| **Default **    |     |     |     |     | X   | 1   | 1   | X   |
| **Name**        |     |     |     |     | ReadAvailable | ReadBlock | Enabled | Available |

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

## Interrupts/Faults | 0x1*

Faults can be caught and handled by specifying a jump location in FLTRH and FLTRL. This handler must clear FLTSYS/2, it may call RET to continue execution or jump elsewhere.

### Fault Status | FLTSTS | 0x10

|                 | 8   | 7   | 6               | 5               | 4          | 3            | 2    | 1   |
| --------------- | --- | --- | --------------- | --------------- | ---------- | ------------ | ---- | --- |
| **Read/Write**  |     |     | R               | R               | R          | R            | R/W  | R/W |
| **Default **    |     |     | X               | X               | X          | X            | X    | 0   |
| **Name**        |     |     | Memory Overflow | Stack  Exceeded | Illegal Op | Undefined Op | Trip | Enabled |

If not enabled, the system will halt should a fault occur. 

In a fault condition, only one type will be indicated.

Clearing **Trip** will clear status bits 3-8, it cannot be set.

If a second fault occurs before trip is cleared, the system will halt; this known as a double-fault.

**Undefined Op** indicates an operation that was unknown. **Illegal Op** indicates that the parameters to the operation were not valid.

**Stack Exceeded** Indicates that the stack either overflowed (PUSH or CALL when full) or underflowed (RET or POP when empty).

### Fault Routine High Byte | FLTJH | 0x11

The high byte of the fault handler jump.

### Fault Routine Low Byte | FLTJL | 0x12

The low byte of the fault handler jump.


## ALU | 0x2*

Flags here for logic/math results

## Tape | 0x3*

Tape control and status



<!-- 
### Name | ALIAS | 0x00

|                 | 8   | 7   | 6   | 5   | 4   | 3   | 2   | 1   |
| --------------- | --- | --- | --- | --- | --- | --- | --- | --- |
| **Read/Write**  |     |     |     |     |     |     |     |     |
| **Default **    |     |     |     |     |     |     |     |     |
| **Name**        |     |     |     |     |     |     |     |     |
-->