# IP2Location .NET Component

This component allows user to query an IP address for its geolocation info such as country, region or state, city, latitude and longitude, ZIP/Postal code, time zone, Internet Service Provider (ISP) or company name, domain name, net speed, area code, weather station code, weather station name, mobile country code (MCC), mobile network code (MNC) and carrier brand, elevation, and usage type. It lookup the IP address from **IP2Location BIN Data** file. This data file can be downloaded at

* Free IP2Location BIN Data: https://lite.ip2location.com
* Commercial IP2Location BIN Data: https://www.ip2location.com/database/ip2location

## Requirements

Microsoft .NET 4.72 framework or later.
Compatible with .NET Core 2.x/3.x SDK.

## Parameters
Below are the parameters to set before using this class.

|Parameter Name|Description|
|---|---|
|IPDatabasePath|Sets the IP2Location database path.|
|UseMemoryMappedFile|Set to True to enable memory mapped file feature. This will increase query speed but require more memory. It is set to False by default.|

## Methods
Below are the methods supported in this class.

|Method Name|Description|
|---|---|
|IPQuery(ByVal _IPAddress As String)|Query IP address. This method returns results in IP2Location.IPResult object.|

## Result fields
Below are the result fields.

|Field Name|Description|
|---|---|
|IPAddress|IP address.|
|IPNumber|IP address in decimal format.|
|CountryShort|Two-character country code based on ISO 3166.|
|CountryLong|Country name based on ISO 3166.|
|Region|Region or state name.|
|City|City name.|
|Latitude|City level latitude.|
|Longitude|City level longitude.|
|ZIPCode|ZIP code or postal code.|
|TimeZone|Time zone in UTC (Coordinated Universal Time).|
|InternetServiceProvider|Internet Service Provider (ISP) name.|
|DomainName|Domain name associated to IP address range.|
|NetSpeed|Internet connection speed <ul><li>(DIAL) Dial-up</li><li>(DSL) DSL/Cable</li><li>(COMP) Company/T1</li></ul>|
|IDDCode|The IDD prefix to call the city from another country.|
|AreaCode|A varying length number assigned to geographic areas for call between cities.|
|WeatherStationCode|Special code to identify the nearest weather observation station.|
|WeatherStationName|Name of the nearest weather observation station.|
|MCC|Mobile country code.|
|MNC|Mobile network code.|
|MobileBrand|Mobile carrier brand.|
|Status|Status code of query.|
|Elevation|Average height of city above sea level in meters (m).|
|UsageType|Usage type classification of ISP or company:<ul><li>(COM) Commercial</li><li>(ORG) Organization</li><li>(GOV) Government</li><li>(MIL) Military</li><li>(EDU) University/College/School</li><li>(LIB) Library</li><li>(CDN) Content Delivery Network</li><li>(ISP) Fixed Line ISP</li><li>(MOB) Mobile ISP</li><li>(DCH) Data Center/Web Hosting/Transit</li><li>(SES) Search Engine Spider</li><li>(RSV) Reserved</li></ul>|

## Status codes
Below are the status codes.
|Code|Description|
|---|---|
|OK|The query has been successfully performed.|
|EMPTY_IP_ADDRESS|The IP address is empty.|
|INVALID_IP_ADDRESS|The format of the IP address is wrong.|
|MISSING_FILE|The BIN file path is wrong or the BIN file is unreadable.|
|IP_ADDRESS_NOT_FOUND|The IP address does not exists in the BIN file.|
|IPV6_NOT_SUPPORTED|The BIN file does not contain IPv6 data.|

## Usage

```vb.net
Dim oIPResult As New IP2Location.IPResult
Dim oIP2Location As New IP2Location.Component
Try
	Dim strIPAddress = "8.8.8.8"
	If strIPAddress.Trim <> "" Then
		oIP2Location.IPDatabasePath = "C:\myfolder\IP-COUNTRY-REGION-CITY-LATITUDE-LONGITUDE-ZIPCODE-TIMEZONE-ISP-DOMAIN-NETSPEED-AREACODE-WEATHER-MOBILE-ELEVATION-USAGETYPE.BIN"
		' oIP2Location.UseMemoryMappedFile = True ' uncomment this line if you want to use MemoryMappedFile.
		oIPResult = oIP2Location.IPQuery(strIPAddress)
		Select Case oIPResult.Status
			Case "OK"
				Console.WriteLine("IP Address: " & oIPResult.IPAddress)
				Console.WriteLine("City: " & oIPResult.City)
				Console.WriteLine("Country Code: " & oIPResult.CountryShort)
				Console.WriteLine("Country Name: " & oIPResult.CountryLong)
				Console.WriteLine("Postal Code: " & oIPResult.ZipCode)
				Console.WriteLine("Domain Name: " & oIPResult.DomainName)
				Console.WriteLine("ISP Name: " & oIPResult.InternetServiceProvider)
				Console.WriteLine("Latitude: " & oIPResult.Latitude)
				Console.WriteLine("Longitude: " & oIPResult.Longitude)
				Console.WriteLine("Region: " & oIPResult.Region)
				Console.WriteLine("TimeZone: " & oIPResult.TimeZone)
				Console.WriteLine("NetSpeed: " & oIPResult.NetSpeed)
				Console.WriteLine("IDD Code: " & oIPResult.IDDCode)
				Console.WriteLine("Area Code: " & oIPResult.AreaCode)
				Console.WriteLine("Weather Station Code: " & oIPResult.WeatherStationCode)
				Console.WriteLine("Weather Station Name: " & oIPResult.WeatherStationName)
				Console.WriteLine("MCC: " & oIPResult.MCC)
				Console.WriteLine("MNC: " & oIPResult.MNC)
				Console.WriteLine("Mobile Brand: " & oIPResult.MobileBrand)
				Console.WriteLine("Elevation: " & oIPResult.Elevation)
				Console.WriteLine("Usage Type: " & oIPResult.UsageType)
			Case "EMPTY_IP_ADDRESS"
				Console.WriteLine("IP Address cannot be blank.")
			Case "INVALID_IP_ADDRESS"
				Console.WriteLine("Invalid IP Address.")
			Case "MISSING_FILE"
				Console.WriteLine("Invalid Database Path.")
		End Select
	Else
		Console.WriteLine("IP Address cannot be blank.")
	End If
Catch ex As Exception
	Console.WriteLine(ex.Message)
Finally
	oIPResult = Nothing
	oIP2Location = Nothing
End Try

```
