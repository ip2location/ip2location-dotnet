# IP2Location .NET API

## Component Class

```{py:function} Open(DBPath, UseMMF)
Open and load the IP2Location BIN database for lookup.

:param String DBPath: (Required) The file path links to IP2Location BIN databases.
:param Boolean UseMMF: (Optional) Specify whether to use MemoryMappedFile then preload BIN file or not. Default is False.
```

```{py:function} Open(DBStream)
Initialize component with a stream that contains the BIN database then preload BIN file.

:param String DBPath: (Required) A stream that contains the BIN database.
```

```{py:function} Close()
Closes BIN file and resets metadata.
```

```{py:function} IPQuery(_IPAddress)
Retrieve geolocation information for an IP address.

:param String _IPAddress: (Required) The IP address (IPv4 or IPv6).
:return: Returns the geolocation information. Refer below table for the fields avaliable.

**RETURN FIELDS**

| Field Name       | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| CountryShort    |     Two-character country code based on ISO 3166. |
| CountryLong     |     Country name based on ISO 3166. |
| Region           |     Region or state name. |
| City             |     City name. |
| InternetServiceProvider              |     Internet Service Provider or company\'s name. |
| Latitude         |     City latitude. Defaults to capital city latitude if city is unknown. |
| Longitude        |     City longitude. Defaults to capital city longitude if city is unknown. |
| DomainName           |     Internet domain name associated with IP address range. |
| ZipCode          |     ZIP code or Postal code. [172 countries supported](https://www.ip2location.com/zip-code-coverage). |
| TimeZone         |     UTC time zone (with DST supported). |
| NetSpeed         |     Internet connection type. |
| IDDCode         |     The IDD prefix to call the city from another country. |
| AreaCode        |     A varying length number assigned to geographic areas for calls between cities. [223 countries supported](https://www.ip2location.com/area-code-coverage). |
| WeatherStationCode     |     The special code to identify the nearest weather observation station. |
| WeatherStationName     |     The name of the nearest weather observation station. |
| MCC              |     Mobile Country Codes (MCC) as defined in ITU E.212 for use in identifying mobile stations in wireless telephone networks, particularly GSM and UMTS networks. |
| MNC              |     Mobile Network Code (MNC) is used in combination with a Mobile Country Code(MCC) to uniquely identify a mobile phone operator or carrier. |
| MobileBrand     |     Commercial brand associated with the mobile carrier. You may click [mobile carrier coverage](https://www.ip2location.com/mobile-carrier-coverage) to view the coverage report. |
| Elevation        |     Average height of city above sea level in meters (m). |
| UsageType       |     Usage type classification of ISP or company. |
| AddressType     |     IP address types as defined in Internet Protocol version 4 (IPv4) and Internet Protocol version 6 (IPv6). |
| Category         |     The domain category based on [IAB Tech Lab Content Taxonomy](https://www.ip2location.com/free/iab-categories). |
| District         |     District or county name. |
| ASN              |     Autonomous system number (ASN). BIN databases. |
| AS          |     Autonomous system (AS) name. |
```

```{py:function} IPQueryAsync(_IPAddress)
Query IP address asynchronously.

:param String _IPAddress: (Required) The IP address (IPv4 or IPv6).
:return: Returns the geolocation information. Refer below table for the fields avaliable.

**RETURN FIELDS**

| Field Name       | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| CountryShort    |     Two-character country code based on ISO 3166. |
| CountryLong     |     Country name based on ISO 3166. |
| Region           |     Region or state name. |
| City             |     City name. |
| InternetServiceProvider              |     Internet Service Provider or company\'s name. |
| Latitude         |     City latitude. Defaults to capital city latitude if city is unknown. |
| Longitude        |     City longitude. Defaults to capital city longitude if city is unknown. |
| DomainName           |     Internet domain name associated with IP address range. |
| ZipCode          |     ZIP code or Postal code. [172 countries supported](https://www.ip2location.com/zip-code-coverage). |
| TimeZone         |     UTC time zone (with DST supported). |
| NetSpeed         |     Internet connection type. |
| IDDCode         |     The IDD prefix to call the city from another country. |
| AreaCode        |     A varying length number assigned to geographic areas for calls between cities. [223 countries supported](https://www.ip2location.com/area-code-coverage). |
| WeatherStationCode     |     The special code to identify the nearest weather observation station. |
| WeatherStationName     |     The name of the nearest weather observation station. |
| MCC              |     Mobile Country Codes (MCC) as defined in ITU E.212 for use in identifying mobile stations in wireless telephone networks, particularly GSM and UMTS networks. |
| MNC              |     Mobile Network Code (MNC) is used in combination with a Mobile Country Code(MCC) to uniquely identify a mobile phone operator or carrier. |
| MobileBrand     |     Commercial brand associated with the mobile carrier. You may click [mobile carrier coverage](https://www.ip2location.com/mobile-carrier-coverage) to view the coverage report. |
| Elevation        |     Average height of city above sea level in meters (m). |
| UsageType       |     Usage type classification of ISP or company. |
| AddressType     |     IP address types as defined in Internet Protocol version 4 (IPv4) and Internet Protocol version 6 (IPv6). |
| Category         |     The domain category based on [IAB Tech Lab Content Taxonomy](https://www.ip2location.com/free/iab-categories). |
| District         |     District or county name. |
| ASN              |     Autonomous system number (ASN). BIN databases. |
| AS          |     Autonomous system (AS) name. |
```

