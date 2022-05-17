'---------------------------------------------------------------------------
' Author       : IP2Location.com
' URL          : http://www.ip2location.com
' Email        : sales@ip2location.com
'
' Copyright (c) 2002-2022 IP2Location.com
'---------------------------------------------------------------------------
Public Class IPResult
    Dim m_ip As String = "?"
    Dim m_ipno As String = "?"
    Dim m_countrySHORT As String = "?"
    Dim m_countryLONG As String = "?"
    Dim m_region As String = "?"
    Dim m_city As String = "?"
    Dim m_latitude As Single
    Dim m_longitude As Single
    Dim m_zipcode As String = "?"
    Dim m_isp As String = "?"
    Dim m_domain As String = "?"
    Dim m_timezone As String = "?"
    Dim m_netspeed As String = "?"
    Dim m_iddcode As String = "?"
    Dim m_areacode As String = "?"
    Dim m_weatherstationcode As String = "?"
    Dim m_weatherstationname As String = "?"
    Dim m_mcc As String = "?"
    Dim m_mnc As String = "?"
    Dim m_mobilebrand As String = "?"
    Dim m_elevation As Single
    Dim m_usagetype As String = "?"
    Dim m_addresstype As String = "?"
    Dim m_category As String = "?"
    Dim m_status As String = "?"

    ' Description: Get/Set the value of IPAddress
    Public Property IPAddress() As String
        Get
            Return m_ip
        End Get
        Set(ByVal Value As String)
            m_ip = Value
        End Set
    End Property

    ' Description: Get/Set the value of IPNumber
    Public Property IPNumber() As String
        Get
            Return m_ipno
        End Get
        Set(ByVal Value As String)
            m_ipno = Value
        End Set
    End Property

    ' Description: Get/Set the value of CountryShort
    Public Property CountryShort() As String
        Get
            Return m_countrySHORT
        End Get
        Set(ByVal Value As String)
            m_countrySHORT = Value
        End Set
    End Property

    ' Description: Get/Set the value of CountryLong
    Public Property CountryLong() As String
        Get
            Return m_countryLONG
        End Get
        Set(ByVal Value As String)
            m_countryLONG = Value
        End Set
    End Property

    ' Description: Get/Set the value of Region
    Public Property Region() As String
        Get
            Return m_region
        End Get
        Set(ByVal Value As String)
            m_region = Value
        End Set
    End Property

    ' Description: Get/Set the value of City
    Public Property City() As String
        Get
            Return m_city
        End Get
        Set(ByVal Value As String)
            m_city = Value
        End Set
    End Property

    ' Description: Get/Set the value of Latitude
    Public Property Latitude() As Single
        Get
            Return m_latitude
        End Get
        Set(ByVal Value As Single)
            m_latitude = Value
        End Set
    End Property

    ' Description: Get/Set the value of Longitude
    Public Property Longitude() As Single
        Get
            Return m_longitude
        End Get
        Set(ByVal Value As Single)
            m_longitude = Value
        End Set
    End Property

    ' Description: Get/Set the value of ZIPCode
    Public Property ZipCode() As String
        Get
            Return m_zipcode
        End Get
        Set(ByVal Value As String)
            m_zipcode = Value
        End Set
    End Property

    ' Description: Get/Set the value of TimeZone
    Public Property TimeZone() As String
        Get
            Return m_timezone
        End Get
        Set(ByVal Value As String)
            m_timezone = Value
        End Set
    End Property

    ' Description: Get/Set the value of NetSpeed
    Public Property NetSpeed() As String
        Get
            Return m_netspeed
        End Get
        Set(ByVal Value As String)
            m_netspeed = Value
        End Set
    End Property

    ' Description: Get/Set the value of IDDCode
    Public Property IDDCode() As String
        Get
            Return m_iddcode
        End Get
        Set(ByVal Value As String)
            m_iddcode = Value
        End Set
    End Property

    ' Description: Get/Set the value of AreaCode
    Public Property AreaCode() As String
        Get
            Return m_areacode
        End Get
        Set(ByVal Value As String)
            m_areacode = Value
        End Set
    End Property

    ' Description: Get/Set the value of WeatherStationCode
    Public Property WeatherStationCode() As String
        Get
            Return m_weatherstationcode
        End Get
        Set(ByVal Value As String)
            m_weatherstationcode = Value
        End Set
    End Property

    ' Description: Get/Set the value of WeatherStationName
    Public Property WeatherStationName() As String
        Get
            Return m_weatherstationname
        End Get
        Set(ByVal Value As String)
            m_weatherstationname = Value
        End Set
    End Property

    ' Description: Get/Set the value of InternetServiceProvider
    Public Property InternetServiceProvider() As String
        Get
            Return m_isp
        End Get
        Set(ByVal Value As String)
            m_isp = Value
        End Set
    End Property

    ' Description: Get/Set the value of DomainName
    Public Property DomainName() As String
        Get
            Return m_domain
        End Get
        Set(ByVal Value As String)
            m_domain = Value
        End Set
    End Property

    ' Description: Get/Set the value of MCC
    Public Property MCC() As String
        Get
            Return m_mcc
        End Get
        Set(ByVal Value As String)
            m_mcc = Value
        End Set
    End Property

    ' Description: Get/Set the value of MNC
    Public Property MNC() As String
        Get
            Return m_mnc
        End Get
        Set(ByVal Value As String)
            m_mnc = Value
        End Set
    End Property

    ' Description: Get/Set the value of MobileBrand
    Public Property MobileBrand() As String
        Get
            Return m_mobilebrand
        End Get
        Set(ByVal Value As String)
            m_mobilebrand = Value
        End Set
    End Property

    ' Description: Get/Set the value of Elevation
    Public Property Elevation() As Single
        Get
            Return m_elevation
        End Get
        Set(ByVal Value As Single)
            m_elevation = Value
        End Set
    End Property

    ' Description: Get/Set the value of UsageType
    Public Property UsageType() As String
        Get
            Return m_usagetype
        End Get
        Set(ByVal Value As String)
            m_usagetype = Value
        End Set
    End Property

    ' Description: Get/Set the value of AddressType
    Public Property AddressType() As String
        Get
            Return m_addresstype
        End Get
        Set(ByVal Value As String)
            m_addresstype = Value
        End Set
    End Property

    ' Description: Get/Set the value of Category
    Public Property Category() As String
        Get
            Return m_category
        End Get
        Set(ByVal Value As String)
            m_category = Value
        End Set
    End Property

    ' Description: Get/Set the value of Status
    Public Property Status() As String
        Get
            Return m_status
        End Get
        Set(ByVal Value As String)
            m_status = Value
        End Set
    End Property
End Class
