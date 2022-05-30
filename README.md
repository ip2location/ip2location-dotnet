# IP2Location IP Geolocation .NET Component

This IP Geolocation .NET component allows user to query an IP address for useful IP geolocation information such as the ISO3166 country code, country name, region or state, city, latitude and longitude, ZIP/Postal code, time zone, Internet Service Provider (ISP) or company name, domain name, net speed, area code, weather station code, weather station name, mobile country code (MCC), mobile network code (MNC) and carrier brand, elevation, usage type, address type and IAB category. It lookup the IP address from **IP2Location BIN Data** file. This data file can be downloaded at

* Free IP2Location IP Geolocation BIN Data: https://lite.ip2location.com
* Commercial IP2Location IP Geolocation BIN Data: https://www.ip2location.com/database/ip2location

As an alternative, this geolocation component can also call the IP2Location Web Service. This requires an API key. If you don't have an existing API key, you can subscribe for one at the below:

https://www.ip2location.com/web-service/ip2location

## Requirements

Microsoft .NET 4.72 framework or later.
Compatible with .NET Core 2.x/3.x SDK.

## QUERY USING THE BIN FILE

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
|Open(ByVal DBPath As String, Optional ByVal UseMMF As Boolean = False)|Initialize component and preload BIN file.|
|IPQuery(ByVal _IPAddress As String)|Query IP address. This method returns results in IP2Location.IPResult object.|
|Close()|Destroy memory accessors & memory mapped file (only use in specific cases, otherwise don't use).|

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
|AddressType|IP address types as defined in Internet Protocol version 4 (IPv4) and Internet Protocol version 6 (IPv6).<ul><li>(A) Anycast - One to the closest</li><li>(U) Unicast - One to one</li><li>(M) Multicast - One to multiple</li><li>(B) Broadcast - One to all</li></ul>|
|Category|The domain category is based on [IAB Tech Lab Content Taxonomy](https://www.ip2location.com/free/iab-categories). These categories are comprised of Tier-1 and Tier-2 (if available) level categories widely used in services like advertising, Internet security and filtering appliances.|

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
		oIP2Location.Open("C:\myfolder\IP-COUNTRY-REGION-CITY-LATITUDE-LONGITUDE-ZIPCODE-TIMEZONE-ISP-DOMAIN-NETSPEED-AREACODE-WEATHER-MOBILE-ELEVATION-USAGETYPE-ADDRESSTYPE-CATEGORY.BIN", True)
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

## QUERY USING THE IP2LOCATION IP GEOLOCATION WEB SERVICE

## Methods
Below are the methods supported in this class.

|Method Name|Description|
|---|---|
|Open(ByVal APIKey As String, ByVal Package As String, ByVal Optional UseSSL As Boolean = True)|Initialize component.|
|IPQuery(ByVal IP As String, ByVal Optional Language As String = "en")|Query IP address. This method returns a JObject.|
|IPQuery(ByVal IP As String, ByVal AddOns() As String, ByVal Optional Language As String = "en")|Query IP address and Addons. This method returns a JObject.|
|GetCredit()|This method returns the web service credit balance in a JObject.|

Below are the Addons supported in this class.

|Addon Name|Description|
|---|---|
|continent|Returns continent code, name, hemispheres and translations.|
|country|Returns country codes, country name, flag, capital, total area, population, currency info, language info, IDD, TLD and translations.|
|region|Returns region code, name and translations.|
|city|Returns city name and translations.|
|geotargeting|Returns metro code based on the ZIP/postal code.|
|country_groupings|Returns group acronyms and names.|
|time_zone_info|Returns time zones, DST, GMT offset, sunrise and sunset.|

## Result fields
Below are the result fields.

|Name|
|---|
|<ul><li>country_code</li><li>country_name</li><li>region_name</li><li>city_name</li><li>latitude</li><li>longitude</li><li>zip_code</li><li>time_zone</li><li>isp</li><li>domain</li><li>net_speed</li><li>idd_code</li><li>area_code</li><li>weather_station_code</li><li>weather_station_name</li><li>mcc</li><li>mnc</li><li>mobile_brand</li><li>elevation</li><li>usage_type</li><li>address_type</li><li>category</li><li>category_name</li><li>continent<ul><li>name</li><li>code</li><li>hemisphere</li><li>translations</li></ul></li><li>country<ul><li>name</li><li>alpha3_code</li><li>numeric_code</li><li>demonym</li><li>flag</li><li>capital</li><li>total_area</li><li>population</li><li>currency<ul><li>code</li><li>name</li><li>symbol</li></ul></li><li>language<ul><li>code</li><li>name</li></ul></li><li>idd_code</li><li>tld</li><li>is_eu</li><li>translations</li></ul></li><li>region<ul><li>name</li><li>code</li><li>translations</li></ul></li><li>city<ul><li>name</li><li>translations</li></ul></li><li>geotargeting<ul><li>metro</li></ul></li><li>country_groupings</li><li>time_zone_info<ul><li>olson</li><li>current_time</li><li>gmt_offset</li><li>is_dst</li><li>sunrise</li><li>sunset</li></ul></li><ul>|

## Usage

```vb.net
Dim oIP2LocationWS As New IP2Location.ComponentWebService
Try
	Dim strIPAddress = "8.8.8.8"
	Dim strAPIKey = "YOUR_API_KEY_HERE"
	Dim strPackage = "WS25"
	Dim addOn() As String = {"continent", "country", "region", "city", "geotargeting", "country_groupings", "time_zone_info"}
	Dim strLang = "en"
	Dim boolSSL = True
	Dim myarr() As String

	oIP2LocationWS.Open(strAPIKey, strPackage, boolSSL)
	Dim myresult = oIP2LocationWS.IPQuery(strIPAddress, addOn, strLang)

	If myresult("response") Is Nothing Then
		' standard results
		Console.WriteLine("country_code: " & If(myresult("country_code") IsNot Nothing, myresult("country_code").ToString, ""))
		Console.WriteLine("country_name: " & If(myresult("country_name") IsNot Nothing, myresult("country_name").ToString, ""))
		Console.WriteLine("region_name: " & If(myresult("region_name") IsNot Nothing, myresult("region_name").ToString, ""))
		Console.WriteLine("city_name: " & If(myresult("city_name") IsNot Nothing, myresult("city_name").ToString, ""))
		Console.WriteLine("latitude: " & If(myresult("latitude") IsNot Nothing, myresult("latitude").ToString, ""))
		Console.WriteLine("longitude: " & If(myresult("longitude") IsNot Nothing, myresult("longitude").ToString, ""))
		Console.WriteLine("zip_code: " & If(myresult("zip_code") IsNot Nothing, myresult("zip_code").ToString, ""))
		Console.WriteLine("time_zone: " & If(myresult("time_zone") IsNot Nothing, myresult("time_zone").ToString, ""))
		Console.WriteLine("isp: " & If(myresult("isp") IsNot Nothing, myresult("isp").ToString, ""))
		Console.WriteLine("domain: " & If(myresult("domain") IsNot Nothing, myresult("domain").ToString, ""))
		Console.WriteLine("net_speed: " & If(myresult("net_speed") IsNot Nothing, myresult("net_speed").ToString, ""))
		Console.WriteLine("idd_code: " & If(myresult("idd_code") IsNot Nothing, myresult("idd_code").ToString, ""))
		Console.WriteLine("area_code: " & If(myresult("area_code") IsNot Nothing, myresult("area_code").ToString, ""))
		Console.WriteLine("weather_station_code: " & If(myresult("weather_station_code") IsNot Nothing, myresult("weather_station_code").ToString, ""))
		Console.WriteLine("weather_station_name: " & If(myresult("weather_station_name") IsNot Nothing, myresult("weather_station_name").ToString, ""))
		Console.WriteLine("mcc: " & If(myresult("mcc") IsNot Nothing, myresult("mcc").ToString, ""))
		Console.WriteLine("mnc: " & If(myresult("mnc") IsNot Nothing, myresult("mnc").ToString, ""))
		Console.WriteLine("mobile_brand: " & If(myresult("mobile_brand") IsNot Nothing, myresult("mobile_brand").ToString, ""))
		Console.WriteLine("elevation: " & If(myresult("elevation") IsNot Nothing, myresult("elevation").ToString, ""))
		Console.WriteLine("usage_type: " & If(myresult("usage_type") IsNot Nothing, myresult("usage_type").ToString, ""))
		Console.WriteLine("address_type: " & If(myresult("address_type") IsNot Nothing, myresult("address_type").ToString, ""))
		Console.WriteLine("category: " & If(myresult("category") IsNot Nothing, myresult("category").ToString, ""))
		Console.WriteLine("category_name: " & If(myresult("category_name") IsNot Nothing, myresult("category_name").ToString, ""))
		Console.WriteLine("credits_consumed: " & If(myresult("credits_consumed") IsNot Nothing, myresult("credits_consumed").ToString, ""))

		' continent addon
		If myresult("continent") IsNot Nothing Then
			Console.WriteLine("continent => name: " & myresult("continent")("name").ToString)
			Console.WriteLine("continent => code: " & myresult("continent")("code").ToString)
			myarr = myresult("continent")("hemisphere").ToObject(Of String())()
			Console.WriteLine("continent => hemisphere: " & String.Join(",", myarr))
			Console.WriteLine("continent => translations: " & myresult("continent")("translations")(strLang).ToString)
		End If

		' country addon
		If myresult("country") IsNot Nothing Then
			Console.WriteLine("country => name: " & myresult("country")("name").ToString)
			Console.WriteLine("country => alpha3_code: " & myresult("country")("alpha3_code").ToString)
			Console.WriteLine("country => numeric_code: " & myresult("country")("numeric_code").ToString)
			Console.WriteLine("country => demonym: " & myresult("country")("demonym").ToString)
			Console.WriteLine("country => flag: " & myresult("country")("flag").ToString)
			Console.WriteLine("country => capital: " & myresult("country")("capital").ToString)
			Console.WriteLine("country => total_area: " & myresult("country")("total_area").ToString)
			Console.WriteLine("country => population: " & myresult("country")("population").ToString)
			Console.WriteLine("country => idd_code: " & myresult("country")("idd_code").ToString)
			Console.WriteLine("country => tld: " & myresult("country")("tld").ToString)
			Console.WriteLine("country => is_eu: " & myresult("country")("is_eu").ToString)
			Console.WriteLine("country => translations: " & myresult("country")("translations")(strLang).ToString)

			Console.WriteLine("country => currency => code: " & myresult("country")("currency")("code").ToString)
			Console.WriteLine("country => currency => name: " & myresult("country")("currency")("name").ToString)
			Console.WriteLine("country => currency => symbol: " & myresult("country")("currency")("symbol").ToString)

			Console.WriteLine("country => language => code: " & myresult("country")("language")("code").ToString)
			Console.WriteLine("country => language => name: " & myresult("country")("language")("name").ToString)
		End If

		' region addon
		If myresult("region") IsNot Nothing Then
			Console.WriteLine("region => name: " & myresult("region")("name").ToString)
			Console.WriteLine("region => code: " & myresult("region")("code").ToString)
			Console.WriteLine("region => translations: " & myresult("region")("translations")(strLang).ToString)
		End If

		' city addon
		If myresult("city") IsNot Nothing Then
			Console.WriteLine("city => name: " & myresult("city")("name").ToString)
			' may not have translations for city names
			Console.WriteLine("city => translations: " & If(myresult("city")("translations").Count <> 0, myresult("city")("translations")(strLang).ToString, ""))
		End If

		' geotargeting addon
		If myresult("geotargeting") IsNot Nothing Then
			Console.WriteLine("geotargeting => metro: " & myresult("geotargeting")("metro").ToString)
		End If

		' country_groupings addon
		If myresult("country_groupings") IsNot Nothing Then
			If myresult("country_groupings").Count > 0 Then
				Dim x As Integer
				Dim max As Integer
				max = myresult("country_groupings").Count - 1
				For x = 0 To max
					Console.WriteLine("country_groupings => #" & x & " => acronym: " & myresult("country_groupings")(x)("acronym").ToString)
					Console.WriteLine("country_groupings => #" & x & " => name: " & myresult("country_groupings")(x)("name").ToString)
				Next
			End If
		End If

		' time_zone_info addon
		If myresult("time_zone_info") IsNot Nothing Then
			Console.WriteLine("time_zone_info => olson: " & myresult("time_zone_info")("olson").ToString)
			Console.WriteLine("time_zone_info => current_time: " & myresult("time_zone_info")("current_time").ToString)
			Console.WriteLine("time_zone_info => gmt_offset: " & myresult("time_zone_info")("gmt_offset").ToString)
			Console.WriteLine("time_zone_info => is_dst: " & myresult("time_zone_info")("is_dst").ToString)
			Console.WriteLine("time_zone_info => sunrise: " & myresult("time_zone_info")("sunrise").ToString)
			Console.WriteLine("time_zone_info => sunset: " & myresult("time_zone_info")("sunset").ToString)
		End If
	Else
		Console.WriteLine("Error: " & myresult("response").ToString)
	End If

	myresult = oIP2LocationWS.GetCredit()

	If myresult("response") IsNot Nothing Then
		Console.WriteLine("Credit balance: " & myresult("response").ToString)
	End If
Catch ex As Exception
	Console.WriteLine(ex.Message)
Finally
	oIP2LocationWS = Nothing
End Try

```

## IPTools Class

## Methods
Below are the methods supported in this class.

|Method Name|Description|
|---|---|
|IsIPv4(ByVal IP As String) As Boolean|Returns true is string contains an IPv4 address. Otherwise false.|
|IsIPv6(ByVal IP As String) As Boolean|Returns true is string contains an IPv6 address. Otherwise false.|
|IPv4ToDecimal(ByVal IP As String) As BigInteger|Returns the IP number for an IPv4 address.|
|IPv6ToDecimal(ByVal IP As String) As BigInteger|Returns the IP number for an IPv6 address.|
|DecimalToIPv4(ByVal IPNum As BigInteger) As String|Returns the IPv4 address for the supplied IP number.|
|DecimalToIPv6(ByVal IPNum As BigInteger) As String|Returns the IPv6 address for the supplied IP number.|
|CompressIPv6(ByVal IP As String) As String|Returns the IPv6 address in compressed form.|
|ExpandIPv6(ByVal IP As String) As String|Returns the IPv6 address in expanded form.|
|IPv4ToCIDR(ByVal IPFrom As String, ByVal IPTo As String) As List(Of String)|Returns a list of CIDR from the supplied IPv4 range.|
|IPv6ToCIDR(ByVal IPFrom As String, ByVal IPTo As String) As List(Of String)|Returns a list of CIDR from the supplied IPv6 range.|
|CIDRToIPv4(ByVal CIDR As String) As (IPStart As String, IPEnd As String)|Returns the IPv4 range from the supplied CIDR.|
|CIDRToIPv6(ByVal CIDR As String) As (IPStart As String, IPEnd As String)|Returns the IPv6 range from the supplied CIDR.|

## Usage

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