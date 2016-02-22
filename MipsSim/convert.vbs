set objFSO = CreateObject("Scripting.FileSystemObject")

set objFile = objFSO.CreateTextFile("commands.cs")
set objFile1 = objFSO.CreateTextFile("Type.cs")
set objFile2 = objFSO.CreateTextFile("OpCode.cs")
set objFile3 = objFSO.CreateTextFile("Funct.cs")
set objFileFunct = objFSO.CreateTextFile("FunctBack.cs")

set objR = objFSO.OpenTextFile("commands.csv", 1)
set objI = objFSO.OpenTextFile("commands_immidiate.csv", 1)
set objJ = objFSO.OpenTextFile("commands-jump.csv", 1)

do until objR.AtEndOfStream
	strLine = objR.ReadLine
	parts = split(strLine, ";")
	addCommand parts(0)
	addOpCode right(GetBit(parts(1)), len(GetBit(parts(1)))-2),parts(0)
	addFunc right(GetBit(parts(2)), len(GetBit(parts(2)))-2),parts(0)
	addType "RType",parts(0)
	addFunctBack parts(0), right(GetBit(parts(2)), len(GetBit(parts(2)))-2)
loop

objFileFunct.WriteLine ""

do until objI.AtEndOfStream
	strLine = objI.ReadLine
	parts = split(strLine, ";")
	addCommand parts(0)
	addOpCode right(GetBit(parts(1)), len(GetBit(parts(1)))-2),parts(0)
	addType "IType",parts(0)
	addOpCodeBack parts(0), right(GetBit(parts(1)), len(GetBit(parts(1)))-2)
loop

do until objJ.AtEndOfStream
	strLine = objJ.ReadLine
	parts = split(strLine, ";")
	addCommand parts(0)
	addOpCode right(GetBit(parts(1)), len(GetBit(parts(1)))-2),parts(0)
	addType "JType",parts(0)
loop

sub addCommand(data)
	objFile.writeLine "public const string " & ucase(left(data,1)) & right(data, len(data)-1) & " = """ & data & """;"
end sub

sub addFunctBack(inst, data)
	objFileFunct.writeLine "public const string " & ucase(left(inst,1)) & right(inst, len(inst)-1) & "Funct = """ & data & """;"
end sub

sub addOpCodeBack(inst, data)
	objFileFunct.writeLine "public const string " & ucase(left(inst,1)) & right(inst, len(inst)-1) & "OpCode = """ & data & """;"
end sub

sub addOpCode(data, inst)
	objFile2.writeLine "case AssemblerInstructions." & ucase(left(inst,1)) & right(inst, len(inst)-1) & ":"
	objFile2.writeLine vbtab & "result = GetBitsBitString(""" & data & """);"
	objFile2.writeLine vbtab & "break;"
end sub

sub addFunc(data, inst)
	objFile3.writeLine "case AssemblerInstructions." & ucase(left(inst,1)) & right(inst, len(inst)-1) & ":"
	objFile3.writeLine vbtab & "result = GetBitsBitString(""" & data & """);"
	objFile3.writeLine vbtab & "break;"
end sub

sub addType(data, inst)
	objFile1.writeLine "case AssemblerInstructions." & ucase(left(inst,1)) & right(inst, len(inst)-1) & ":"
	objFile1.writeLine vbtab & "result = InstructionType." & data & ";"
	objFile1.writeLine vbtab & "break;"
end sub

objR.close
objI.close
objJ.Close
objFile.Close
objFile1.Close
objFile2.Close
objFile3.Close
objFileFunct.Close

msgbox "done"

function GetBit(data)
	bits = ""
	
	data = replace(data, "0x","")
	
	for i = 0 to len(data)
		number = mid(data, i+1, 1)
		Select Case number
		   Case "0"
			 bits = bits & "0000"
		   Case "1"
			 bits = bits & "0001"
		   Case "2"
			 bits = bits & "0010"
		   Case "3"
			 bits = bits & "0011"
		   Case "4"
			 bits = bits & "0100"
		   Case "5"
			 bits = bits & "0101"
		   Case "6"
			 bits = bits & "0110"
		   Case "7"
			 bits = bits & "0111"
		   Case "8"
			 bits = bits & "1000"
		   Case "9"
			 bits = bits & "1001"
		   Case "A"
			 bits = bits & "1010"
		   Case "B"
			 bits = bits & "1011"
		   Case "C"
			 bits = bits & "1100"
		   Case "D"
			 bits = bits & "1101"
		   Case "E"
			 bits = bits & "1110"
		   Case "F"
			 bits = bits & "1111"
		 End Select
	next
	
	GetBit = bits
end function