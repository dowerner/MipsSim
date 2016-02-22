set objFSO = CreateObject("Scripting.FileSystemObject")
set objFile = objFSO.CreateTextFile("doc.xaml")
set objDoc = objFSO.OpenTextFile("doc.csv",1)

index = 0

Do until objDoc.AtEndOfStream
	strLine = objDoc.ReadLine
	parts = split(strLine, ";")
	writeDocLine parts(0), parts(1), index
	index = index + 1
loop

msgbox "done"

sub writeDocLine(instruction, description, index)
	objFile.WriteLine "<TextBlock VerticalAlignment=""Center"" Grid.Column=""0"" Grid.Row=""" & index & """>" & instruction & "</TextBlock>"
	objFile.WriteLine "<TextBlock VerticalAlignment=""Center""  Grid.Column=""1"" Grid.Row=""" & index & """ TextWrapping=""Wrap"">"
	objFile.WriteLine vbtab & description
	objFile.WriteLine "</TextBlock>"
end sub

objDoc.Close
objFile.Close