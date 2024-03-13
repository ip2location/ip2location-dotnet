'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2024 IP2Location.com
'---------------------------------------------------------------------------
Public Class Ref(Of T)
    Private _t As T
    Public Sub New()
    End Sub

    Public Sub New(value As T)
        _t = value
    End Sub

    Public Property Value As T
        Get
            Return _t
        End Get
        Set(value As T)
            _t = value
        End Set
    End Property

End Class