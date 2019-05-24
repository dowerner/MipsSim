# MipsSim
A small simulator of a MIPS 32 Processor for Windows.

## Introduction
MipsSim is meant as an easy to use tool to write short MIPS 32 programs in the MIPS assembler language and be able to execute them on a simulated CPU. The program executes the program line by line allowing the user to see exactly what happens in the CPU register banks as well as what happens in the accessed parts of the main memory.

## Functionality
* Code Editor for writing saving and loading programs
* Executing programs written in the MIPS assembler language
* Read and edit Registers and Memory at runtime
* Save/Load Memory content to a file at runtime
* Save/Load Register content to a file at runtime
* Configure size of the main memory of the simulated computer

## User Interface
The user interface in MipsSim consists of the following components.

### Menu Bar
On the top left of the menu bar the file menu can be found. The file menu allows saving, loading and opening code files. The file menu also allows the user to save the current content of the main memory of the virtual computer to a file as well as the content of the registers. Both of those file types can then be loaded back into the program.
Also located in the menu bar are the "run code", "stop execution" and "step" buttons. The step button executes the next line of code. The settings allow to alter the amount of main memory the virtual computer has to work with. In the help are informations about the usage of the program and a list of all "real" MIPS commands.
Also in the menu view the program can be configured easily to show everything in decimal or hex for easier reading.

### Code Editor
The code editor can be used to write programs with the MIPS assembler language. When the program is executed the editor will highlight the line that is currently beeing executed green. If there is an error in the code the line will be highlighted red.

### Memory View
The memory view has two parts. On the left is the representation of the instruction memory and on the right is the representation of the data memory. Both sections are part of the same main memory but are far apart which can be seen by looking at the address ranges. For better usability both views only display a certain part of the memory and not every address. The instruction view does show the all addresses which contain program code, the length therefore depends on the length of the code written in the editor. In the data section the visible addresses depend on the stack pointer ($sp) and the frame pointer ($fp) registers, where the $fp marks the beginning and the $fp the end. For future releases there should be a better option to select the relevant addresses for the data view.
Rigth clicking in the memory viewer allows the user to save the content of the virtual main memory to a file or to load a previously saved file.
At the top of each section is a tab which allows the user to switch from a bytewise view to a  wordwise view. In this architecture a word consits of four bytes so the addresses will always be mod 4. In the instruction view in word view a click on a cell will result in a little editor window which shows the individual parts of the instruction.
By clicking on a cell the user can edit the content of the main memory at any time. The user does not have to write data in the currently selected format (binary, decimal, hex) but can input any of the three formats and the program can recognize them.

### Register View
The register view shows the register banks of the CPU. Some registers like PC, HI and LO are seperated from the rest of the registers and cannot be set by using standard code. However, these registers along with any others can be edited by the user at any time as well as saved or restored by right clicking in the view and using the respective options from within the context menu.

## Coding in MipsSim
MipsSim implements all existing commands used in the MIPS RISK instruction set. Currently MipsSim does NOT implement pseudo commands like "move", "gt", etc..

## Limitations
* Currently the $sp and $fp cannot be setup correctly since the MipsSim cannot support the required amount of main memory.
* No pseudo commands
* No directives at the beginning of the code files are supported yet
