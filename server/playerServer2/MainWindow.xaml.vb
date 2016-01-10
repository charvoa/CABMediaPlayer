Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic


Class MainWindow
    Dim isRunning As Boolean = False
    Dim port As Integer
    Shared dir As String

    Public Structure thread_Data
        Dim dir As String
        Dim port As Integer
        Dim ar As IAsyncResult
    End Structure

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Dim trd As Thread

        If (isRunning) Then
            statusLabel.Content = "Stopped"
            button.Content = "Start Server"
            isRunning = False
        Else
            statusLabel.Content = "Started"
            button.Content = "Stop Server"
            isRunning = True
            trd = New Thread(AddressOf startNetwork)
            trd.IsBackground = True
            Dim STthread_Data As thread_Data
            STthread_Data.dir = Me.dir
            STthread_Data.port = Me.port
            trd.Start(STthread_Data)
        End If
    End Sub

    Private Sub button1_Click(sender As Object, e As RoutedEventArgs) Handles button1.Click
        Dim win As SettingsWindow = New SettingsWindow()
        win.Show()
        win.main = Me
    End Sub
    Public Sub handleSrv(s As SettingsWindow)
        Dim count As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        count = My.Computer.FileSystem.GetFiles(s.mediaDir.Text)
        label1_Copy.Content = "Running on port " + s.port
        label1.Content = CStr(count.Count) + " File(s) found"
        Me.port = s.port
        Me.dir = s.mediaDir.Text()
        Me.button.IsEnabled = True
    End Sub

    Public Class StateObject
        Public workSocket As Socket = Nothing
        Public Const BufferSize As Integer = 1024
        Public buffer(BufferSize) As Byte
        Public sb As New StringBuilder
    End Class

    Public Shared allDone As New ManualResetEvent(False)
    Public Shared Sub startNetwork(ByVal data As thread_Data)
        Dim bytes() As Byte = New [Byte](1023) {}
        Dim ipHostInfo As IPHostEntry = Dns.Resolve(Dns.GetHostName())
        Dim ipAddress As IPAddress = ipHostInfo.AddressList(0)
        Dim localEndPoint As New IPEndPoint(ipAddress, data.port)

        Dim listener As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        listener.Bind(localEndPoint)
        listener.Listen(100)

        While True
            allDone.Reset()
            Console.WriteLine("Waiting for a connection...")

            listener.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listener)

            allDone.WaitOne()
        End While
    End Sub 'Main


    Public Shared Sub AcceptCallback(ByVal ar As IAsyncResult)
        Console.WriteLine("New client connected")
        Dim listener As Socket = CType(ar.AsyncState, Socket)
        Dim handler As Socket = listener.EndAccept(ar)

        Dim state As New StateObject
        state.workSocket = handler
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
    End Sub


    Public Shared Sub ReadCallback(ByVal ar As IAsyncResult)
        Dim content As String = String.Empty

        Dim state As StateObject = CType(ar.AsyncState, StateObject)
        Dim handler As Socket = state.workSocket

        Dim bytesRead As Integer = handler.EndReceive(ar)

        If bytesRead > 0 Then
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))

            content = state.sb.ToString()
            If (content.StartsWith("listFiles")) Then
                    Dim di As New IO.DirectoryInfo(dir)
                    Dim diar1 As IO.FileInfo() = di.GetFiles()
                    Dim dra As IO.FileInfo
                    content = ""
                    For Each dra In diar1
                    content += dra.ToString() + ":"
                Next
                    Dim byteData = Encoding.ASCII.GetBytes(content)
                    handler.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), handler)
                ElseIf (content.StartsWith("getFile")) Then
                    Dim i As Integer = 0
                    Dim j As Integer
                    content = content.Remove(0, 8)
                content = content.TrimEnd(vbCr, vbLf)
                'content = "video.mp4"
                Console.WriteLine(content)
                Dim inputFile = IO.File.Open(dir + "\" + content, IO.FileMode.Open)

                    Dim byteData(inputFile.Length) As Byte
                    Dim bytes = New Byte(2048 - 1) {}

                    While (j = inputFile.Read(bytes, 0, 2048)) > 0
                        byteData.SetValue(bytes, i)
                        i += j
                    End While

                    inputFile.Close()
                    handler.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), handler)
                Else
                    MsgBox("Bad command received : " + content + " size = " + content.Length.ToString())
                    handler.Shutdown(SocketShutdown.Both)
                    handler.Close()
                    Return
                End If
            End If
    End Sub 'ReadCallback

    Private Shared Sub SendCallback(ByVal ar As IAsyncResult)
            Dim handler As Socket = CType(ar.AsyncState, Socket)

            Dim bytesSent As Integer = handler.EndSend(ar)
            Console.WriteLine("Sent {0} bytes to client.", bytesSent)

            handler.Shutdown(SocketShutdown.Both)
            handler.Close()
            allDone.Set()
        End Sub 'SendCallback
    End Class
