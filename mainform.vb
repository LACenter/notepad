'///////////////////////////////////////////////////////////////////////////////
' Unit Description  : mainform Description
' Unit Author       : LA.Center Corporation
' Date Created      : February, Tuesday 16, 2016
' ------------------------------------------------------------------------------
'
' History
'
'
'///////////////////////////////////////////////////////////////////////////////

dim currentFile as string = ""
dim isModified as bool = false

dim mainMenu as TMainMenu
dim rootMenu as TMenuItem
dim subMenu as TMenuItem
dim memo as TMemo

'constructor of mainform
function mainformCreate(Owner as TComponent) as TForm
    return TForm.CreateWithConstructor(Owner, addressof mainform_OnCreate)
end function

'OnCreate Event of mainform
sub mainform_OnCreate(Sender as TForm)
    'Form Constructor

    'todo: some additional constructing code
    Sender.Caption = Application.Title
    Sender.Width = screen.Width - 200
    Sender.Height = screen.Height - 200
    Sender.Position = poScreenCenter
    Sender.Color = clForm
    Sender.OnCloseQuery = addressof mainform_OnClose

    mainMenu = new TMainMenu(Sender)

    rootMenu = new TMenuItem(Sender)
    rootMenu.Caption = "File"
    mainMenu.Items.Add(rootMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "New"
        subMenu.ShortCut = TextToShortCut("Ctrl+N")
        subMenu.OnClick = addressof mainform_NewClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "-"
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Open"
        subMenu.ShortCut = TextToShortCut("Ctrl+O")
        subMenu.OnClick = addressof mainform_OpenClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Save"
        subMenu.ShortCut = TextToShortCut("Ctrl+S")
        subMenu.OnClick = addressof mainform_SaveClick
        subMenu.Name = "mSave"
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Save as..."
        subMenu.ShortCut = TextToShortCut("Ctrl+Shift+S")
        subMenu.OnClick = addressof mainform_SaveAsClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "-"
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Exit"
        subMenu.ShortCut = TextToShortCut("Alt+F4")
        subMenu.OnClick = addressof mainform_ExitClick
        rootMenu.Add(subMenu)

    rootMenu = new TMenuItem(Sender)
    rootMenu.Caption = "Edit"
    mainMenu.Items.Add(rootMenu)

    'LINUX does not have Undo for TCustomEdit based components
    if not Linux and not FREEBSD then
        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Undo"
        subMenu.ShortCut = TextToShortCut("Ctrl+Z")
        subMenu.OnClick = addressof mainform_UndoClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "-"
        rootMenu.Add(subMenu)
    end if

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Cut"
        subMenu.ShortCut = TextToShortCut("Ctrl+X")
        subMenu.OnClick = addressof mainform_CutClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Copy"
        subMenu.ShortCut = TextToShortCut("Ctrl+C")
        subMenu.OnClick = addressof mainform_CopyClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Paste"
        subMenu.ShortCut = TextToShortCut("Ctrl+V")
        subMenu.OnClick = addressof mainform_PasteClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "-"
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Select All"
        subMenu.ShortCut = TextToShortCut("Ctrl+A")
        subMenu.OnClick = addressof mainform_SelectAllClick
        rootMenu.Add(subMenu)

    rootMenu = new TMenuItem(Sender)
    rootMenu.Caption = "View"
    mainMenu.Items.Add(rootMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Word Wrap"
        subMenu.Checked = true
        subMenu.OnClick = addressof mainform_WordWrapClick
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "-"
        rootMenu.Add(subMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "Font"
        subMenu.OnClick = addressof mainform_FontClick
        rootMenu.Add(subMenu)

    rootMenu = new TMenuItem(Sender)
    rootMenu.Caption = "Help"
    mainMenu.Items.Add(rootMenu)

        subMenu = new TMenuItem(Sender)
        subMenu.Caption = "About"
        subMenu.OnClick = addressof mainform_AboutClick
        rootMenu.Add(subMenu)

    memo = new TMemo(Sender)
    memo.Parent = Sender
    memo.Align = alClient
    memo.BorderSpacing.Around = 1
    memo.Color = clWindow
    memo.Font.Color = clWindowText
    memo.Name = "memo"
    memo.Lines.Text = ""
    memo.OnChange = addressof mainform_MemoChange
    memo.ScrollBars = ssAutoBoth
    memo.Font.Name = "Courier New"
    memo.Font.Size = 12
    memo.BorderStyle = bsNone

    'Check Que if there is anyhting to do
    s = GetMessage("LANotepad-Startup")
    if s <> "" then
        if FileExists(s) then
            memo.Lines.LoadFromFile(s)
            currentFile = s
            isModified = false
        end if
    end if

    'Set as Application.MainForm
    Sender.setAsMainForm
end sub

sub mainform_MemoChange(Sender as TMemo)
    isModified = true
end sub

sub mainform_NewClick(Sender as TMenuItem)
    if isModified then
        if MsgQuestion("Please Confirm", "Would you like to save the changes?") then
            mainform_SaveClick(Sender)
            isModified = false
        end if
    end if

    memo.Lines.Text = ""
    isModified = false
    currentFile = ""
end sub

sub mainform_OpenClick(Sender as TMenuItem)
    dim dlg as TOpenDialog

    if isModified then
        if MsgQuestion("Please Confirm", "Would you like to save the changes?") then
            mainform_SaveClick(Sender)
            isModified = false
        end if
    end if

    dlg = new TOpenDialog(Sender.Owner)
    dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*)|*"
    dlg.DefaultExt = "*.txt"
    if dlg.ExecuteDimmed then
        currentFile = dlg.FileName
        memo.Lines.LoadFromFile(currentFile)
        isModified = false
    end if
    delete dlg
end sub

sub mainform_SaveClick(Sender as TMenuItem)
    if currentFile <> "" then
        memo.Lines.SaveToFile(currentFile)
        isModified = false
    else
        mainform_SaveAsClick(Sender)
    end if
end sub

sub mainform_SaveAsClick(Sender as TMenuItem)
    dim dlg as TSaveDialog

    dlg = new TSaveDialog(Sender.Owner)
    dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*)|*"
    dlg.DefaultExt = "*.txt"
    if dlg.Execute then
        currentFile = dlg.FileName
        memo.Lines.SaveToFile(currentFile)
        isModified = false
    else
        isModified = true
    end if
    delete dlg
end sub

sub mainform_ExitClick(Sender as TMenuItem)
    TForm(Sender.Owner).Close
end sub

sub mainform_OnClose(Sender as TForm, byref CanClose as bool)
    dim chk as int

    if isModified then
        chk = MsgQuestionCancel("Please Confirm", "Would you like to save the changes?")
        select case chk
            case mrCancel:
                CanClose = false
            case mrYes:
                mainform_SaveClick(TMenuItem(Sender.Find("mSave")))
            case mrNo:
                isModified = false
        end select
    end if

    if isModified then
        CanClose = false
    end if
end sub

sub mainform_UndoClick(Sender as TMenuItem)
    memo.Undo
end sub

sub mainform_CutClick(Sender as TMenuItem)
    memo.CutToClipboard
end sub

sub mainform_CopyClick(Sender as TMenuItem)
    memo.CopyToClipboard
end sub

sub mainform_PasteClick(Sender as TMenuItem)
    memo.PasteFromClipboard
end sub

sub mainform_SelectAllClick(Sender as TMenuItem)
    memo.SelectAll
end sub

sub mainform_WordWrapClick(Sender as TMenuItem)
    memo.WordWrap = _
        not memo.WordWrap
end sub

sub mainform_FontClick(Sender as TMenuItem)
    dim dlg as TFontDialog

    dlg = new TFontDialog(Sender.Owner)
    if dlg.Execute then
        memo.Font.Assign(dlg.Font)
    end if
    delete dlg
end sub

sub mainform_AboutClick(Sender as TMenuItem)
    MsgInfo("About", "LA.Notepad Version 1.0"+"\r\n"+ _
        "Copyright (C) 2016 LA.Center Corporation")
end sub

'<events-code> - note: DESIGNER TAG => DO NOT REMOVE!

'mainform initialization constructor


