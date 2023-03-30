Imports System.Globalization

'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2023 IP2Location.com
'---------------------------------------------------------------------------
Friend Class MetaData
    Dim _BaseAddr As Integer = 0
    Dim _DBCount As Integer = 0
    Dim _DBColumn As Integer = 0
    Dim _DBType As Integer = 0
    Dim _DBDay As Integer = 1
    Dim _DBMonth As Integer = 1
    Dim _DBYear As Integer = 1
    Dim _BaseAddrIPv6 As Integer = 0
    Dim _DBCountIPv6 As Integer = 0
    Dim _OldBIN As Boolean = False
    Dim _Indexed As Boolean = False
    Dim _IndexedIPv6 As Boolean = False
    Dim _IndexBaseAddr As Integer = 0
    Dim _IndexBaseAddrIPv6 As Integer = 0
    Dim _ProductCode As Integer = 0
    Dim _ProductType As Integer = 0
    Dim _FileSize As Integer = 0


    Public Property BaseAddr() As Integer
        Get
            Return _BaseAddr
        End Get
        Set(ByVal Value As Integer)
            _BaseAddr = Value
        End Set
    End Property

    Public Property DBCount() As Integer
        Get
            Return _DBCount
        End Get
        Set(ByVal Value As Integer)
            _DBCount = Value
        End Set
    End Property

    Public Property DBColumn() As Integer
        Get
            Return _DBColumn
        End Get
        Set(ByVal Value As Integer)
            _DBColumn = Value
        End Set
    End Property

    Public Property DBType() As Integer
        Get
            Return _DBType
        End Get
        Set(ByVal Value As Integer)
            _DBType = Value
        End Set
    End Property

    Public Property DBDay() As Integer
        Get
            Return _DBDay
        End Get
        Set(ByVal Value As Integer)
            _DBDay = Value
        End Set
    End Property

    Public Property DBMonth() As Integer
        Get
            Return _DBMonth
        End Get
        Set(ByVal Value As Integer)
            _DBMonth = Value
        End Set
    End Property

    Public Property DBYear() As Integer
        Get
            Return _DBYear
        End Get
        Set(ByVal Value As Integer)
            _DBYear = Value
        End Set
    End Property

    Public Property BaseAddrIPv6() As Integer
        Get
            Return _BaseAddrIPv6
        End Get
        Set(ByVal Value As Integer)
            _BaseAddrIPv6 = Value
        End Set
    End Property

    Public Property DBCountIPv6() As Integer
        Get
            Return _DBCountIPv6
        End Get
        Set(ByVal Value As Integer)
            _DBCountIPv6 = Value
        End Set
    End Property

    Public ReadOnly Property IPVersion() As String
        Get
            Return _DBYear.ToString(CultureInfo.CurrentCulture()) & "." & _DBMonth.ToString(CultureInfo.CurrentCulture()) & "." & _DBDay.ToString(CultureInfo.CurrentCulture())
        End Get
    End Property

    Public Property OldBIN() As Boolean
        Get
            Return _OldBIN
        End Get
        Set(ByVal Value As Boolean)
            _OldBIN = Value
        End Set
    End Property

    Public Property Indexed() As Boolean
        Get
            Return _Indexed
        End Get
        Set(ByVal Value As Boolean)
            _Indexed = Value
        End Set
    End Property

    Public Property IndexedIPv6() As Boolean
        Get
            Return _IndexedIPv6
        End Get
        Set(ByVal Value As Boolean)
            _IndexedIPv6 = Value
        End Set
    End Property

    Public Property IndexBaseAddr() As Integer
        Get
            Return _IndexBaseAddr
        End Get
        Set(ByVal Value As Integer)
            _IndexBaseAddr = Value
        End Set
    End Property

    Public Property IndexBaseAddrIPv6() As Integer
        Get
            Return _IndexBaseAddrIPv6
        End Get
        Set(ByVal Value As Integer)
            _IndexBaseAddrIPv6 = Value
        End Set
    End Property

    Public Property ProductCode() As Integer
        Get
            Return _ProductCode
        End Get
        Set(ByVal Value As Integer)
            _ProductCode = Value
        End Set
    End Property

    Public Property ProductType() As Integer
        Get
            Return _ProductType
        End Get
        Set(ByVal Value As Integer)
            _ProductType = Value
        End Set
    End Property

    Public Property FileSize() As Integer
        Get
            Return _FileSize
        End Get
        Set(ByVal Value As Integer)
            _FileSize = Value
        End Set
    End Property
End Class
