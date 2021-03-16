'--------------------------------------------------------------------------
' Title        : IP2Location .NET Component
' Description  : This component lookup the IP2Location database from an IP address.
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
' Copyright (c) 2002-2021 IP2Location.com
'
'---------------------------------------------------------------------------
Imports System.IO
Imports System.IO.MemoryMappedFiles
Imports System.Net
Imports System.Text
Imports System.DateTime
Imports System.Numerics
Imports System.Text.RegularExpressions

<Assembly: System.Runtime.InteropServices.ComVisible(False)>
Public NotInheritable Class Component
    Private _LockLoadBIN As New Object
    Private _DBFilePath As String = ""
    Private _LicenseFilePath As String
    Private _MetaData As MetaData = Nothing
    Private _MMF As MemoryMappedFile = Nothing
    'Private _IndexAccessor As MemoryMappedViewAccessor = Nothing
    'Private _IndexOffset As Integer = 0
    Private _IndexArrayIPv4(65535, 1) As Integer
    Private _IndexArrayIPv6(65535, 1) As Integer
    Private _IPv4Accessor As MemoryMappedViewAccessor = Nothing
    Private _IPv4Offset As Integer = 0
    Private _IPv6Accessor As MemoryMappedViewAccessor = Nothing
    Private _IPv6Offset As Integer = 0
    Private _MapDataAccessor As MemoryMappedViewAccessor = Nothing
    Private _MapDataOffset As Integer = 0
    Private _OutlierCase1 As Regex = New Regex("^:(:[\dA-F]{1,4}){7}$", RegexOptions.IgnoreCase)
    Private _OutlierCase2 As Regex = New Regex("^:(:[\dA-F]{1,4}){5}:(\d{1,3}\.){3}\d{1,3}$", RegexOptions.IgnoreCase)
    Private _OutlierCase3 As Regex = New Regex("^\d+$")
    Private _OutlierCase4 As Regex = New Regex("^([\dA-F]{1,4}:){6}(0\d+\.|.*?\.0\d+).*$")
    Private _OutlierCase5 As Regex = New Regex("^(\d+\.){1,2}\d+$")
    Private _IPv4MappedRegex As Regex = New Regex("^(.*:)((\d+\.){3}\d+)$")
    Private _IPv4MappedRegex2 As Regex = New Regex("^.*((:[\dA-F]{1,4}){2})$")
    Private _IPv4CompatibleRegex As Regex = New Regex("^::[\dA-F]{1,4}$", RegexOptions.IgnoreCase)
    Private _UseMemoryMappedFile As Boolean = False
    Private _IPv4ColumnSize As Integer = 0
    Private _IPv6ColumnSize As Integer = 0
    Private _MapFileName As String = "MyBIN" ' If running multiple websites with different application pools, every website must have different mapped file name

    Private _fromBI As New BigInteger(281470681743360)
    Private _toBI As New BigInteger(281474976710655)
    Private _FromBI2 As BigInteger = BigInteger.Parse("42545680458834377588178886921629466624")
    Private _ToBI2 As BigInteger = BigInteger.Parse("42550872755692912415807417417958686719")
    Private _FromBI3 As BigInteger = BigInteger.Parse("42540488161975842760550356425300246528")
    Private _ToBI3 As BigInteger = BigInteger.Parse("42540488241204005274814694018844196863")
    Private _DivBI As BigInteger = New BigInteger(4294967295)

    Private Const MAX_IPV4_RANGE As Long = 4294967295
    Private MAX_IPV6_RANGE As BigInteger = BigInteger.Pow(2, 128) - 1
    Private Const MSG_OK As String = "OK"
    Private Const MSG_NOT_SUPPORTED As String = "This method is not applicable for current IP2Location binary data file. Please upgrade your subscription package to install new data file."

    Private COUNTRY_POSITION() As Byte = {0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}
    Private REGION_POSITION() As Byte = {0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3}
    Private CITY_POSITION() As Byte = {0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}
    Private ISP_POSITION() As Byte = {0, 0, 3, 0, 5, 0, 7, 5, 7, 0, 8, 0, 9, 0, 9, 0, 9, 0, 9, 7, 9, 0, 9, 7, 9}
    Private LATITUDE_POSITION() As Byte = {0, 0, 0, 0, 0, 5, 5, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5}
    Private LONGITUDE_POSITION() As Byte = {0, 0, 0, 0, 0, 6, 6, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6}
    Private DOMAIN_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 6, 8, 0, 9, 0, 10, 0, 10, 0, 10, 0, 10, 8, 10, 0, 10, 8, 10}
    Private ZIPCODE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 0, 7, 7, 7, 0, 7, 0, 7, 7, 7, 0, 7}
    Private TIMEZONE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 7, 8, 8, 8, 7, 8, 0, 8, 8, 8, 0, 8}
    Private NETSPEED_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 11, 0, 11, 8, 11, 0, 11, 0, 11, 0, 11}
    Private IDDCODE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 12, 0, 12, 0, 12, 9, 12, 0, 12}
    Private AREACODE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 13, 0, 13, 0, 13, 10, 13, 0, 13}
    Private WEATHERSTATIONCODE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 14, 0, 14, 0, 14, 0, 14}
    Private WEATHERSTATIONNAME_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 15, 0, 15, 0, 15, 0, 15}
    Private MCC_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 16, 0, 16, 9, 16}
    Private MNC_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 17, 0, 17, 10, 17}
    Private MOBILEBRAND_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 18, 0, 18, 11, 18}
    Private ELEVATION_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 19, 0, 19}
    Private USAGETYPE_POSITION() As Byte = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 20}

    Private COUNTRY_POSITION_OFFSET As Integer = 0
    Private REGION_POSITION_OFFSET As Integer = 0
    Private CITY_POSITION_OFFSET As Integer = 0
    Private ISP_POSITION_OFFSET As Integer = 0
    Private DOMAIN_POSITION_OFFSET As Integer = 0
    Private ZIPCODE_POSITION_OFFSET As Integer = 0
    Private LATITUDE_POSITION_OFFSET As Integer = 0
    Private LONGITUDE_POSITION_OFFSET As Integer = 0
    Private TIMEZONE_POSITION_OFFSET As Integer = 0
    Private NETSPEED_POSITION_OFFSET As Integer = 0
    Private IDDCODE_POSITION_OFFSET As Integer = 0
    Private AREACODE_POSITION_OFFSET As Integer = 0
    Private WEATHERSTATIONCODE_POSITION_OFFSET As Integer = 0
    Private WEATHERSTATIONNAME_POSITION_OFFSET As Integer = 0
    Private MCC_POSITION_OFFSET As Integer = 0
    Private MNC_POSITION_OFFSET As Integer = 0
    Private MOBILEBRAND_POSITION_OFFSET As Integer = 0
    Private ELEVATION_POSITION_OFFSET As Integer = 0
    Private USAGETYPE_POSITION_OFFSET As Integer = 0

    Private COUNTRY_ENABLED As Boolean = False
    Private REGION_ENABLED As Boolean = False
    Private CITY_ENABLED As Boolean = False
    Private ISP_ENABLED As Boolean = False
    Private DOMAIN_ENABLED As Boolean = False
    Private ZIPCODE_ENABLED As Boolean = False
    Private LATITUDE_ENABLED As Boolean = False
    Private LONGITUDE_ENABLED As Boolean = False
    Private TIMEZONE_ENABLED As Boolean = False
    Private NETSPEED_ENABLED As Boolean = False
    Private IDDCODE_ENABLED As Boolean = False
    Private AREACODE_ENABLED As Boolean = False
    Private WEATHERSTATIONCODE_ENABLED As Boolean = False
    Private WEATHERSTATIONNAME_ENABLED As Boolean = False
    Private MCC_ENABLED As Boolean = False
    Private MNC_ENABLED As Boolean = False
    Private MOBILEBRAND_ENABLED As Boolean = False
    Private ELEVATION_ENABLED As Boolean = False
    Private USAGETYPE_ENABLED As Boolean = False

    ' Description: Gets or sets whether to use memory mapped file instead of filestream
    Public Property UseMemoryMappedFile() As Boolean
        Get
            Return _UseMemoryMappedFile
        End Get
        Set(ByVal Value As Boolean)
            _UseMemoryMappedFile = Value
        End Set
    End Property

    ' Description: Gets or sets the memory mapped file name
    Public Property MapFileName() As String
        Get
            Return _MapFileName
        End Get
        Set(ByVal Value As String)
            _MapFileName = Value
        End Set
    End Property

    ' Description: Returns the IP database version
    Public ReadOnly Property IPVersion() As String
        Get
            Dim returnval As String = ""
            If _MetaData Is Nothing Then
                If LoadBIN() Then
                    returnval = _MetaData.IPVersion
                End If
            End If
            Return returnval
        End Get
    End Property

    ' Description: Set/Get the value of IPv4+IPv6 database path
    Public Property IPDatabasePath() As String
        Get
            Return _DBFilePath
        End Get
        Set(ByVal Value As String)
            _DBFilePath = Value
        End Set
    End Property

    ' Description: Set/Get the value of license key path (DEPRECATED)
    Public Property IPLicensePath() As String
        Get
            Return _LicenseFilePath
        End Get
        Set(ByVal Value As String)
            _LicenseFilePath = Value
        End Set
    End Property

    ' Description: Set the parameters and perform BIN pre-loading
    Public Sub Open(ByVal DBPath As String, Optional ByVal UseMMF As Boolean = False)
        IPDatabasePath = DBPath
        UseMemoryMappedFile = UseMMF

        LoadBIN()
    End Sub

    ' Description: Create memory mapped file
    Private Sub CreateMemoryMappedFile()
        'Using MyBIN instead of Global\MyBIN is coz the newer OSes don't grant permission to create global shared memory object
        'So new style is using localised memory object but using Global.asax.vb file to initialise and share the object
        If _MMF Is Nothing Then
            Try
                'Throw New Exception ' for debugging
                _MMF = MemoryMappedFile.OpenExisting(_MapFileName, MemoryMappedFileRights.Read)
            Catch ex As Exception
                Try
                    'Throw New Exception ' for debugging
                    _MMF = MemoryMappedFile.CreateFromFile(_DBFilePath, FileMode.Open, _MapFileName, New FileInfo(_DBFilePath).Length, MemoryMappedFileAccess.Read)
                Catch ex2 As Exception
                    'Throw ex2
                    Try
                        Dim len As Long = New FileInfo(_DBFilePath).Length
                        _MMF = MemoryMappedFile.CreateNew(_MapFileName, len, MemoryMappedFileAccess.ReadWrite)
                        Using stream As MemoryMappedViewStream = _MMF.CreateViewStream()
                            Using writer As BinaryWriter = New BinaryWriter(stream)
                                Using fs As FileStream = New FileStream(_DBFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                                    Dim buff(len) As Byte
                                    fs.Read(buff, 0, buff.Length)
                                    writer.Write(buff, 0, buff.Length)
                                End Using
                            End Using
                        End Using
                    Catch ex3 As Exception
                        'Throw ex3

                        ' this part onwards trying Linux specific stuff (no named map)
                        Try
                            'Throw New Exception ' for debugging
                            _MMF = MemoryMappedFile.OpenExisting(Nothing, MemoryMappedFileRights.Read)
                        Catch ex4 As Exception
                            Try
                                'Throw New Exception ' for debugging
                                _MMF = MemoryMappedFile.CreateFromFile(_DBFilePath, FileMode.Open, Nothing, New FileInfo(_DBFilePath).Length, MemoryMappedFileAccess.Read)
                            Catch ex5 As Exception
                                'Throw ex5
                                Dim len As Long = New FileInfo(_DBFilePath).Length
                                _MMF = MemoryMappedFile.CreateNew(Nothing, len, MemoryMappedFileAccess.ReadWrite)
                                Using stream As MemoryMappedViewStream = _MMF.CreateViewStream()
                                    Using writer As BinaryWriter = New BinaryWriter(stream)
                                        Using fs As FileStream = New FileStream(_DBFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                                            Dim buff(len) As Byte
                                            fs.Read(buff, 0, buff.Length)
                                            writer.Write(buff, 0, buff.Length)
                                        End Using
                                    End Using
                                End Using
                            End Try
                        End Try
                    End Try
                End Try
            End Try
        End If
    End Sub

    ' Description: Destroy memory mapped file
    Private Sub DestroyMemoryMappedFile()
        If Not _MMF Is Nothing Then
            _MMF.Dispose()
            _MMF = Nothing
        End If
    End Sub

    ' Description: Create memory accessors
    Private Sub CreateAccessors()
        If _IPv4Accessor Is Nothing Then
            Dim IPv4Bytes As Integer
            IPv4Bytes = _IPv4ColumnSize * _MetaData.DBCount ' 4 bytes per column
            _IPv4Offset = _MetaData.BaseAddr - 1
            _IPv4Accessor = _MMF.CreateViewAccessor(_IPv4Offset, IPv4Bytes, MemoryMappedFileAccess.Read) ' assume MMF created
            _MapDataOffset = _IPv4Offset + IPv4Bytes
        End If

        ' NO LONGER USING ACCESSOR AS IT IS TOO SLOW, USING MULTI-DIMENSIONAL ARRAY NOW
        ' Indexes (both IPv4 & IPv6)
        'If _MetaData.Indexed Then
        '    Dim IndexBytes As Integer
        '    IndexBytes = _MetaData.BaseAddr - _MetaData.IndexBaseAddr ' from start of IPv4 index till just before start of IPv4 data
        '    _IndexOffset = _MetaData.IndexBaseAddr - 1
        '    _IndexAccessor = _MMF.CreateViewAccessor(_IndexOffset, IndexBytes, MemoryMappedFileAccess.Read) ' assume MMF created
        'End If

        If Not _MetaData.OldBIN AndAlso _IPv6Accessor Is Nothing Then
            Dim IPv6Bytes As Integer
            IPv6Bytes = _IPv6ColumnSize * _MetaData.DBCountIPv6 ' 4 bytes per column but IPFrom 16 bytes
            _IPv6Offset = _MetaData.BaseAddrIPv6 - 1
            _IPv6Accessor = _MMF.CreateViewAccessor(_IPv6Offset, IPv6Bytes, MemoryMappedFileAccess.Read) ' assume MMF created
            _MapDataOffset = _IPv6Offset + IPv6Bytes
        End If

        If _MapDataAccessor Is Nothing Then
            _MapDataAccessor = _MMF.CreateViewAccessor(_MapDataOffset, 0, MemoryMappedFileAccess.Read) ' read from offset till EOF
        End If
    End Sub

    ' Description: Destroy memory accessors
    Private Sub DestroyAccessors()
        If Not _IPv4Accessor Is Nothing Then
            _IPv4Accessor.Dispose()
            _IPv4Accessor = Nothing
        End If

        'If Not _IndexAccessor Is Nothing Then
        '    _IndexAccessor.Dispose()
        '    _IndexAccessor = Nothing
        'End If

        If Not _IPv6Accessor Is Nothing Then
            _IPv6Accessor.Dispose()
            _IPv6Accessor = Nothing
        End If

        If Not _MapDataAccessor Is Nothing Then
            _MapDataAccessor.Dispose()
            _MapDataAccessor = Nothing
        End If
    End Sub

    ' Description: Read BIN file into memory mapped file and create accessors
    Public Function LoadBIN() As Boolean
        Dim loadOK As Boolean = False
        SyncLock _LockLoadBIN
            If _DBFilePath <> "" Then
                CreateMemoryMappedFile()

                If _MMF IsNot Nothing Then ' file either couldn't be read or path incorrect
                    ' below use temp accessor as we only need once to read meta data (use this even when in filestream mode)
                    Using _MetaAccessor As MemoryMappedViewAccessor = _MMF.CreateViewAccessor(0, 64, MemoryMappedFileAccess.Read) ' 64 bytes header
                        _MetaData = New MetaData
                        With _MetaData
                            .DBType = _MetaAccessor.ReadByte(0)
                            .DBColumn = _MetaAccessor.ReadByte(1)
                            .DBYear = _MetaAccessor.ReadByte(2)
                            .DBMonth = _MetaAccessor.ReadByte(3)
                            .DBDay = _MetaAccessor.ReadByte(4)
                            .DBCount = _MetaAccessor.ReadInt32(5) '4 bytes
                            .BaseAddr = _MetaAccessor.ReadInt32(9) '4 bytes
                            .DBCountIPv6 = _MetaAccessor.ReadInt32(13) '4 bytes
                            .BaseAddrIPv6 = _MetaAccessor.ReadInt32(17) '4 bytes
                            .IndexBaseAddr = _MetaAccessor.ReadInt32(21) '4 bytes
                            .IndexBaseAddrIPv6 = _MetaAccessor.ReadInt32(25) '4 bytes

                            If .IndexBaseAddr > 0 Then
                                .Indexed = True
                            End If

                            If .DBCountIPv6 = 0 Then ' old style IPv4-only BIN file
                                .OldBIN = True
                            Else
                                If .IndexBaseAddrIPv6 > 0 Then
                                    .IndexedIPv6 = True
                                End If
                            End If

                            _IPv4ColumnSize = .DBColumn << 2 ' 4 bytes each column
                            _IPv6ColumnSize = 16 + ((.DBColumn - 1) << 2) ' 4 bytes each column, except IPFrom column which is 16 bytes

                            Dim dbt As Integer = .DBType

                            ' since both IPv4 and IPv6 use 4 bytes for the below columns, can just do it once here
                            'COUNTRY_POSITION_OFFSET = If(COUNTRY_POSITION(dbt) <> 0, (COUNTRY_POSITION(dbt) - 1) << 2, 0)
                            'REGION_POSITION_OFFSET = If(REGION_POSITION(dbt) <> 0, (REGION_POSITION(dbt) - 1) << 2, 0)
                            'CITY_POSITION_OFFSET = If(CITY_POSITION(dbt) <> 0, (CITY_POSITION(dbt) - 1) << 2, 0)
                            'ISP_POSITION_OFFSET = If(ISP_POSITION(dbt) <> 0, (ISP_POSITION(dbt) - 1) << 2, 0)
                            'DOMAIN_POSITION_OFFSET = If(DOMAIN_POSITION(dbt) <> 0, (DOMAIN_POSITION(dbt) - 1) << 2, 0)
                            'ZIPCODE_POSITION_OFFSET = If(ZIPCODE_POSITION(dbt) <> 0, (ZIPCODE_POSITION(dbt) - 1) << 2, 0)
                            'LATITUDE_POSITION_OFFSET = If(LATITUDE_POSITION(dbt) <> 0, (LATITUDE_POSITION(dbt) - 1) << 2, 0)
                            'LONGITUDE_POSITION_OFFSET = If(LONGITUDE_POSITION(dbt) <> 0, (LONGITUDE_POSITION(dbt) - 1) << 2, 0)
                            'TIMEZONE_POSITION_OFFSET = If(TIMEZONE_POSITION(dbt) <> 0, (TIMEZONE_POSITION(dbt) - 1) << 2, 0)
                            'NETSPEED_POSITION_OFFSET = If(NETSPEED_POSITION(dbt) <> 0, (NETSPEED_POSITION(dbt) - 1) << 2, 0)
                            'IDDCODE_POSITION_OFFSET = If(IDDCODE_POSITION(dbt) <> 0, (IDDCODE_POSITION(dbt) - 1) << 2, 0)
                            'AREACODE_POSITION_OFFSET = If(AREACODE_POSITION(dbt) <> 0, (AREACODE_POSITION(dbt) - 1) << 2, 0)
                            'WEATHERSTATIONCODE_POSITION_OFFSET = If(WEATHERSTATIONCODE_POSITION(dbt) <> 0, (WEATHERSTATIONCODE_POSITION(dbt) - 1) << 2, 0)
                            'WEATHERSTATIONNAME_POSITION_OFFSET = If(WEATHERSTATIONNAME_POSITION(dbt) <> 0, (WEATHERSTATIONNAME_POSITION(dbt) - 1) << 2, 0)
                            'MCC_POSITION_OFFSET = If(MCC_POSITION(dbt) <> 0, (MCC_POSITION(dbt) - 1) << 2, 0)
                            'MNC_POSITION_OFFSET = If(MNC_POSITION(dbt) <> 0, (MNC_POSITION(dbt) - 1) << 2, 0)
                            'MOBILEBRAND_POSITION_OFFSET = If(MOBILEBRAND_POSITION(dbt) <> 0, (MOBILEBRAND_POSITION(dbt) - 1) << 2, 0)
                            'ELEVATION_POSITION_OFFSET = If(ELEVATION_POSITION(dbt) <> 0, (ELEVATION_POSITION(dbt) - 1) << 2, 0)
                            'USAGETYPE_POSITION_OFFSET = If(USAGETYPE_POSITION(dbt) <> 0, (USAGETYPE_POSITION(dbt) - 1) << 2, 0)

                            ' slightly different offset for reading by row
                            COUNTRY_POSITION_OFFSET = If(COUNTRY_POSITION(dbt) <> 0, (COUNTRY_POSITION(dbt) - 2) << 2, 0)
                            REGION_POSITION_OFFSET = If(REGION_POSITION(dbt) <> 0, (REGION_POSITION(dbt) - 2) << 2, 0)
                            CITY_POSITION_OFFSET = If(CITY_POSITION(dbt) <> 0, (CITY_POSITION(dbt) - 2) << 2, 0)
                            ISP_POSITION_OFFSET = If(ISP_POSITION(dbt) <> 0, (ISP_POSITION(dbt) - 2) << 2, 0)
                            DOMAIN_POSITION_OFFSET = If(DOMAIN_POSITION(dbt) <> 0, (DOMAIN_POSITION(dbt) - 2) << 2, 0)
                            ZIPCODE_POSITION_OFFSET = If(ZIPCODE_POSITION(dbt) <> 0, (ZIPCODE_POSITION(dbt) - 2) << 2, 0)
                            LATITUDE_POSITION_OFFSET = If(LATITUDE_POSITION(dbt) <> 0, (LATITUDE_POSITION(dbt) - 2) << 2, 0)
                            LONGITUDE_POSITION_OFFSET = If(LONGITUDE_POSITION(dbt) <> 0, (LONGITUDE_POSITION(dbt) - 2) << 2, 0)
                            TIMEZONE_POSITION_OFFSET = If(TIMEZONE_POSITION(dbt) <> 0, (TIMEZONE_POSITION(dbt) - 2) << 2, 0)
                            NETSPEED_POSITION_OFFSET = If(NETSPEED_POSITION(dbt) <> 0, (NETSPEED_POSITION(dbt) - 2) << 2, 0)
                            IDDCODE_POSITION_OFFSET = If(IDDCODE_POSITION(dbt) <> 0, (IDDCODE_POSITION(dbt) - 2) << 2, 0)
                            AREACODE_POSITION_OFFSET = If(AREACODE_POSITION(dbt) <> 0, (AREACODE_POSITION(dbt) - 2) << 2, 0)
                            WEATHERSTATIONCODE_POSITION_OFFSET = If(WEATHERSTATIONCODE_POSITION(dbt) <> 0, (WEATHERSTATIONCODE_POSITION(dbt) - 2) << 2, 0)
                            WEATHERSTATIONNAME_POSITION_OFFSET = If(WEATHERSTATIONNAME_POSITION(dbt) <> 0, (WEATHERSTATIONNAME_POSITION(dbt) - 2) << 2, 0)
                            MCC_POSITION_OFFSET = If(MCC_POSITION(dbt) <> 0, (MCC_POSITION(dbt) - 2) << 2, 0)
                            MNC_POSITION_OFFSET = If(MNC_POSITION(dbt) <> 0, (MNC_POSITION(dbt) - 2) << 2, 0)
                            MOBILEBRAND_POSITION_OFFSET = If(MOBILEBRAND_POSITION(dbt) <> 0, (MOBILEBRAND_POSITION(dbt) - 2) << 2, 0)
                            ELEVATION_POSITION_OFFSET = If(ELEVATION_POSITION(dbt) <> 0, (ELEVATION_POSITION(dbt) - 2) << 2, 0)
                            USAGETYPE_POSITION_OFFSET = If(USAGETYPE_POSITION(dbt) <> 0, (USAGETYPE_POSITION(dbt) - 2) << 2, 0)

                            COUNTRY_ENABLED = If(COUNTRY_POSITION(dbt) <> 0, True, False)
                            REGION_ENABLED = If(REGION_POSITION(dbt) <> 0, True, False)
                            CITY_ENABLED = If(CITY_POSITION(dbt) <> 0, True, False)
                            ISP_ENABLED = If(ISP_POSITION(dbt) <> 0, True, False)
                            LATITUDE_ENABLED = If(LATITUDE_POSITION(dbt) <> 0, True, False)
                            LONGITUDE_ENABLED = If(LONGITUDE_POSITION(dbt) <> 0, True, False)
                            DOMAIN_ENABLED = If(DOMAIN_POSITION(dbt) <> 0, True, False)
                            ZIPCODE_ENABLED = If(ZIPCODE_POSITION(dbt) <> 0, True, False)
                            TIMEZONE_ENABLED = If(TIMEZONE_POSITION(dbt) <> 0, True, False)
                            NETSPEED_ENABLED = If(NETSPEED_POSITION(dbt) <> 0, True, False)
                            IDDCODE_ENABLED = If(IDDCODE_POSITION(dbt) <> 0, True, False)
                            AREACODE_ENABLED = If(AREACODE_POSITION(dbt) <> 0, True, False)
                            WEATHERSTATIONCODE_ENABLED = If(WEATHERSTATIONCODE_POSITION(dbt) <> 0, True, False)
                            WEATHERSTATIONNAME_ENABLED = If(WEATHERSTATIONNAME_POSITION(dbt) <> 0, True, False)
                            MCC_ENABLED = If(MCC_POSITION(dbt) <> 0, True, False)
                            MNC_ENABLED = If(MNC_POSITION(dbt) <> 0, True, False)
                            MOBILEBRAND_ENABLED = If(MOBILEBRAND_POSITION(dbt) <> 0, True, False)
                            ELEVATION_ENABLED = If(ELEVATION_POSITION(dbt) <> 0, True, False)
                            USAGETYPE_ENABLED = If(USAGETYPE_POSITION(dbt) <> 0, True, False)

                        End With
                    End Using

                    With _MetaData
                        If .Indexed Then
                            Using _IndexAccessor As MemoryMappedViewAccessor = _MMF.CreateViewAccessor(.IndexBaseAddr - 1, .BaseAddr - .IndexBaseAddr, MemoryMappedFileAccess.Read) ' reading indexes
                                Dim pointer As Integer = 0

                                ' read IPv4 index
                                For x As Integer = _IndexArrayIPv4.GetLowerBound(0) To _IndexArrayIPv4.GetUpperBound(0)
                                    _IndexArrayIPv4(x, 0) = _IndexAccessor.ReadInt32(pointer) '4 bytes for from row
                                    _IndexArrayIPv4(x, 1) = _IndexAccessor.ReadInt32(pointer + 4) '4 bytes for to row
                                    pointer += 8
                                Next

                                If .IndexedIPv6 Then
                                    ' read IPv6 index
                                    For x As Integer = _IndexArrayIPv6.GetLowerBound(0) To _IndexArrayIPv6.GetUpperBound(0)
                                        _IndexArrayIPv6(x, 0) = _IndexAccessor.ReadInt32(pointer) '4 bytes for from row
                                        _IndexArrayIPv6(x, 1) = _IndexAccessor.ReadInt32(pointer + 4) '4 bytes for to row
                                        pointer += 8
                                    Next
                                End If
                            End Using
                        End If
                    End With

                    If _UseMemoryMappedFile Then
                        CreateAccessors()
                    Else
                        DestroyMemoryMappedFile()
                    End If
                    loadOK = True
                End If
            End If
        End SyncLock
        Return loadOK
    End Function

    ' Description: Make sure the component is registered (DEPRECATED)
    Public Function IsRegistered() As Boolean
        Return True
    End Function

    ' Description: Reverse the bytes if system is little endian
    Private Sub LittleEndian(ByRef byteArr() As Byte)
        If System.BitConverter.IsLittleEndian Then
            Dim byteList As New List(Of Byte)(byteArr)
            byteList.Reverse()
            byteArr = byteList.ToArray()
        End If
    End Sub

    ' Description: Reverse the bytes
    Private Sub ReverseArray(ByRef byteArr() As Byte)
        Dim byteList As New List(Of Byte)(byteArr)
        byteList.Reverse()
        byteArr = byteList.ToArray()
    End Sub

    ' Description: Query database to get location information by IP address
    Public Function IPQuery(ByVal myIPAddress As String) As IPResult
        Dim obj As New IPResult
        Dim strIP As String
        Dim myIPType As Integer = 0
        Dim myDBType As Integer = 0
        Dim myBaseAddr As Integer = 0
        Dim myDBColumn As Integer = 0
        Dim myAccessor As MemoryMappedViewAccessor = Nothing
        Dim myFilestream As FileStream = Nothing

        Dim countrypos As Long = 0
        Dim low As Long = 0
        Dim high As Long = 0
        Dim mid As Long = 0
        Dim ipfrom As BigInteger = 0
        Dim ipto As BigInteger = 0
        Dim ipnum As BigInteger = 0
        'Dim ipnum2 As BigInteger = 0
        Dim indexaddr As Long = 0
        Dim MAX_IP_RANGE As BigInteger = 0
        Dim rowoffset As Long = 0
        Dim rowoffset2 As Long = 0
        Dim myColumnSize As Integer = 0
        Dim overCapacity As Boolean = False

        Try
            If myIPAddress = "" OrElse myIPAddress Is Nothing Then
                obj.Status = "EMPTY_IP_ADDRESS"
                Return obj
                Exit Try
            End If

            strIP = Me.VerifyIP(myIPAddress, myIPType, ipnum)
            If strIP <> "Invalid IP" Then
                myIPAddress = strIP
            Else
                obj.Status = "INVALID_IP_ADDRESS"
                Return obj
                'End Try
            End If

            ' Read BIN if haven't done so
            If _MetaData Is Nothing Then
                If Not LoadBIN() Then ' problems reading BIN
                    obj.Status = "MISSING_FILE"
                    Return obj
                    Exit Try
                End If
            End If

            If _UseMemoryMappedFile Then
                CreateMemoryMappedFile()
                CreateAccessors()
            Else
                DestroyAccessors()
                DestroyMemoryMappedFile()
                myFilestream = New FileStream(_DBFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
            End If

            Select Case myIPType
                Case 4
                    ' IPv4
                    MAX_IP_RANGE = MAX_IPV4_RANGE
                    high = _MetaData.DBCount
                    If _UseMemoryMappedFile Then
                        myAccessor = _IPv4Accessor
                    Else
                        myBaseAddr = _MetaData.BaseAddr
                    End If
                    myColumnSize = _IPv4ColumnSize

                    If _MetaData.Indexed Then
                        'ipnum2 = ipnum
                        'ipnum2 >>= 16 'get the first 2 octets
                        'indexaddr = ipnum2 << 3 '4 bytes for from row & 4 bytes for to row
                        'indexaddr = (ipnum >> 16) << 3 'old style for viewaccessor
                        indexaddr = ipnum >> 16 'new style for array

                        'If Not _UseMemoryMappedFile Then
                        '    indexaddr += _MetaData.IndexBaseAddr
                        'End If
                        'low = read32(indexaddr, _IndexAccessor, myFilestream) '4 bytes
                        'high = read32(indexaddr + 4, _IndexAccessor, myFilestream) '4 bytes

                        low = _IndexArrayIPv4(indexaddr, 0)
                        high = _IndexArrayIPv4(indexaddr, 1)
                    End If
                Case 6
                    ' IPv6
                    If _MetaData.OldBIN Then ' old IPv4-only BIN don't contain IPv6 data
                        obj.Status = "IPV6_NOT_SUPPORTED"
                        Return obj
                        Exit Try
                    End If
                    MAX_IP_RANGE = MAX_IPV6_RANGE
                    high = _MetaData.DBCountIPv6
                    If _UseMemoryMappedFile Then
                        myAccessor = _IPv6Accessor
                    Else
                        myBaseAddr = _MetaData.BaseAddrIPv6
                    End If
                    myColumnSize = _IPv6ColumnSize

                    If _MetaData.IndexedIPv6 Then
                        'ipnum2 = ipnum
                        'ipnum2 >>= 112 'get the first 2 octets
                        'indexaddr = ipnum2 << 3 '4 bytes for from row & 4 bytes for to row
                        'indexaddr = (ipnum >> 112) << 3 'old style for viewaccessor
                        indexaddr = ipnum >> 112 'new style for array

                        'If Not _UseMemoryMappedFile Then
                        '    indexaddr += _MetaData.IndexBaseAddrIPv6
                        'Else
                        '    indexaddr += (_MetaData.IndexBaseAddrIPv6 - _MetaData.IndexBaseAddr) 'IPv6 index always follows IPv4 index so offset IPv4 first
                        'End If
                        'low = read32(indexaddr, _IndexAccessor, myFilestream) '4 bytes
                        'high = read32(indexaddr + 4, _IndexAccessor, myFilestream) '4 bytes

                        low = _IndexArrayIPv6(indexaddr, 0)
                        high = _IndexArrayIPv6(indexaddr, 1)
                    End If
            End Select

            If ipnum >= MAX_IP_RANGE Then
                ipnum = MAX_IP_RANGE - 1
            End If

            While (low <= high)
                mid = CInt((low + high) / 2)

                rowoffset = myBaseAddr + (mid * myColumnSize)
                rowoffset2 = rowoffset + myColumnSize

                If _UseMemoryMappedFile Then
                    overCapacity = (rowoffset2 >= myAccessor.Capacity)
                End If

                ipfrom = read32or128(rowoffset, myIPType, myAccessor, myFilestream)
                ipto = If(overCapacity, BigInteger.Zero, read32or128(rowoffset2, myIPType, myAccessor, myFilestream))

                If ipnum >= ipfrom AndAlso ipnum < ipto Then
                    Dim country_short As String = MSG_NOT_SUPPORTED
                    Dim country_long As String = MSG_NOT_SUPPORTED
                    Dim region As String = MSG_NOT_SUPPORTED
                    Dim city As String = MSG_NOT_SUPPORTED
                    Dim isp As String = MSG_NOT_SUPPORTED
                    Dim latitude As Single = 0.0
                    Dim longitude As Single = 0.0
                    Dim domain As String = MSG_NOT_SUPPORTED
                    Dim zipcode As String = MSG_NOT_SUPPORTED
                    Dim timezone As String = MSG_NOT_SUPPORTED
                    Dim netspeed As String = MSG_NOT_SUPPORTED
                    Dim iddcode As String = MSG_NOT_SUPPORTED
                    Dim areacode As String = MSG_NOT_SUPPORTED
                    Dim weatherstationcode As String = MSG_NOT_SUPPORTED
                    Dim weatherstationname As String = MSG_NOT_SUPPORTED
                    Dim mcc As String = MSG_NOT_SUPPORTED
                    Dim mnc As String = MSG_NOT_SUPPORTED
                    Dim mobilebrand As String = MSG_NOT_SUPPORTED
                    Dim elevation As Single = 0.0
                    Dim usagetype As String = MSG_NOT_SUPPORTED

                    Dim firstCol As Integer = 4 ' for IPv4, IP From is 4 bytes
                    If myIPType = 6 Then ' IPv6
                        firstCol = 16 ' 16 bytes for IPv6
                        'rowoffset = rowoffset + 12 ' coz below is assuming all columns are 4 bytes, so got 12 left to go to make 16 bytes total
                    End If

                    ' read the row here after the IP From column (remaining columns are all 4 bytes)
                    Dim row() As Byte = readrow(rowoffset + firstCol, myColumnSize - firstCol, myAccessor, myFilestream)

                    If COUNTRY_ENABLED Then
                        'countrypos = read32(rowoffset + COUNTRY_POSITION_OFFSET, myAccessor, myFilestream)
                        'countrypos = read32_row(row, COUNTRY_POSITION_OFFSET - firstCol)
                        countrypos = read32_row(row, COUNTRY_POSITION_OFFSET)
                        country_short = readStr(countrypos, myFilestream)
                        country_long = readStr(countrypos + 3, myFilestream)
                    End If
                    If REGION_ENABLED Then
                        'region = readStr(read32(rowoffset + REGION_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'region = readStr(read32_row(row, REGION_POSITION_OFFSET - firstCol), myFilestream)
                        region = readStr(read32_row(row, REGION_POSITION_OFFSET), myFilestream)
                    End If
                    If CITY_ENABLED Then
                        'city = readStr(read32(rowoffset + CITY_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'city = readStr(read32_row(row, CITY_POSITION_OFFSET - firstCol), myFilestream)
                        city = readStr(read32_row(row, CITY_POSITION_OFFSET), myFilestream)
                    End If
                    If ISP_ENABLED Then
                        'isp = readStr(read32(rowoffset + ISP_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'isp = readStr(read32_row(row, ISP_POSITION_OFFSET - firstCol), myFilestream)
                        isp = readStr(read32_row(row, ISP_POSITION_OFFSET), myFilestream)
                    End If
                    If DOMAIN_ENABLED Then
                        'domain = readStr(read32(rowoffset + DOMAIN_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'domain = readStr(read32_row(row, DOMAIN_POSITION_OFFSET - firstCol), myFilestream)
                        domain = readStr(read32_row(row, DOMAIN_POSITION_OFFSET), myFilestream)
                    End If
                    If ZIPCODE_ENABLED Then
                        'zipcode = readStr(read32(rowoffset + ZIPCODE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'zipcode = readStr(read32_row(row, ZIPCODE_POSITION_OFFSET - firstCol), myFilestream)
                        zipcode = readStr(read32_row(row, ZIPCODE_POSITION_OFFSET), myFilestream)
                    End If
                    If LATITUDE_ENABLED Then
                        ' new requirement to "format" to 6 decimal places
                        'latitude = Math.Round(New Decimal(readFloat(rowoffset + LATITUDE_POSITION_OFFSET, myAccessor, myFilestream)), 6)
                        'latitude = Math.Round(New Decimal(readFloat_row(row, LATITUDE_POSITION_OFFSET - firstCol)), 6)
                        latitude = Math.Round(New Decimal(readFloat_row(row, LATITUDE_POSITION_OFFSET)), 6)
                    End If
                    If LONGITUDE_ENABLED Then
                        'longitude = Math.Round(New Decimal(readFloat(rowoffset + LONGITUDE_POSITION_OFFSET, myAccessor, myFilestream)), 6)
                        'longitude = Math.Round(New Decimal(readFloat_row(row, LONGITUDE_POSITION_OFFSET - firstCol)), 6)
                        longitude = Math.Round(New Decimal(readFloat_row(row, LONGITUDE_POSITION_OFFSET)), 6)
                    End If
                    If TIMEZONE_ENABLED Then
                        'timezone = readStr(read32(rowoffset + TIMEZONE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'timezone = readStr(read32_row(row, TIMEZONE_POSITION_OFFSET - firstCol), myFilestream)
                        timezone = readStr(read32_row(row, TIMEZONE_POSITION_OFFSET), myFilestream)
                    End If
                    If NETSPEED_ENABLED Then
                        'netspeed = readStr(read32(rowoffset + NETSPEED_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'netspeed = readStr(read32_row(row, NETSPEED_POSITION_OFFSET - firstCol), myFilestream)
                        netspeed = readStr(read32_row(row, NETSPEED_POSITION_OFFSET), myFilestream)
                    End If
                    If IDDCODE_ENABLED Then
                        'iddcode = readStr(read32(rowoffset + IDDCODE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'iddcode = readStr(read32_row(row, IDDCODE_POSITION_OFFSET - firstCol), myFilestream)
                        iddcode = readStr(read32_row(row, IDDCODE_POSITION_OFFSET), myFilestream)
                    End If
                    If AREACODE_ENABLED Then
                        'areacode = readStr(read32(rowoffset + AREACODE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'areacode = readStr(read32_row(row, AREACODE_POSITION_OFFSET - firstCol), myFilestream)
                        areacode = readStr(read32_row(row, AREACODE_POSITION_OFFSET), myFilestream)
                    End If
                    If WEATHERSTATIONCODE_ENABLED Then
                        'weatherstationcode = readStr(read32(rowoffset + WEATHERSTATIONCODE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'weatherstationcode = readStr(read32_row(row, WEATHERSTATIONCODE_POSITION_OFFSET - firstCol), myFilestream)
                        weatherstationcode = readStr(read32_row(row, WEATHERSTATIONCODE_POSITION_OFFSET), myFilestream)
                    End If
                    If WEATHERSTATIONNAME_ENABLED Then
                        'weatherstationname = readStr(read32(rowoffset + WEATHERSTATIONNAME_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'weatherstationname = readStr(read32_row(row, WEATHERSTATIONNAME_POSITION_OFFSET - firstCol), myFilestream)
                        weatherstationname = readStr(read32_row(row, WEATHERSTATIONNAME_POSITION_OFFSET), myFilestream)
                    End If
                    If MCC_ENABLED Then
                        'mcc = readStr(read32(rowoffset + MCC_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'mcc = readStr(read32_row(row, MCC_POSITION_OFFSET - firstCol), myFilestream)
                        mcc = readStr(read32_row(row, MCC_POSITION_OFFSET), myFilestream)
                    End If
                    If MNC_ENABLED Then
                        'mnc = readStr(read32(rowoffset + MNC_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'mnc = readStr(read32_row(row, MNC_POSITION_OFFSET - firstCol), myFilestream)
                        mnc = readStr(read32_row(row, MNC_POSITION_OFFSET), myFilestream)
                    End If
                    If MOBILEBRAND_ENABLED Then
                        'mobilebrand = readStr(read32(rowoffset + MOBILEBRAND_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'mobilebrand = readStr(read32_row(row, MOBILEBRAND_POSITION_OFFSET - firstCol), myFilestream)
                        mobilebrand = readStr(read32_row(row, MOBILEBRAND_POSITION_OFFSET), myFilestream)
                    End If
                    If ELEVATION_ENABLED Then
                        'Single.TryParse(readStr(read32(rowoffset + ELEVATION_POSITION_OFFSET, myAccessor, myFilestream), myFilestream), elevation)
                        'Single.TryParse(readStr(read32_row(row, ELEVATION_POSITION_OFFSET - firstCol), myFilestream), elevation)
                        Single.TryParse(readStr(read32_row(row, ELEVATION_POSITION_OFFSET), myFilestream), elevation)
                    End If
                    If USAGETYPE_ENABLED Then
                        'usagetype = readStr(read32(rowoffset + USAGETYPE_POSITION_OFFSET, myAccessor, myFilestream), myFilestream)
                        'usagetype = readStr(read32_row(row, USAGETYPE_POSITION_OFFSET - firstCol), myFilestream)
                        usagetype = readStr(read32_row(row, USAGETYPE_POSITION_OFFSET), myFilestream)
                    End If

                    obj.IPAddress = myIPAddress
                    obj.IPNumber = ipnum.ToString()
                    obj.CountryShort = country_short
                    obj.CountryLong = country_long
                    obj.Region = region
                    obj.City = city
                    obj.InternetServiceProvider = isp
                    obj.DomainName = domain
                    obj.ZipCode = zipcode
                    obj.NetSpeed = netspeed
                    obj.IDDCode = iddcode
                    obj.AreaCode = areacode
                    obj.WeatherStationCode = weatherstationcode
                    obj.WeatherStationName = weatherstationname
                    obj.TimeZone = timezone
                    obj.Latitude = latitude
                    obj.Longitude = longitude
                    obj.MCC = mcc
                    obj.MNC = mnc
                    obj.MobileBrand = mobilebrand
                    obj.Elevation = elevation
                    obj.UsageType = usagetype
                    obj.Status = MSG_OK

                    Return obj
                    Exit While
                Else
                    If ipnum < ipfrom Then
                        high = mid - 1
                    Else
                        low = mid + 1
                    End If
                End If
            End While

            obj.Status = "IP_ADDRESS_NOT_FOUND"
            Return obj
        Finally
            obj = Nothing
            If Not myFilestream Is Nothing Then
                myFilestream.Close()
                myFilestream.Dispose()
            End If
        End Try
    End Function

    ' Read 8 bits in the encrypted database (at present NOT being used as metadata is being read via memory accessor)
    'Private Function read8(ByVal _Pos As Long, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As Byte
    '    If _UseFileStream Then
    '        Dim _Byte(0) As Byte
    '        MyFilestream.Seek(_Pos - 1, SeekOrigin.Begin)
    '        MyFilestream.Read(_Byte, 0, 1)
    '        Return _Byte(0)
    '    Else
    '        Return MyAccessor.ReadByte(_Pos)
    '    End If
    'End Function

    ' Read whole row into array of bytes
    Private Function readrow(ByVal _Pos As Long, ByVal MyLen As UInt32, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As Byte()
        Dim row(MyLen - 1) As Byte

        If _UseMemoryMappedFile Then
            MyAccessor.ReadArray(Of Byte)(_Pos, row, 0, MyLen)
        Else
            MyFilestream.Seek(_Pos - 1, SeekOrigin.Begin)
            MyFilestream.Read(row, 0, MyLen)
        End If

        Return row
    End Function

    Private Function read32or128(ByVal _Pos As Long, ByVal _MyIPType As Integer, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As BigInteger
        If _MyIPType = 4 Then
            Return read32(_Pos, MyAccessor, MyFilestream)
        ElseIf _MyIPType = 6 Then
            Return read128(_Pos, MyAccessor, MyFilestream) ' only IPv6 will run this
        Else
            Return 0
        End If
    End Function

    ' Read 128 bits in the encrypted database
    Private Function read128(ByVal _Pos As Long, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As BigInteger
        Dim bigRetVal As BigInteger = 0

        If _UseMemoryMappedFile Then
            bigRetVal = MyAccessor.ReadUInt64(_Pos + 8)
            bigRetVal <<= 64
            bigRetVal += MyAccessor.ReadUInt64(_Pos)
        Else
            Dim _Byte(15) As Byte ' 16 bytes
            MyFilestream.Seek(_Pos - 1, SeekOrigin.Begin)
            MyFilestream.Read(_Byte, 0, 16)
            bigRetVal = System.BitConverter.ToUInt64(_Byte, 8)
            bigRetVal <<= 64
            bigRetVal += System.BitConverter.ToUInt64(_Byte, 0)
        End If

        Return bigRetVal
    End Function

    ' Read 32 bits in byte array
    Private Function read32_row(ByRef row() As Byte, ByVal byteOffset As Integer) As BigInteger
        Dim _Byte(3) As Byte ' 4 bytes
        Array.Copy(row, byteOffset, _Byte, 0, 4)
        Return System.BitConverter.ToUInt32(_Byte, 0)
    End Function

    ' Read 32 bits in the encrypted database
    Private Function read32(ByVal _Pos As Long, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As BigInteger
        If _UseMemoryMappedFile Then
            Return MyAccessor.ReadUInt32(_Pos)
        Else
            Dim _Byte(3) As Byte ' 4 bytes
            MyFilestream.Seek(_Pos - 1, SeekOrigin.Begin)
            MyFilestream.Read(_Byte, 0, 4)

            Return System.BitConverter.ToUInt32(_Byte, 0)
        End If
    End Function

    ' Read strings in the encrypted database
    Private Function readStr(ByVal _Pos As Long, ByRef Myfilestream As FileStream) As String
        If _UseMemoryMappedFile Then
            Dim _Byte1 As Byte
            Dim _Bytes() As Byte
            _Pos -= _MapDataOffset
            _Byte1 = _MapDataAccessor.ReadByte(_Pos)
            ReDim _Bytes(_Byte1 - 1)
            _MapDataAccessor.ReadArray(_Pos + 1, _Bytes, 0, _Byte1)
            Return System.Text.Encoding.Default.GetString(_Bytes)
        Else
            Dim _Bytes(0) As Byte
            Dim _Bytes2() As Byte
            Myfilestream.Seek(_Pos, SeekOrigin.Begin)
            Myfilestream.Read(_Bytes, 0, 1)
            ReDim _Bytes2(_Bytes(0) - 1)
            Myfilestream.Read(_Bytes2, 0, _Bytes(0))
            Return System.Text.Encoding.Default.GetString(_Bytes2)
        End If
    End Function

    ' Read float number in byte array
    Private Function readFloat_row(ByRef row() As Byte, ByVal byteOffset As Integer) As Single
        Dim _Byte(3) As Byte
        Array.Copy(row, byteOffset, _Byte, 0, 4)
        Return System.BitConverter.ToSingle(_Byte, 0)
    End Function

    ' Read float number in the encrypted database
    Private Function readFloat(ByVal _Pos As Long, ByRef MyAccessor As MemoryMappedViewAccessor, ByRef MyFilestream As FileStream) As Single
        If _UseMemoryMappedFile Then
            Return MyAccessor.ReadSingle(_Pos)
        Else
            Dim _Byte(3) As Byte
            MyFilestream.Seek(_Pos - 1, SeekOrigin.Begin)
            MyFilestream.Read(_Byte, 0, 4)
            Return System.BitConverter.ToSingle(_Byte, 0)
        End If
    End Function

    ' Description: Initialize
    Public Sub New()
    End Sub

    ' Description: Remove memory accessors
    Protected Overrides Sub Finalize()
        DestroyAccessors()
        MyBase.Finalize()
    End Sub

    ' Description: Destroy memory accessors & memory mapped file (only use in specific cases, otherwise don't use)
    Public Sub Close()
        Try
            _MetaData = Nothing
            DestroyAccessors()
            DestroyMemoryMappedFile()
        Catch Ex As Exception
            ' do nothing
        End Try
    End Sub

    ' Description: Validate the IP address input
    Private Function VerifyIP(ByVal strParam As String, ByRef strIPType As Integer, ByRef ipnum As BigInteger) As String
        Try
            Dim address As IPAddress = Nothing
            Dim finalIP As String = ""

            'do checks for outlier cases here
            If _OutlierCase1.IsMatch(strParam) OrElse _OutlierCase2.IsMatch(strParam) Then 'good ip list outliers
                strParam = "0000" & strParam.Substring(1)
            End If

            If Not _OutlierCase3.IsMatch(strParam) AndAlso Not _OutlierCase4.IsMatch(strParam) AndAlso Not _OutlierCase5.IsMatch(strParam) AndAlso IPAddress.TryParse(strParam, address) Then
                Select Case address.AddressFamily
                    Case System.Net.Sockets.AddressFamily.InterNetwork
                        ' we have IPv4
                        strIPType = 4
                        'Return address.ToString()
                    Case System.Net.Sockets.AddressFamily.InterNetworkV6
                        ' we have IPv6
                        strIPType = 6
                        'Return address.ToString()
                    Case Else
                        Return "Invalid IP"
                End Select

                finalIP = address.ToString().ToUpper()

                'get ip number
                ipnum = IPNo(address)

                If strIPType = 6 Then
                    If ipnum >= _fromBI AndAlso ipnum <= _toBI Then
                        'ipv4-mapped ipv6 should treat as ipv4 and read ipv4 data section
                        strIPType = 4
                        ipnum = ipnum - _fromBI

                        'expand ipv4-mapped ipv6
                        If _IPv4MappedRegex.IsMatch(finalIP) Then
                            Dim tmp As String = String.Join("", Enumerable.Repeat("0000:", 5).ToList)
                            finalIP = finalIP.Replace("::", tmp)
                        ElseIf _IPv4MappedRegex2.IsMatch(finalIP) Then
                            Dim mymatch As RegularExpressions.Match = _IPv4MappedRegex2.Match(finalIP)
                            Dim x As Integer = 0

                            Dim tmp As String = mymatch.Groups(1).ToString()
                            Dim tmparr() As String = tmp.Trim(":").Split(":")
                            Dim len As Integer = tmparr.Length - 1
                            For x = 0 To len
                                tmparr(x) = tmparr(x).PadLeft(4, "0")
                            Next
                            Dim myrear As String = String.Join("", tmparr)
                            Dim bytes As Byte()

                            bytes = BitConverter.GetBytes(Convert.ToInt32("0x" & myrear, 16))
                            finalIP = finalIP.Replace(tmp, ":" & bytes(3) & "." & bytes(2) & "." & bytes(1) & "." & bytes(0))
                            tmp = String.Join("", Enumerable.Repeat("0000:", 5).ToList)
                            finalIP = finalIP.Replace("::", tmp)
                        End If
                    ElseIf ipnum >= _FromBI2 AndAlso ipnum <= _ToBI2 Then
                        '6to4 so need to remap to ipv4
                        strIPType = 4

                        ipnum = ipnum >> 80
                        ipnum = ipnum And _DivBI ' get last 32 bits
                    ElseIf ipnum >= _FromBI3 AndAlso ipnum <= _ToBI3 Then
                        'Teredo so need to remap to ipv4
                        strIPType = 4

                        ipnum = Not ipnum
                        ipnum = ipnum And _DivBI ' get last 32 bits
                    ElseIf ipnum <= MAX_IPV4_RANGE Then
                        'ipv4-compatible ipv6 (DEPRECATED BUT STILL SUPPORTED BY .NET)
                        strIPType = 4

                        If _IPv4CompatibleRegex.IsMatch(finalIP) Then
                            Dim bytes As Byte() = BitConverter.GetBytes(Convert.ToInt32(finalIP.Replace("::", "0x"), 16))
                            finalIP = "::" & bytes(3) & "." & bytes(2) & "." & bytes(1) & "." & bytes(0)
                        ElseIf finalIP = "::" Then
                            finalIP = finalIP & "0.0.0.0"
                        End If
                        Dim tmp As String = String.Join("", Enumerable.Repeat("0000:", 5).ToList)
                        finalIP = finalIP.Replace("::", tmp & "FFFF:")
                    Else
                        'expand ipv6 normal
                        Dim myarr() As String = Regex.Split(finalIP, "::")
                        Dim x As Integer = 0
                        Dim leftside As New List(Of String)
                        leftside.AddRange(myarr(0).Split(":"))

                        If myarr.Length > 1 Then
                            Dim rightside As New List(Of String)
                            rightside.AddRange(myarr(1).Split(":"))

                            Dim midarr As List(Of String)
                            midarr = Enumerable.Repeat("0000", 8 - leftside.Count - rightside.Count).ToList

                            rightside.InsertRange(0, midarr)
                            rightside.InsertRange(0, leftside)

                            Dim rlen As Integer = rightside.Count - 1
                            For x = 0 To rlen
                                rightside.Item(x) = rightside.Item(x).PadLeft(4, "0")
                            Next

                            finalIP = String.Join(":", rightside)
                        Else
                            Dim llen As Integer = leftside.Count - 1
                            For x = 0 To llen
                                leftside.Item(x) = leftside.Item(x).PadLeft(4, "0")
                            Next

                            finalIP = String.Join(":", leftside)
                        End If
                    End If

                End If

                Return finalIP
            Else
                Return "Invalid IP"
            End If
        Catch ex As Exception
            Return "Invalid IP"
        End Try
    End Function

    ' Description: Convert either IPv4 or IPv6 into big integer
    Private Function IPNo(ByRef ipAddress As IPAddress) As BigInteger
        Try
            Dim addrBytes() As Byte = ipAddress.GetAddressBytes()
            LittleEndian(addrBytes)

            Dim final As BigInteger

            If addrBytes.Length > 8 Then
                'IPv6
                final = System.BitConverter.ToUInt64(addrBytes, 8)
                final <<= 64
                final += System.BitConverter.ToUInt64(addrBytes, 0)
            Else
                'IPv4
                final = System.BitConverter.ToUInt32(addrBytes, 0)
            End If

            Return final
        Catch ex As Exception
            Return 0
        End Try
    End Function

End Class
