'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2022 IP2Location.com
'---------------------------------------------------------------------------
Imports System.Globalization
Imports System.IO
Imports CsvHelper

Public Class Country
    Private resultsDict As Dictionary(Of String, CountryInfo)
    Private resultsList As List(Of CountryInfo)

    ' Description: Parses the country information CSV and stores the data
    Public Sub New(ByVal CSVFile As String)
        If Not File.Exists(CSVFile) Then
            Throw New Exception("The CSV file '" & CSVFile & "' is not found.")
        End If
        resultsDict = New Dictionary(Of String, CountryInfo)
        resultsList = New List(Of CountryInfo)()

        Using reader = New StreamReader(CSVFile)
            Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
                Try
                    resultsList = csv.GetRecords(Of CountryInfo)().ToList()
                    If resultsList.Count > 0 AndAlso resultsList.Item(0).country_code.Trim = "" Then
                        Throw New Exception("Unable to read '" & CSVFile & "'.")
                    End If
                    Dim x As Integer
                    For x = 0 To resultsList.Count - 1
                        resultsDict.Add(resultsList.Item(x).country_code, resultsList.Item(x))
                    Next
                Catch ex As HeaderValidationException
                    Throw New Exception("Invalid country information CSV file.")
                End Try
            End Using
        End Using
    End Sub

    ' Get country information for the supplied country code
    Public Function GetCountryInfo(ByVal CountryCode As String) As CountryInfo
        If resultsDict.Count = 0 Then
            Throw New Exception("No record available.")
        End If
        If CountryCode.Trim = "" OrElse Not resultsDict.ContainsKey(CountryCode) Then
            Return Nothing
        ElseIf CountryCode <> "" Then
            Return resultsDict.Item(CountryCode)
        End If
    End Function

    ' Get country information for all countries
    Public Function GetCountryInfo() As List(Of CountryInfo)
        If resultsList.Count = 0 Then
            Throw New Exception("No record available.")
        End If
        Return resultsList
    End Function
End Class
