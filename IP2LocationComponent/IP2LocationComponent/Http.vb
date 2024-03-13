'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2024 IP2Location.com
'---------------------------------------------------------------------------
Imports System.Net
Imports System.Text

Public Class Http
    Public Function GetMethod(url As String) As String
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse
        request = WebRequest.Create(url)
        request.Method = "GET"
        response = request.GetResponse()
        If response.StatusCode = HttpStatusCode.OK Then
            Dim reader As New IO.StreamReader(response.GetResponseStream())
            Dim raw As String = reader.ReadToEnd
            Return raw
        Else
            Return ("Failed : HTTP error code :" & response.StatusCode)
        End If
    End Function
    Public Function PostMethod(url As String, post As String) As String
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse
        Dim encode As UTF8Encoding
        Dim postdata As String = post
        Dim postdatabytes As Byte()

        request = WebRequest.Create(url)
        encode = New UTF8Encoding()
        postdatabytes = encode.GetBytes(postdata)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = postdatabytes.Length

        Using stream = request.GetRequestStream
            stream.Write(postdatabytes, 0, postdatabytes.Length)
        End Using
        response = request.GetResponse()
        If response.StatusCode = HttpStatusCode.OK Then
            Dim reader As New IO.StreamReader(response.GetResponseStream())
            Dim raw As String = reader.ReadToEnd
            Return raw
        Else
            Return ("Failed : HTTP error code :" & response.StatusCode)
        End If
    End Function
End Class