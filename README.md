<h1 align="center">
  SqlConsole2
</h1>

<h4 align="center">A lightweight application for running queries on a variety of data sources</h4>

<h4 align="center">
  <a href="#features">Features</a>&nbsp;|&nbsp;
  <a href="#download">Download</a>&nbsp;|&nbsp;
  <a href="#download">Installation</a>&nbsp;|&nbsp;
  <a href="#credits">License</a>&nbsp;|&nbsp;
  <a href="#credits">Credits</a>
</h4>

![Query tab screenshot](https://raw.githubusercontent.com/mayakron/sqlconsole2/main/resources/SqlConsole2QueryTabScreenshot.png)

## Features

* Portable
* Lightweight
* Can query the following data sources:
  - Microsoft SQL Server
  - MySql
  - Odbc
  - OleDb
  - SQLite
  - Windows Management Instrumentation (WMI)
* Can export data sets to the following file formats:
  - Csv
  - Tsv
  - Xml
  - Microsoft Excel
* Can browse data cells containing many binary and text formats (documents, images, html, xml, etc.)
* Written in C# using the .NET Framework and Windows Presentation Foundation (WPF)

SqlConsole2 is a lightweight application by design, its purpose being to allow you to query a variety of data sources with a minimal amount of overhead.

## Download

The latest and all other versions of SqlConsole2 can be downloaded from [here](https://github.com/mayakron/sqlconsole2/releases).

## Installation

For the time being SqlConsole2 is available in portable format only, therefore to install it you just need to expand its archive to a directory of your choice and run the SqlConsole2.exe file. To uninstall SqlConsole2 it is enough to delete that directory.

## License

[GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html)

## Credits

This software uses the following open source packages:

- [EPPlus](https://www.nuget.org/packages/EPPlus/4.5.3.1)
- [MySql.Data](https://www.nuget.org/packages/MySql.Data/6.9.9)
- [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core/1.0.110)