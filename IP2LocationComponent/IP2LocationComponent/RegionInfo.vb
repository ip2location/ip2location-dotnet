Imports CsvHelper.Configuration.Attributes

Public Class RegionInfo
    Public Property country_code As String
    <Name("subdivision_name")>
    Public Property name As String
    Public Property code As String
End Class
