﻿Public Class Form1

    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub AxWindowsMediaPlayer1_Enter(sender As Object, e As EventArgs) Handles AxWindowsMediaPlayer1.Enter
        'AxWindowsMediaPlayer1.enableContextMenu = False
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)

        End Sub

        Private Sub ToolStripDropDownButton1_Click_1(sender As Object, e As EventArgs) Handles ToolStripDropDownButton1.Click

        End Sub

        Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
            OpenFileDialog1.AddExtension = True
            OpenFileDialog1.DefaultExt = "*.*"
            OpenFileDialog1.Filter = "Media(*.*)|*.*"
            OpenFileDialog1.ShowDialog()

            AxWindowsMediaPlayer1.currentPlaylist.appendItem(AxWindowsMediaPlayer1.newMedia(OpenFileDialog1.FileName))
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            'AxWindowsMediaPlayer1.play
        End Sub

        Private Sub SelectServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectServerToolStripMenuItem.Click
            Dim Win As Dialog1 = New Dialog1()
            Win.Show()
        End Sub

        Private Sub About_Click(sender As Object, e As EventArgs) Handles About.Click
            Dim Win As AboutBox1 = New AboutBox1()
            Win.Show()
        End Sub

        Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

        End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub AxWindowMediaPlayer1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            Dim files() As String
            Dim i As Integer

            files = e.Data.GetData(DataFormats.FileDrop)
            For i = 0 To files.Length - 1
                AxWindowsMediaPlayer1.currentPlaylist.appendItem(AxWindowsMediaPlayer1.newMedia(files(i)))
            Next
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        End If
    End Sub
End Class
