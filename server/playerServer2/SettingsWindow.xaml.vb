Public Class SettingsWindow
    Public port As String
    Public dir As String
    Public main As MainWindow
    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Dim dialog As Windows.Forms.FolderBrowserDialog = New Windows.Forms.FolderBrowserDialog()
        dialog.ShowDialog()
        mediaDir.Text = dialog.SelectedPath()
    End Sub

    Private Sub button1_Click(sender As Object, e As RoutedEventArgs) Handles button1.Click
        dir = mediaDir.Text
        port = textBox.Text
        main.handleSrv(Me)
        Me.Close()
    End Sub
End Class
