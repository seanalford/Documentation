---
layout: Meadow
title: Networking
subtitle: Network options and operation.
---

Both the Meadow F7 development board and SMT module have WiFi networking via the ESP32 co-processor. The Meadow F7 embedded SMT module also adds optional ethernet capabilities.

## Current Beta Limitations

 * **All SSL Certificates Accepted** - SSL connections are supported, but currently, all certificates over TLS (https) are accepted without any validation.

## Sample Apps

For example code, see the following networking sample apps in the [Meadow.Core.Samples repo](https://github.com/wildernesslabs/Meadow.Core.Samples):
 * **[Wifi_Basics](https://github.com/WildernessLabs/Meadow.Core.Samples/tree/main/Source/Meadow.Core.Samples/Network/WiFi_Basics)** - Covers the basics of enumerating and connecting to WiFi networks.
 * **[HttpListener](https://github.com/WildernessLabs/Meadow.Core.Samples/tree/main/Source/Meadow.Core.Samples/Network/HttpListener)** - Shows how to respond to HTTP requests with `HttpListenerContext`, `HttpListenerRequest`, and `HttpListenerResponse`.
 * **[Antenna Switching](https://github.com/WildernessLabs/Meadow.Core.Samples/tree/main/Source/Meadow.Core.Samples/Network/Antenna_Switching)** - Shows how to use the antenna API to switch between the onboard and external antenna connection.

# WiFi

## Initializing the `WiFiAdapter`

In order to use wifi networking, you must first initialize the `WiFiAdpater` by calling `InitWiFiAdapter()` on the `F7Micro` device:

```csharp
Device.InitWiFiAdapter().Wait();
```

The intialization method can take 5 or more seconds, and is awaitable.

Once initialized, the `WiFiAdapter` is available as a property on the `F7Micro` class and can be accessed via the `Device` property in your app class:

```csharp
// from within your app class:
Device.WiFiAdapter

// from other classes, where [MeadowApp] is the name of your app class:
[MeadowApp].Device.WiFiAdapter
```

## Connecting to a WiFi Network

Once the `WiFiAdapter` has been initialized, you can connect to a network by calling the `Connect` method and passing in the SSID (network name), and password:

```csharp
if (Device.WiFiAdapter.Connect("SSID", "Pass").ConnectionStatus != ConnectionStatus.Success) {
    throw new Exception("Cannot connect to network, applicaiton halted.");
}
```

## Scanning for WiFi Networks

You can also scan for WiFi networks via the `Scan()` method on the `WiFiAdapter` and then access the network list via the `Networks` `ObservableCollection` property:

```csharp
protected void ScanForAccessPoints()
{
    Console.WriteLine("Getting list of access points.");
    ObservableCollection<WifiNetwork> networks = Device.WiFiAdapter.Scan();
    if (networks.Count > 0) {
        Console.WriteLine("|-------------------------------------------------------------|---------|");
        Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
        Console.WriteLine("|-------------------------------------------------------------|---------|");
        foreach (WifiNetwork accessPoint in networks) {
            Console.WriteLine($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
        }
    } else {
        Console.WriteLine($"No access points detected.");
    }
}
```

# Performing Requests

Once the network is connected, you can generally use the built-in .NET network methods as usual, however `HttpServer` is not availble in this beta.

## `HttpClient.Request()` Example

The following code illustrates making a request to a web page via the `HttpClient` class:

```csharp
using (HttpClient client = new HttpClient()) {
    HttpResponseMessage response = await client.GetAsync(uri);
    response.EnsureSuccessStatusCode();
    string responseBody = await response.Content.ReadAsStringAsync();
    Console.WriteLine(responseBody);
}
```

### Posting

You can also modify the request to `POST` data. For example, the following code posts a temperature reading to Adafruit.IO:

```csharp
using (HttpClient client = new HttpClient()) {
    client.DefaultRequestHeaders.Add("X-AIO-Key", [APIO_KEY]);
    client.Timeout = TimeSpan.FromMinutes(10);
    string temperature = "23.70";
    string data = "{\"value\":\"" + temperature + "\"}";
    var content = new StringContent(data, Encoding.UTF8, "application/json");
    var result = client.PostAsync(uri, content).Result;
}
```

# Antenna

Both the Meadow development board and production module have an onboard ceramic chip antenna and a U.FL connector for an external antenna for the 2.4GHz WiFi and Bluetooth radio.

For more information on getting the curent antenna information and switching, see the [Antenna guide](/Meadow/Meadow_Basics/Networking/Antenna).

# Creating RESTful Web APIs with Maple Server

If you need to expose simple RESTful Web APIs, Meadow.Foundation includes a lightweight web server called Maple Server that may be useful. Check out the [Maple Server guide](/Meadow/Meadow.Foundation/Libraries_and_Frameworks/Maple.Server/) for more information.