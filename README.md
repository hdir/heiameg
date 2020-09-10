# Helsedirektoratet Heia Meg 2018

**Platform Support**

| Platform        | Version |
| --------------- | :-----: |
| Xamarin.iOS     |  8.1+   |
| Xamarin.Android | API 19+ |

## About

An app for Android and iOS for helping people get motivated to change their bad habits

## Requirements

The project requires that you have the following tools installed:

- [Visual Studio IDE 2017](http://visualstudio.com/) (with the following) \* [Xamarin](https://docs.xamarin.com)

## Set up project

1. Open [the solution file](HeiaMeg.sln)
2. Restore nuget packages for solution

## Appcenter integration

[Appcenter](https://appcenter.ms) integration has been added for this app.

### Analytics

Analytics has been implemented for this app for tracking events. These include both data and UI events

### Crash reports and diagnostics

Crash reports and diagnostics have been implemented for this app, and all crashes should deliver a stack trace (where possible)

# REST API

Heia Meg updates messages updated from a REST API. The app will check the API daily for changes.

BaseUrl: <>

#### Terminology

Heia Meg uses 2 types of messages. _Consecutive_ and _Scheduled_ messages.

- Consecutive messages are messages without a specific date. They override scheduled messages and appear consecutively with a predetermined time between them (1-3 days).
- Scheduled messages are messages that will appear only on a specific date.

Messages can also appear between an interval on the day they are scheduled.

#### Current status of API

Get the current status of the API. Used by app to check for update.

```
/api?apikey={APIKEY}
```

sample response:

```
{
	"VersionCode": 1,
	"VersionName": "v1",
	"CronSchedule": "0 0 * * *",
	"LastUpdate": "2019-11-18T00:07:01.4354453Z",
	"ThemesModified": "2018-12-18T20:41:43.856",
	"MessagesModified": "2019-11-12T00:00:32.0335209",
	"ContentModified": "0001-01-01T00:00:00",
	"MinimumModifiedMinutesThreshold": 10,
	"UpdateFrequency": 60
}
```

| Response                        | Description                                                                |
| ------------------------------- | -------------------------------------------------------------------------- |
| VersionCode                     | Current version of API (number)                                            |
| VersionName                     | Current version of API (string)                                            |
| CronSchedule                    | Current frequency database updates from source                             |
| LastUpdate                      | DateTime content was checked for changes                                   |
| ThemesModified                  | DateTime the last message was modified                                     |
| MessagesModified                | Last time messages                                                         |
| ContentModified                 | [deprecated] Last time content was modified                                |
| MinimumModifiedMinutesThreshold | Minutes since content changed before server accepts new changes from Excel |
| UpdateFrequency                 | [deprecated] Freqency of updated content                                   |

#### Specific Theme

Get one theme based on id

```
/api/themes/{THEMEID}?apikey={APIKEY}
```

sample response:

```
{
	"id": 1,
	"name": "Tobakk",
	"title": "Slutte med røyk",
	"description": "Beskrivelse av Tobakk",
	"modified": "2018-12-18T20:41:43.856"
}
```

| Response    | Description                  |
| ----------- | ---------------------------- |
| id          | Id of theme                  |
| name        | Name of theme                |
| title       | Title of theme               |
| description | Description of theme         |
| modified    | Last time theme was modified |

#### All Themes

Get an array of all themes

```
/api/themes?apikey={APIKEY}
```

#### Specific Message

Get one message based on id

```
/api/messages/{MESSAGEID}?apikey={APIKEY}
```

sample response:

```
{
	"id": 1003,
	"themeId": 1,
	"text": "Nikotinlegemidler gjør det lettere å slutte, så det kan være smart å ha i bakhånd når du bestemmer deg for å slutte. Har du en sluttevenn du kan fortelle at du prøver å slutte? Denne kan hjelpe deg å holde motet oppe underveis 😊",
	"link": "https://helsenorge.no/rus-og-avhengighet/snus-og-roykeslutt/hjelpemidler-ved-roykeslutt",
	"linkText": "Hjelpemidler til røykeslutt",
	"from": "12:00:00",
	"to": "12:00:00",
	"date": null,
	"modified": "2019-10-21T08:02:54.9833144"
}
```

| Response | Description                                                                      |
| -------- | -------------------------------------------------------------------------------- |
| id       | Id of message                                                                    |
| themeId  | Id of theme                                                                      |
| text     | Text of message                                                                  |
| link     | Link of message                                                                  |
| linkText | Displayed text on link                                                           |
| from     | Start timespan message can appear                                                |
| to       | End timespan message can appear                                                  |
| date     | Date message can appear (for scheduled messages, null if message is consecutive) |
| modified | [deprecated] Freqency of updated content                                         |

#### All Messages

Get an array of all messages.

```
/api/messages?apikey={APIKEY}
```

#### Messages after date

Get all messages after specific date (also include consecutive messages).

```
/api/messages?since={DATETIMETICKS}&apikey={APIKEY}
```

#### Modified Messages

Get all messages modified after specific date

```
/api/messages?modifiedSince={DATETIMETICKS}&apikey={APIKEY}
```

#### Messages modified after date

Combined request for messages after certain date which have been modified after date (includes consecutive messages)

```
/api/messages?since={DATETIMETICKS}&modifiedSince={DATETIMETICKS}&apikey={APIKEY}
```

## Licenses

- [MvvmCross - MS-PL](https://opensource.org/licenses/ms-pl.html)
- [Xamarin Android Appcompat - Mit](https://github.com/xamarin/AndroidSupportComponents/blob/master/LICENSE.md)
- [sqlite-net-pcl - MIT](https://github.com/praeclarum/sqlite-net/blob/master/LICENSE.md)
- [Settings plugins for Xamarin and Windows - MIT](https://github.com/jamesmontemagno/SettingsPlugin/blob/master/LICENSE)
- [ACR UserDialogs - MIT](https://github.com/aritchie/userdialogs/blob/master/LICENSE.md)
- [CarouselView.FormsPlugin - MIT](https://github.com/alexrainman/CarouselView/blob/master/README.md)
- [Xamarin.FFImageLoading.Svg.Forms - MIT](https://raw.githubusercontent.com/luberda-molinet/FFImageLoading/master/LICENSE.md)
- [VG.XFShapeView.NetCore - MIT](https://github.com/vincentgury/XFShapeView/blob/master/LICENSE)
- [Rg.Plugins.Popup - MIT](https://github.com/rotorgames/Rg.Plugins.Popup/blob/master/LICENSE.md)
- [FlowListView for Xamarin.Forms - Apache](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/LICENSE)
- [SkiaSharp - MIT](https://github.com/mono/SkiaSharp/blob/master/LICENSE.md)
- [Xam.Plugin.Connectivity - MIT](https://github.com/jamesmontemagno/ConnectivityPlugin/blob/master/LICENSE)
- [Newtonsoft.Json - MIT](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
- [Microsoft.AppCenter - MIT](https://github.com/Microsoft/AppCenter-SDK-DotNet/blob/master/license.txt)
- [WEB - ncrontab - Apache](https://github.com/atifaziz/NCrontab)
- [WEB - Google.Apis - Apache](https://github.com/googleapis/google-api-dotnet-client)
