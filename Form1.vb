Imports System.IO
Imports System.Reflection.Emit

Public Class Form1

    Dim tab As TabPage
    WithEvents tb As TextBox
    Dim fn As Font = New Font("MS Gothic", 12)

    Private Sub Form1_Load() Handles MyBase.Load
        tb = New TextBox
        With tb
            .ScrollBars = ScrollBars.Both
            .Multiline = True
            .Font = fn
            .Size = New Size(TabControl1.Width, TabControl1.Height)
            .Dock = DockStyle.Fill
        End With
        tab = New TabPage
        tab.Text = IO.Path.GetFileName("新規")
        tab.Tag = "新規"
        TabControl1.TabPages.Add(tab)
        tab.Controls.Add(tb)
        lineDisp()
    End Sub
    Private Sub lineDisp() Handles tb.Click
        '行数表示
        TabControl1.Tag = CType(TabControl1.SelectedTab.Controls(0), TextBox).GetLineFromCharIndex(tb.SelectionStart) + 1
        ToolStripStatusLabel1.Text = TabControl1.Tag.ToString + "行"
        Me.Focus() 'textboxをクリックすると選択解除されるため
    End Sub

    Public Sub tabcontrol_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tb.KeyDown
        If e.KeyData = Keys.Down Or Keys.Up Then
            Call lineDisp()
        End If
    End Sub

    Private Sub 開くToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 開くToolStripMenuItem.Click
        OpenFileDialog1.InitialDirectory = My.Settings.file
        OpenFileDialog1.ShowDialog()
        If My.Settings.fname <> "" Then
            fn = New Font(My.Settings.fname, 12)
        End If
    End Sub

    Private Sub openFileDialog_OK() Handles OpenFileDialog1.FileOk

        tb = New TextBox
        With tb
            .ScrollBars = ScrollBars.Both
            .Multiline = True
            .Font = fn
            .Size = New Size(TabControl1.Width, TabControl1.Height)
            .Dock = DockStyle.Fill
        End With
        tab = New TabPage
        tab.Text = IO.Path.GetFileName(OpenFileDialog1.FileName)
        tab.Tag = OpenFileDialog1.FileName
        tb.Text = File.ReadAllText(OpenFileDialog1.FileName, System.Text.Encoding.GetEncoding("SHIFT-jis"))
        My.Settings.file = Path.GetDirectoryName(OpenFileDialog1.FileName)
        TabControl1.TabPages.Add(tab)
        tab.Controls.Add(tb)
        TabControl1.SelectedIndex = TabControl1.TabCount + 1
        lineDisp()

    End Sub

    Private Sub 新規作成ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 新規作成ToolStripMenuItem.Click
        'タブ追加
        If TabControl1.TabCount <= 5 Then
            tb = New TextBox
            With tb
                .ScrollBars = ScrollBars.Both
                .Multiline = True
                .Font = fn
                .Size = New Size(TabControl1.Width, TabControl1.Height)
                .Dock = DockStyle.Fill
            End With
            tab = New TabPage
            tab.Text = IO.Path.GetFileName("新規")
            tab.Tag = "新規"
            TabControl1.TabPages.Add(tab)
            tab.Controls.Add(tb)
            lineDisp()
        Else
            MsgBox("一度に編集できるファイルの数は5つまでです!", 16)
        End If
    End Sub

    Private Sub 保存ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存ToolStripMenuItem.Click
        If MsgBox("変更を保存しますか？", 36) = vbYes Then
            Dim fileName As String = TabControl1.SelectedTab.Tag
            If "新規".Equals(TabControl1.SelectedTab.Tag) Then
                fileName = fileName + ".txt"
            End If
            File.WriteAllText(fileName, CType(TabControl1.SelectedTab.Controls(0), TextBox).Text,
                              System.Text.Encoding.GetEncoding("SHIFT-jis"))
        End If
        TabControl1.SelectedTab.Dispose()
        If TabControl1.TabCount = 0 Then
            Application.Exit()
        End If
    End Sub

    Private Sub 閉じるToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 閉じるToolStripMenuItem.Click
        Application.Exit()
    End Sub
End Class
