Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Sockets


Public Class Dialog1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Dim port As Integer
        Dim ipAddr As String
        Dim client As TcpClient

        Try
            'port = TextBoxPort.Text()
            'ipAddr = TextBoxIP.Text()
            port = 4242
            ipAddr = "127.0.0.1"
            Console.WriteLine(ipAddr)
            Console.WriteLine(port)
            client = New TcpClient(ipAddr.ToString, port)
            Dim stream As NetworkStream
            stream = client.GetStream

            Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("listFiles" + vbCrLf)
            Dim dataRead As [Byte]() = System.Text.Encoding.ASCII.GetBytes("")

            Console.Write(data)
            stream.Write(data, 0, data.Length)

            Dim bytesRead As Integer
            Dim buffer(4096) As Byte

            bytesRead = stream.Read(buffer, 0, buffer.Length)
            If bytesRead > 0 Then
                Console.Write(bytesRead)
                Dim arrayFiles() As String = Split(System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead), ":")
                Dim result As New System.Text.StringBuilder
                Dim counter As Integer

                result.Append("{")
                For counter = 0 To arrayFiles.Length - 1
                    result.Append(arrayFiles(counter).ToString())
                    If (counter < (arrayFiles.Length - 1)) Then result.Append(",")
                Next counter
                result.Append("}")

                Console.WriteLine(result.ToString())

                'Debug.Print(System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead))
            End If

        Catch ex As Exception
            Console.WriteLine("Exception ...")
        End Try

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPort.TextChanged

    End Sub

    Private Sub TextBoxIP_TextChanged(sender As Object, e As EventArgs) Handles TextBoxIP.TextChanged

    End Sub
End Class
