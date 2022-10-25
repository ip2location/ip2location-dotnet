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

Public Class Region
    Private resultsDict As Dictionary(Of String, List(Of RegionInfo))

    ' Description: Parses the region information CSV and stores the data
    Public Sub New(ByVal CSVFile As String)
        If Not File.Exists(CSVFile) Then
            Throw New Exception("The CSV file '" & CSVFile & "' is not found.")
        End If
        resultsDict = New Dictionary(Of String, List(Of RegionInfo))

        Using reader = New StreamReader(CSVFile)
            Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
                Try
                    Dim resultsList As List(Of RegionInfo) = csv.GetRecords(Of RegionInfo)().ToList()
                    If resultsList.Count > 0 AndAlso resultsList.Item(0).name.Trim = "" Then
                        Throw New Exception("Unable to read '" & CSVFile & "'.")
                    End If
                    Dim x As Integer
                    For x = 0 To resultsList.Count - 1
                        If Not resultsDict.ContainsKey(resultsList.Item(x).country_code) Then
                            resultsDict.Add(resultsList.Item(x).country_code, New List(Of RegionInfo))
                        End If
                        resultsDict.Item(resultsList.Item(x).country_code).Add(resultsList.Item(x))
                    Next
                Catch ex As HeaderValidationException
                    Throw New Exception("Invalid region information CSV file.")
                End Try
            End Using
        End Using
    End Sub

    ' Get region code for the supplied country code and region name
    Public Function GetRegionCode(ByVal CountryCode As String, ByVal RegionName As String) As String
        If resultsDict.Count = 0 Then
            Throw New Exception("No record available.")
        End If
        If CountryCode.Trim = "" OrElse Not resultsDict.ContainsKey(CountryCode) Then
            Return Nothing
        End If

        Dim x As Integer
        For x = 0 To resultsDict.Item(CountryCode).Count - 1
            If resultsDict.Item(CountryCode).Item(x).name.ToUpper = RegionName.ToUpper Then
                Return resultsDict.Item(CountryCode).Item(x).code
            End If
        Next

        Return Nothing
    End Function

End Class