## IPTools Class

```{py:class} IPTools()
Initiate IPTools class.
```

```{py:function} IsIPv4(IP)
Verify if a string is a valid IPv4 address.

:param String IP: (Required) IP address.
:return: Return True if the IP address is a valid IPv4 address or False if it isn't a valid IPv4 address.
:rtype: Boolean
```

```{py:function} IsIPv6(IP)
Verify if a string is a valid IPv6 address

:param String IP: (Required) IP address.
:return: Return True if the IP address is a valid IPv6 address or False if it isn't a valid IPv6 address.
:rtype: Boolean
```

```{py:function} IPv4ToDecimal(IP)
Translate IPv4 address from dotted-decimal address to decimal format.

:param String IP: (Required) IPv4 address.
:return: Return the decimal format of the IPv4 address.
:rtype: BigInteger
```

```{py:function} DecimalToIPv4(IPNum)
Translate IPv4 address from decimal number to dotted-decimal address.

:param String IPNum: (Required) Decimal format of the IPv4 address.
:return: Returns the dotted-decimal format of the IPv4 address.
:rtype: String
```

```{py:function} IPv6ToDecimal(IP)
Translate IPv6 address from hexadecimal address to decimal format.

:param String IP: (Required) IPv6 address.
:return: Return the decimal format of the IPv6 address.
:rtype: BigInteger
```

```{py:function} DecimalToIPv6(IPNum)
Translate IPv6 address from decimal number into hexadecimal address.

:param String IPNum: (Required) Decimal format of the IPv6 address.
:return: Returns the hexadecimal format of the IPv6 address.
:rtype: String
```

```{py:function} IPv4ToCIDR(IPFrom, IPTo)
Convert IPv4 range into a list of IPv4 CIDR notation.

:param String IPFrom: (Required) The starting IPv4 address in the range.
:param String IPTo: (Required) The ending IPv4 address in the range.
:return: Returns the list of IPv4 CIDR notation.
:rtype: List
```

```{py:function} CIDRToIPv4(CIDR)
Convert IPv4 CIDR notation into a list of IPv4 addresses.

:param String CIDR: (Required) IPv4 CIDR notation.
:return: Returns an list of IPv4 addresses.
:rtype: List
```

```{py:function} IPv6ToCIDR(IPFrom, IPTo)
Convert IPv6 range into a list of IPv6 CIDR notation.

:param String IPFrom: (Required) The starting IPv6 address in the range.
:param String IPTo: (Required) The ending IPv6 address in the range.
:return: Returns the list of IPv6 CIDR notation.
:rtype: List
```

```{py:function} CIDRToIPv6(CIDR)
Convert IPv6 CIDR notation into a list of IPv6 addresses.

:param String CIDR: (Required) IPv6 CIDR notation.
:return: Returns an list of IPv6 addresses.
:rtype: List
```


```{py:function} CompressIPv6(IP)
Compress a IPv6 to shorten the length.

:param String IP: (Required) IPv6 address.
:return: Returns the compressed version of IPv6 address.
:rtype: String
```

```{py:function} ExpandIPv6(IP)
Expand a shorten IPv6 to full length.

:param String IP: (Required) IPv6 address.
:return: Returns the extended version of IPv6 address.
:rtype: String
```

## Country Class

```{py:class} Country(CSVFile)
Initiate Country class and load the IP2Location Country Information CSV file. This database is free for download at <https://www.ip2location.com/free/country-information>.

:param String CSVFile: (Required) The file path links to IP2Location Country Information CSV file.
```

```{py:function} GetCountryInfo(CountryCode)
Provide a ISO 3166 country code to get the country information in array. Will return a full list of countries information if country code not provided. 

:param String CountryCode: (Required) The ISO 3166 country code of a country.
:return: Returns the country information in List. Refer below table for the fields avaliable in the List.
:rtype: List

**RETURN FIELDS**

| Field Name       | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| country_code     | Two-character country code based on ISO 3166.                |
| country_alpha3_code | Three-character country code based on ISO 3166.           |
| country_numeric_code | Three-character country code based on ISO 3166.          |
| capital          | Capital of the country.                                      |
| country_demonym  | Demonym of the country.                                      |
| total_area       | Total area in km{sup}`2`.                                    |
| population       | Population of year 2014.                                     |
| idd_code         | The IDD prefix to call the city from another country.        |
| currency_code    | Currency code based on ISO 4217.                             |
| currency_name    | Currency name.                                               |
| currency_symbol  | Currency symbol.                                             |
| lang_code        | Language code based on ISO 639.                              |
| lang_name        | Language name.                                               |
| cctld            | Country-Code Top-Level Domain.                               |
```

## Region Class

```{py:class} Region(CSVFile)
Initiate Region class and load the IP2Location ISO 3166-2 Subdivision Code CSV file. This database is free for download at <https://www.ip2location.com/free/iso3166-2>

:param String CSVFile: (Required) The file path links to IP2Location ISO 3166-2 Subdivision Code CSV file.
```

```{py:function} GetRegionCode(CountryCode, RegionName)
Provide a ISO 3166 country code and the region name to get ISO 3166-2 subdivision code for the region.

:param String CountryCode: (Required) Two-character country code based on ISO 3166.
:param String RegionName: (Required) Region or state name.
:return: Returns the ISO 3166-2 subdivision code of the region.
:rtype: String
```
