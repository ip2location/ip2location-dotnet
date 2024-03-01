# Quickstart

## Requirements

Microsoft .NET 4.72 framework or later.
Compatible with .NET Core 2.x/3.x SDK.
Compatible with .NET 5/6/7.

## Dependencies

This library requires IP2Location BIN database to function. You may
download the BIN database at

-   IP2Location LITE BIN Data (Free): <https://lite.ip2location.com>
-   IP2Location Commercial BIN Data (Comprehensive):
    <https://www.ip2location.com>

## Sample Codes

### Query geolocation information from BIN database

You can query the geolocation information from the IP2Location BIN database as below:

```vb.net
Dim oIPResult As New IP2Location.IPResult
Dim oIP2Location As New IP2Location.Component
Try
	Dim strIPAddress = "8.8.8.8"
	If strIPAddress.Trim <> "" Then
		oIP2Location.Open("C:\myfolder\IP-COUNTRY-REGION-CITY-LATITUDE-LONGITUDE-ZIPCODE-TIMEZONE-ISP-DOMAIN-NETSPEED-AREACODE-WEATHER-MOBILE-ELEVATION-USAGETYPE-ADDRESSTYPE-CATEGORY-DISTRICT-ASN.BIN", True)
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
				Console.WriteLine("Address Type: " & oIPResult.AddressType)
				Console.WriteLine("Category: " & oIPResult.Category)
				Console.WriteLine("District: " & oIPResult.District)
				Console.WriteLine("ASN: " & oIPResult.ASN)
				Console.WriteLine("AS: " & oIPResult.AS)
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
	oIP2Location.Close()
	oIPResult = Nothing
	oIP2Location = Nothing
End Try

```

### Query geolocation information using a stream and async IP query

You can query the geolocation information using a stream and async IP query as below:

```vb.net
Dim oIPResult As New IP2Location.IPResult
Dim oIP2Location As New IP2Location.Component
Try
	Dim strIPAddress = "8.8.8.8"
	If strIPAddress.Trim <> "" Then
		Using myStream As New FileStream("C:\myfolder\IP-COUNTRY-REGION-CITY-LATITUDE-LONGITUDE-ZIPCODE-TIMEZONE-ISP-DOMAIN-NETSPEED-AREACODE-WEATHER-MOBILE-ELEVATION-USAGETYPE-ADDRESSTYPE-CATEGORY-DISTRICT-ASN.BIN", FileMode.Open, FileAccess.Read, FileShare.Read)
			oIP2Location.Open(myStream)
			Dim myTask = oIP2Location.IPQueryAsync(strIPAddress)
			oIPResult = myTask.Result
			Select Case oIPResult.Status
				Case "OK"
					Console.WriteLine("IP Address: " & oIPResult.IPAddress)
					Console.WriteLine("Country Code: " & oIPResult.CountryShort)
					Console.WriteLine("Country Name: " & oIPResult.CountryLong)
					Console.WriteLine("Region: " & oIPResult.Region)
					Console.WriteLine("City: " & oIPResult.City)
					Console.WriteLine("Latitude: " & oIPResult.Latitude)
					Console.WriteLine("Longitude: " & oIPResult.Longitude)
					Console.WriteLine("Postal Code: " & oIPResult.ZipCode)
					Console.WriteLine("TimeZone: " & oIPResult.TimeZone)
					Console.WriteLine("ISP Name: " & oIPResult.InternetServiceProvider)
					Console.WriteLine("Domain Name: " & oIPResult.DomainName)
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
					Console.WriteLine("Address Type: " & oIPResult.AddressType)
					Console.WriteLine("Category: " & oIPResult.Category)
					Console.WriteLine("District: " & oIPResult.District)
					Console.WriteLine("ASN: " & oIPResult.ASN)
					Console.WriteLine("AS: " & oIPResult.AS)
				Case "EMPTY_IP_ADDRESS"
					Console.WriteLine("IP Address cannot be blank.")
				Case "INVALID_IP_ADDRESS"
					Console.WriteLine("Invalid IP Address.")
				Case "MISSING_FILE"
					Console.WriteLine("Invalid Database Path.")
				Case Else
					Console.WriteLine(oIPResult.Status)
			End Select
		End Using
	Else
		Console.WriteLine("IP Address cannot be blank.")
	End If
Catch ex As Exception
	Console.WriteLine(ex.Message)
Finally
	oIP2Location.Close()
	oIPResult = Nothing
	oIP2Location = Nothing
End Try

```


