The text field above contains the query "context" in the form of "ProviderType={{?}};ConnectionString={{?}};CommandTimeout={{?}}", while this text area can contain whatever you want.

Please note that to execute a query its text must have been previously selected from this area.

Supported provider types are:

* MySql
* Odbc
* OleDb
* SQLite
* SqlServer
* Wmi

Some examples of query contexts can be found below:

-- MySql

ProviderType={{MySql}};ConnectionString={{Server=;Database=;Uid=;Pwd=}};CommandTimeout={{120}}
ProviderType={{MySql}};ConnectionString={{Server=;Port=;Database=;Uid=;Pwd=}};CommandTimeout={{120}}

-- Odbc

ProviderType={{Odbc}};ConnectionString={{Server=;Database=;Uid=;Pwd=}};CommandTimeout={{120}}

-- SQLite

ProviderType={{SQLite}};ConnectionString={{Data Source=C:\Temp\Test.db}};CommandTimeout={{120}}

CREATE TABLE Test(Col1 TEXT, Col2 TEXT, Col3 TEXT, Col4 TEXT, Col5 TEXT, Col6 TEXT, Col7 TEXT, Col8 TEXT, Col9 TEXT);
INSERT INTO Test(Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9) VALUES('Antonia Joseph', 'Clarissa Waters', 'Chet Petty', 'Jeanette Schultz', 'Caroline Stanton', 'Porfirio Salas', 'Marquis Lucero', 'Mandy Powell', 'Bridgette Cain');
INSERT INTO Test(Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9) VALUES('Rita Cox', 'August Olsen', 'Concepcion Woodward', 'Denis Mcmahon', 'Alexandra Fields', 'Michelle Henson', 'Jacques Gray', 'Michal Davidson', 'Chung Mason');
INSERT INTO Test(Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9) VALUES('Marion Silva', 'Greta Sullivan', 'Kendra Cooke', 'Luke Little', 'Leroy Knight', 'Eduardo Richmond', 'Jerome Harrington', 'Emmitt Bernard', 'Warner Lyons');
INSERT INTO Test(Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9) VALUES('Chi Lynn', 'Daisy Watkins', 'Ezra Cordova', 'Burt Clay', 'Stanley Compton', 'Maria Dickson', 'Shelby Jefferson', 'Bret Glenn', 'Mitzi Rasmussen');
INSERT INTO Test(Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9) VALUES('Nigel Warren', 'Doug Rowland', 'Peggy Stone', 'Stacie Conner', 'Elsie Ramos', 'Mose Alvarado', 'Chase Hansen', 'Abe Callahan', 'Isabel Knox');

SELECT * FROM Test;

-- Microsoft Sql Server

ProviderType={{SqlServer}};ConnectionString={{Data Source=;Initial Catalog=;User ID=;Password=}};CommandTimeout={{120}}
ProviderType={{SqlServer}};ConnectionString={{Data Source=;Initial Catalog=;Integrated Security=SSPI}};CommandTimeout={{120}}

-- Windows Management Instrumentation (WMI)

ProviderType={{Wmi}};ConnectionString={{Path=;Domain=;UserName=;Password=}};CommandTimeout={{120}}
ProviderType={{Wmi}};ConnectionString={{Path=\\.\root\cimv2;Domain=;UserName=;Password=}};CommandTimeout={{120}}

SELECT Name FROM Win32_Process