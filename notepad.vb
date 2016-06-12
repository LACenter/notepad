'///////////////////////////////////////////////////////////////////////////////
' Unit Description  : notepad Description
' Unit Author       : LA.Center Corporation
' Date Created      : February, Tuesday 16, 2016
' ------------------------------------------------------------------------------
'
' History
'
'
'///////////////////////////////////////////////////////////////////////////////


imports "mainform"

'<events-code> - note: DESIGNER TAG => DO NOT REMOVE!

sub AppException(Sender as TObject, E as Exception)
    'Uncaught Exceptions
    MsgError("Error", E.Message)
end sub

'notepad initialization constructor
Application.Initialize
Application.Title = "LA.Notepad"
mainformCreate(null)
Application.Run