### Processing IP address using IP Tools class

You can manupulate IP address, IP number and CIDR as below:

```vb.net
Dim tools = New IP2Location.IPTools()

Console.WriteLine(tools.IsIPv4("60.54.166.38"))
Console.WriteLine(tools.IsIPv6("2600:1f18:45b0:5b00:f5d8:4183:7710:ceec"))
Console.WriteLine(tools.IPv4ToDecimal("60.54.166.38"))
Console.WriteLine(tools.IPv6ToDecimal("::313F:11:FC:9834"))
Console.WriteLine(tools.DecimalToIPv4(BigInteger.Parse("770")))
Console.WriteLine(tools.DecimalToIPv6(BigInteger.Parse("3548555104422238260")))
Console.WriteLine(tools.CompressIPv6("0000:0000:0000:35:0000:FFFF:0000:0000"))
Console.WriteLine(tools.ExpandIPv6("::35:00:FFFF:000:0"))
Console.WriteLine(String.Join(vbNewLine, tools.IPv4ToCIDR("10.0.0.0", "10.0.0.255")))
Console.WriteLine(String.Join(vbNewLine, tools.IPv6ToCIDR("2001:0DB8:1234:0000:0000:0000:0000:0000", "2001:0DB8:1234:FFFF:FFFF:FFFF:FFFF:FFFF")))
Dim stuff = tools.CIDRToIPv4("2002::1234:abcd:ffff:c0a8:101/64")
Console.WriteLine(stuff.IPStart)
Console.WriteLine(stuff.IPEnd)
stuff = tools.CIDRToIPv6("2002::1234:abcd:ffff:c0a8:101/64")
Console.WriteLine(stuff.IPStart)
Console.WriteLine(stuff.IPEnd)

```

### List down country information

You can query country information for a country from IP2Location Country Information CSV file as below:

```vb.net
Dim cc = New IP2Location.Country("C:\myfolder\IP2LOCATION-COUNTRY-INFORMATION.CSV")
Dim records = cc.GetCountryInfo()
For Each x In records
	Console.WriteLine("country_code: " & x.country_code)
	Console.WriteLine("country_name: " & x.country_name)
	Console.WriteLine("country_alpha3_code: " & x.country_alpha3_code)
	Console.WriteLine("country_numeric_code: " & x.country_numeric_code)
	Console.WriteLine("capital: " & x.capital)
	Console.WriteLine("country_demonym: " & x.country_demonym)
	Console.WriteLine("total_area: " & x.total_area)
	Console.WriteLine("population: " & x.population)
	Console.WriteLine("idd_code: " & x.idd_code)
	Console.WriteLine("currency_code: " & x.currency_code)
	Console.WriteLine("currency_name: " & x.currency_name)
	Console.WriteLine("currency_symbol: " & x.currency_symbol)
	Console.WriteLine("lang_code: " & x.lang_code)
	Console.WriteLine("lang_name: " & x.lang_name)
	Console.WriteLine("cctld: " & x.cctld)
	Console.WriteLine("=======================================================")
Next
Dim record = cc.GetCountryInfo("US")
If record IsNot Nothing Then
	Console.WriteLine("country_code: " & record.country_code)
	Console.WriteLine("country_name: " & record.country_name)
	Console.WriteLine("country_alpha3_code: " & record.country_alpha3_code)
	Console.WriteLine("country_numeric_code: " & record.country_numeric_code)
	Console.WriteLine("capital: " & record.capital)
	Console.WriteLine("country_demonym: " & record.country_demonym)
	Console.WriteLine("total_area: " & record.total_area)
	Console.WriteLine("population: " & record.population)
	Console.WriteLine("idd_code: " & record.idd_code)
	Console.WriteLine("currency_code: " & record.currency_code)
	Console.WriteLine("currency_name: " & record.currency_name)
	Console.WriteLine("currency_symbol: " & record.currency_symbol)
	Console.WriteLine("lang_code: " & record.lang_code)
	Console.WriteLine("lang_name: " & record.lang_name)
	Console.WriteLine("cctld: " & record.cctld)
End If
```

### List down region information

You can get the region code by country code and region name from IP2Location ISO 3166-2 Subdivision Code CSV file as below:

```vb.net
Dim reg = New IP2Location.Region("C:\myfolder\IP2LOCATION-ISO3166-2.CSV")
Dim regioncode = reg.GetRegionCode("US", "California")
Console.WriteLine(regioncode)
```