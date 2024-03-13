'--------------------------------------------------------------------------
' Title        : IP2Location .NET Component
' Description  : This component lookup the IP2Location data from an IP address using the IP2Location Web Service.
'                Things that we can do with this script.
'                		1. Redirect based on country 
'                		2. Digital rights management 
'                		3. Web log stats and analysis 
'                		4. Auto-selection of country on forms 
'                		5. Filter access from countries you do not do business with 
'                		6. Geo-targeting for increased sales and click-thrus 
'                		7. And much, much more! 
' Requirements : .NET Standard 2.0
'
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2024 IP2Location.com
'
'---------------------------------------------------------------------------
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public NotInheritable Class ComponentWebService
    Private _APIKey As String = ""
    Private _Package As String = ""
    Private _UseSSL As Boolean = True
    Private ReadOnly _RegexAPIKey As New Regex("^[\dA-Z]{10}$")
    Private ReadOnly _RegexPackage As New Regex("^WS\d+$")
    Private Const BASE_URL As String = "api.ip2location.com/v2/"

    ' Description: Initialize
    Public Sub New()
    End Sub

    ' Description: Set the API key and package for the queries
    Public Sub Open(APIKey As String, Package As String, Optional UseSSL As Boolean = True)
        _APIKey = APIKey
        _Package = Package
        _UseSSL = UseSSL

        CheckParams()
    End Sub

    ' Description: Validate API key and package
    Private Sub CheckParams()
        If Not _RegexAPIKey.IsMatch(_APIKey) AndAlso _APIKey <> "demo" Then
            Throw New Exception("Invalid API key.")
        End If

        If Not _RegexPackage.IsMatch(_Package) Then
            Throw New Exception("Invalid package name.")
        End If
    End Sub

    ' Description: Query web service to get location information by IP address and translations
    Public Function IPQuery(IP As String, Optional Language As String = "en") As JObject
        Return IPQuery(IP, Nothing, Language)
    End Function

    ' Description: Query web service to get location information by IP address, addons and translations
    Public Function IPQuery(IP As String, AddOns() As String, Optional Language As String = "en") As JObject
        CheckParams() ' check here in case user haven't called Open yet

        Dim url As String
        Dim protocol As String = If(_UseSSL, "https", "http")
        url = protocol & "://" & BASE_URL & "?key=" & _APIKey & "&package=" & _Package & "&ip=" & Net.WebUtility.UrlEncode(IP) & "&lang=" & Net.WebUtility.UrlEncode(Language)
        If AddOns IsNot Nothing Then
            url &= "&addon=" & Net.WebUtility.UrlEncode(String.Join(",", AddOns))
        End If
        Dim request As New Http
        Dim rawjson As String
        rawjson = request.GetMethod(url)
        Dim results = JsonConvert.DeserializeObject(Of Object)(rawjson)

        Return results
    End Function

    ' Description: Check web service credit balance
    Public Function GetCredit() As JObject
        CheckParams() ' check here in case user haven't called Open yet

        Dim url As String
        Dim protocol As String = If(_UseSSL, "https", "http")
        url = protocol & "://" & BASE_URL & "?key=" & _APIKey & "&check=true"
        Dim request As New Http
        Dim rawjson As String
        rawjson = request.GetMethod(url)
        Dim results = JsonConvert.DeserializeObject(Of Object)(rawjson)

        Return results
    End Function

End Class
