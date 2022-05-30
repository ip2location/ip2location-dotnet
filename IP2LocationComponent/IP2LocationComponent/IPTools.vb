'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2022 IP2Location.com
'---------------------------------------------------------------------------
Imports System.IO
Imports System.Net
Imports System.Numerics
Imports System.Text.RegularExpressions

Public Class IPTools
    Private Const MAX_IPV4_RANGE As Long = 4294967295
    Private MAX_IPV6_RANGE As BigInteger = BigInteger.Pow(2, 128) - 1

    ' Description: Checks if IP address is IPv6
    Public Function IsIPv4(ByVal IP As String) As Boolean
        Try
            Dim address As IPAddress = Nothing

            If IPAddress.TryParse(IP, address) Then
                Select Case address.AddressFamily
                    Case Sockets.AddressFamily.InterNetwork
                        Return True
                    Case Else
                        Return False
                End Select
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ' Description: Checks if IP address is IPv6
    Public Function IsIPv6(ByVal IP As String) As Boolean
        Try
            Dim address As IPAddress = Nothing

            If IPAddress.TryParse(IP, address) Then
                Select Case address.AddressFamily
                    Case Sockets.AddressFamily.InterNetworkV6
                        Return True
                    Case Else
                        Return False
                End Select
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ' Description: Reverse the bytes if system is little endian
    Private Sub LittleEndian(ByRef byteArr() As Byte)
        If BitConverter.IsLittleEndian Then
            Dim byteList As New List(Of Byte)(byteArr)
            byteList.Reverse()
            byteArr = byteList.ToArray()
        End If
    End Sub

    ' Description: Convert either IPv4 or IPv6 into big integer
    Private Function IPNo(ByRef IP As IPAddress) As BigInteger
        Try
            Dim addrbytes() As Byte = IP.GetAddressBytes()
            LittleEndian(addrbytes)

            Dim final As BigInteger

            If addrbytes.Length > 8 Then
                'IPv6
                final = BitConverter.ToUInt64(addrbytes, 8)
                final <<= 64
                final += BitConverter.ToUInt64(addrbytes, 0)
            Else
                'IPv4
                final = BitConverter.ToUInt32(addrbytes, 0)
            End If

            Return final
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ' Description: Convert IPv4 into big integer
    Public Function IPv4ToDecimal(ByVal IP As String) As BigInteger
        If IsIPv4(IP) Then
            Return IPNo(IPAddress.Parse(IP))
        Else
            Return Nothing
        End If
    End Function

    ' Description: Convert IPv6 into big integer
    Public Function IPv6ToDecimal(ByVal IP As String) As BigInteger
        If IsIPv6(IP) Then
            Return IPNo(IPAddress.Parse(IP))
        Else
            Return Nothing
        End If
    End Function

    Private Function NumToIPv4(ByVal IPNum As BigInteger) As String
        Dim result As String
        Dim arr As Byte()
        Dim str(3) As String
        arr = IPNum.ToByteArray()
        Dim x As Integer

        LittleEndian(arr)

        If arr.Length > 4 Then
            arr = arr.Skip(1).ToArray() ' remove the 2's complement signed bit
        End If

        If arr.Length < 4 Then
            ' need to insert missing bytes to the front of the array
            Dim list As List(Of Byte) = arr.ToList()
            For x = 1 To (4 - arr.Length)
                list.Insert(0, 0)
            Next
            arr = list.ToArray()
        End If

        For x = arr.GetLowerBound(0) To arr.GetUpperBound(0)
            str(x) = arr(x).ToString()
        Next
        result = String.Join(".", str)
        Return result
    End Function

    ' Description: Convert big integer into IPv4
    Public Function DecimalToIPv4(ByVal IPNum As BigInteger) As String
        If IPNum < 0 OrElse IPNum > MAX_IPV4_RANGE Then
            Return Nothing
        Else
            Return NumToIPv4(IPNum)
        End If
    End Function

    Private Function NumToIPv6(ByVal IPNum As BigInteger) As String
        Dim result As String = ""
        Dim arr As Byte()
        Dim str(7) As String
        Dim x As Integer
        arr = IPNum.ToByteArray()

        LittleEndian(arr)

        If arr.Length > 16 Then
            arr = arr.Skip(1).ToArray() ' remove the 2's complement signed bit
        End If

        If arr.Length < 16 Then
            ' need to insert missing bytes to the front of the array
            Dim list As List(Of Byte) = arr.ToList()
            For x = 1 To (16 - arr.Length)
                list.Insert(0, 0)
            Next
            arr = list.ToArray()
        End If

        For x = arr.GetLowerBound(0) To arr.GetUpperBound(0)
            result &= arr(x).ToString("x2").PadLeft(2, "0")
        Next

        result = Regex.Replace(result, ".{4}", "$0:").TrimEnd(":")
        result = CompressIPv6(result)
        Return result
    End Function

    ' Description: Convert big integer into IPv6
    Public Function DecimalToIPv6(ByVal IPNum As BigInteger) As String
        If IPNum < 0 OrElse IPNum > MAX_IPV6_RANGE Then
            Return Nothing
        Else
            Return NumToIPv6(IPNum)
        End If
    End Function

    ' Description: Convert IPv6 into compressed form
    Public Function CompressIPv6(ByVal IP As String) As String
        Try
            Dim address As IPAddress = Nothing
            If IsIPv6(IP) Then
                address = IPAddress.Parse(IP)
                Return address.ToString()
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ' Description: Convert IPv6 into expanded form
    Public Function ExpandIPv6(ByVal IP As String) As String
        Try
            Dim address As IPAddress = Nothing
            Dim result As String = ""
            If IsIPv6(IP) Then
                address = IPAddress.Parse(IP)

                Dim addrBytes() As Byte = address.GetAddressBytes()

                Dim x As Integer
                For x = addrBytes.GetLowerBound(0) To addrBytes.GetUpperBound(0)
                    result &= addrBytes(x).ToString("x2").PadLeft(2, "0")
                Next

                result = Regex.Replace(result, ".{4}", "$0:").TrimEnd(":")

                Return result
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ' Description: Convert IPv4 range into CIDR
    Public Function IPv4ToCIDR(ByVal IPFrom As String, ByVal IPTo As String) As List(Of String)
        If Not IsIPv4(IPFrom) OrElse Not IsIPv4(IPTo) Then
            Return Nothing
        End If

        Dim startip As Long = IPv4ToDecimal(IPFrom)
        Dim endip As Long = IPv4ToDecimal(IPTo)

        Dim result = New List(Of String)

        While endip >= startip
            Dim maxsize As Byte = 32

            While (maxsize > 0)
                Dim mask As Long = IPv4Mask(maxsize - 1)
                Dim maskbase As Long = startip And mask

                If maskbase <> startip Then
                    Exit While
                End If

                maxsize -= 1
            End While

            Dim x As Double = Math.Log(endip - startip + 1) / Math.Log(2)
            Dim maxdiff As Byte = 32 - Math.Floor(x)

            If maxsize < maxdiff Then
                maxsize = maxdiff
            End If

            Dim ip As String = DecimalToIPv4(startip)
            result.Add(ip + "/" + Convert.ToString(maxsize))
            startip += Math.Pow(2, 32 - maxsize)
        End While
        Return result
    End Function

    Private Function IPv4Mask(ByVal s As Integer) As Long
        Return Math.Pow(2, 32) - Math.Pow(2, 32 - s)
    End Function

    ' Description: Convert IPv6 range into CIDR
    Public Function IPv6ToCIDR(ByVal IPFrom As String, ByVal IPTo As String) As List(Of String)
        If Not IsIPv6(IPFrom) OrElse Not IsIPv6(IPTo) Then
            Return Nothing
        End If

        Dim ipfrombin = IPToBinary(IPFrom)
        Dim iptobin = IPToBinary(IPTo)

        Dim result = New List(Of String)
        Dim networksize = 0
        Dim shift As Integer
        Dim networks = New Dictionary(Of String, Integer)
        Dim n As Integer
        Dim vals = New List(Of Integer)

        If String.Compare(ipfrombin, iptobin) = 0 Then
            result.Add(IPFrom & "/128")
            Return result
        End If

        If String.Compare(ipfrombin, iptobin) > 0 Then
            Dim tmp = ipfrombin
            ipfrombin = iptobin
            iptobin = tmp
        End If

        Do
            If ipfrombin(ipfrombin.Length - 1).ToString() = "1" Then
                networks.Add(ipfrombin.Substring(networksize, 128 - networksize).PadRight(128, "0"), 128 - networksize)
                n = ipfrombin.LastIndexOf("0")
                ipfrombin = If(n = 0, "", ipfrombin.Substring(0, n)) & "1"
                ipfrombin = ipfrombin.PadRight(128, "0")
            End If

            If iptobin(iptobin.Length - 1).ToString() = "0" Then
                networks.Add(iptobin.Substring(networksize, 128 - networksize).PadRight(128, "0"), 128 - networksize)
                n = iptobin.LastIndexOf("1")
                iptobin = If(n = 0, "", iptobin.Substring(0, n)) & "0"
                iptobin = iptobin.PadRight(128, "1")
            End If

            If String.Compare(iptobin, ipfrombin) < 0 Then
                Continue Do
            End If

            vals.Clear()
            vals.Add(ipfrombin.LastIndexOf("0"))
            vals.Add(iptobin.LastIndexOf("1"))
            shift = 128 - vals.Max()

            ipfrombin = ipfrombin.Substring(0, 128 - shift).PadLeft(128, "0")
            iptobin = iptobin.Substring(0, 128 - shift).PadLeft(128, "0")
            networksize += shift

            If String.Compare(ipfrombin, iptobin) = 0 Then
                networks.Add(ipfrombin.Substring(networksize, 128 - networksize).PadRight(128, "0"), 128 - networksize)
                Continue Do
            End If

        Loop While String.Compare(ipfrombin, iptobin) < 0

        ' Get list of keys.
        Dim keys As List(Of String) = networks.Keys.ToList

        ' Sort the keys.
        keys.Sort()

        For Each ip As String In keys
            result.Add(BinaryToIP(ip) & "/" & networks.Item(ip))
        Next

        Return result
    End Function

    ' Description: Convert IPv6 into binary string representation
    Private Function IPToBinary(ByVal IP As String) As String
        If IsIPv6(IP) Then
            Dim address As IPAddress
            address = IPAddress.Parse(IP)
            Dim addrbytes() As Byte = address.GetAddressBytes()

            Dim x As Integer
            Dim result As String = ""

            For x = addrbytes.GetLowerBound(0) To addrbytes.GetUpperBound(0)
                result &= Convert.ToString(addrbytes(x), 2).PadLeft(8, "0")
            Next
            Return result
        Else
            Return Nothing
        End If
    End Function

    ' Description: Convert binary string representation into IPv6
    Private Function BinaryToIP(ByVal Binary As String) As String
        If Not Regex.Match(Binary, "^([01]{8})+$").Success OrElse Binary.Length <> 128 Then
            Return Nothing
        End If

        Dim octets = Regex.Matches(Binary, "[01]{8}")
        Dim m As Match
        Dim result As String
        Dim list = New List(Of Byte)
        For Each m In octets
            list.Add(Convert.ToByte(m.ToString(), 2))
        Next
        Dim address = New IPAddress(list.ToArray())
        result = address.ToString()
        Return result
    End Function

    ' Description: Convert CIDR into IPv4 range
    Public Function CIDRToIPv4(ByVal CIDR As String) As (IPStart As String, IPEnd As String)
        If CIDR.IndexOf("/") = -1 Then
            Return Nothing
        End If

        Dim ip As String
        Dim prefix As Long
        Dim arr = CIDR.Split("/")
        Dim ipstart As String
        Dim ipend As String
        Dim ipstartlong As Long
        Dim ipendlong As Long
        Dim total As Long

        If arr.Length <> 2 OrElse Not IsIPv4(arr(0)) OrElse Not Regex.Match(arr(1), "^[0-9]{1,2}$").Success OrElse Convert.ToInt64(arr(1)) > 32 Then
            Return Nothing
        End If

        ip = arr(0)
        prefix = Convert.ToInt64(arr(1))

        ipstartlong = IPv4ToDecimal(ip)
        ipstartlong = ipstartlong And (-1 << (32 - prefix))
        ipstart = DecimalToIPv4(ipstartlong)

        total = 1 << (32 - prefix)

        ipendlong = ipstartlong + total - 1

        If ipendlong > 4294967295 Then
            ipendlong = 4294967295
        End If

        ipend = DecimalToIPv4(ipendlong)

        Dim result = (ipstart, ipend)

        Return result
    End Function

    ' Description: Convert CIDR into IPv6 range
    Public Function CIDRToIPv6(ByVal CIDR As String) As (IPStart As String, IPEnd As String)
        If CIDR.IndexOf("/") = -1 Then
            Return Nothing
        End If

        Dim ip As String
        Dim prefix As Integer
        Dim arr = CIDR.Split("/")

        If arr.Length <> 2 OrElse Not IsIPv6(arr(0)) OrElse Not Regex.Match(arr(1), "^[0-9]{1,3}$").Success OrElse Convert.ToInt64(arr(1)) > 128 Then
            Return Nothing
        End If

        ip = arr(0)
        prefix = Convert.ToInt64(arr(1))

        Dim hexstartaddress = ExpandIPv6(ip).Replace(":", "")
        Dim hexlastaddress = hexstartaddress

        Dim bits = 128 - prefix
        Dim x As Integer
        Dim y As String
        Dim pos = 31
        Dim vals = New List(Of Integer)
        While bits > 0
            vals.Clear()
            vals.Add(4)
            vals.Add(bits)
            x = Convert.ToInt32(hexlastaddress.Substring(pos, 1), 16)
            y = (x Or (Math.Pow(2, vals.Min()) - 1)).ToString("x") ' single hex char

            hexlastaddress = hexlastaddress.Remove(pos, 1).Insert(pos, y)

            bits -= 4
            pos -= 1
        End While

        hexstartaddress = Regex.Replace(hexstartaddress, ".{4}", "$0:").TrimEnd(":")
        hexlastaddress = Regex.Replace(hexlastaddress, ".{4}", "$0:").TrimEnd(":")

        Return (hexstartaddress, hexlastaddress)
    End Function
End Class
