# Brazil Report - Web System: rBR Scraper .NET API

![description](https://github.com/relatorio-brasil/rbr-api-scraper/blob/dev/rBR.Scraper/rBR.Scraper.Service/wwwroot/Image/01.png)

**.NET API Web Service for social media data import tools.**

> Repository intended for .NET services responsible for obtaining social media data. Currently, such data comes from the APIFY system for data acquisition through scrapers.
> **Update this file with important instructions for the development team.**

# Project Technical Requirements

* .NET 8.0 LTS;
* Follow the coding style proposed by Microsoft;
* Follow architectural standards;
* Maintain XML code documentation.

## Authentication

Authentication for each application will be performed by associating the following information: **Access Token, Authentication and Authorization Policies, and Signing Keys**.

To authenticate in the rBR Scraper application with configured Authentication and Authorization Policies, as well as Signing Keys, it is necessary to have the Access Token obtained directly from the **rBR Auth Manager** system.

### Obtaining the Authentication Token:

> At the present moment, the rBR Auth Manager authentication system is not yet available in Development, Staging and/or Production environments. Therefore, it must be executed locally to allow obtaining the Access Token.

* Location: http://localhost:5001/API/v1.0/User/Account/Login
* Method: POST;
* Header: Content-Type: application/json;
* Body:
```
{
   "UserName":"{username}",
   "Password":"{password}"
}
```
* Response:
```
{
	"AccessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
	"Expires": "06/05/2025 11:59:21 -03:00"
}
```
## Static Environment Configurations

Several static configurations are necessary for the correct functioning of the application. Among them are: Authentication and Authorization, Database, CORS, Culture and Swagger.

### Security Settings for the JWT token (JWKS)

The configuration file `appsettings.{*environment*}.json` must contain, inside the *BaseAuthSettings* object, the object:
```
{
	"JwksSettings": {
		"JwksSettings": {
			"DefaultClockSkew": 15,
			"Issuer": "rBR",
			"IssuerSigningKey": "6a66eb7a-ba32-4ea6-b53d-2ede9bffb3e7",
			"Key": "6a66eb7a-ba32-4ea6-b53d-2ede9bffb3e7"
		}
	}
}
```

### Security Settings for Authorization Policies

Since full control of authorization profiles is not yet operational, applications currently include all profiles that have been presented/used at some point. Therefore, the configuration file `appsettings.{*environment*}.json` must contain, inside the *BaseAuthSettings* object, the object:
```
{
   "AuthorizationPolicySettings":{
      "AuthPolicies":[
         {
            "Claims":[
               {
                  "Type":"Admin",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"Application",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"Client",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"DisplayName"
               },
               {
                  "Type":"Email"
               },
               {
                  "Type":"Status",
                  "Values":[
                     "1"
                  ]
               },
               {
                  "Type":"UserName"
               }
            ],
            "Name":"rBR Admin"
         },
         {
            "Claims":[
               {
                  "Type":"Admin",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"Application",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"Client",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"DisplayName"
               },
               {
                  "Type":"Email"
               },
               {
                  "Type":"Status",
                  "Values":[
                     "1"
                  ]
               },
               {
                  "Type":"UserName"
               }
            ],
            "Name":"rBR Support"
         },
         {
            "Claims":[
               {
                  "Type":"Application",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"Client",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"DisplayName"
               },
               {
                  "Type":"Email"
               },
               {
                  "Type":"Status",
                  "Values":[
                     "1"
                  ]
               },
               {
                  "Type":"UserName"
               }
            ],
            "Name":"rBR Client"
         },
         {
            "Claims":[
               {
                  "Type":"Application",
                  "Values":[
                     "rBR"
                  ]
               },
               {
                  "Type":"DisplayName"
               },
               {
                  "Type":"Email"
               },
               {
                  "Type":"Status",
                  "Values":[
                     "1"
                  ]
               },
               {
                  "Type":"UserName"
               }
            ],
            "Name":"rBR User"
         }
      ]
   }
}
```
### JWT Token Authentication Validation Settings

The *BaseAuthSettings* object must contain the object:
```
{
	"JwtAuthenticationSettings": {
		"RequireAudience": true,
		"RequireExpirationTime": true,
		"RequireHttpsMetadata": true,
		"RequireSignedTokens": true,
		"SaveToken": true,
		"ValidateAudience": true,
		"ValidateIssuer": true,
		"ValidateIssuerSigningKey": true,
		"ValidateLifetime": true,
		"ValidAudience": "rBR"
	}
}
```

### Cross-Origin Resource Sharing - CORS Settings

The CORS configuration was set up as future provisioning. That is, it is not enforced but is available. For it to be made available in code, the file `appsettings.{*environment*}.json` must contain the object:
```
{
	"BaseCorsPolicySettings": {
		"Headers": [],
		"Methods": [],
		"Name": "rBR",
		"Origins": []
	}	
}
```
### Database Settings

Database configurations are still local, as Development, Staging and/or Production environments are not yet available. The configuration supports MySQL and MSSQL Server databases. To apply them, the *DatabaseType* field can be changed from 0 (MySQL) to 1 (MSSQL). The file `appsettings.{*environment*}.json` must contain the object:
```
{
	"BaseContextSettings": {
		"ConnectionStrings": [
				{
				"ConnectionStringMySQL": "server=127.0.0.1;port=3306;database=rBRScraper_Development;uid={your_user};password={your_password}",
				"ConnectionStringSQLServer": "Data Source={sua_instância}; Initial Catalog=rBRScraper_Development; Integrated Security=False; User Id={seu_usuário}; Password={sua_senha}; TrustServerCertificate=true",
				"ContextName": "rBRScraperContext",
				"DatabaseType": 0
				}
			]
		}
	}
```

### Culture Settings

Culture settings establish which cultures are supported by the API controllers. The file `appsettings.{*environment*}.json` must contain the object:
```
{
	"BaseControllersSettings": {
		"SupportedCultures": [
			"en-US",
			"pt-BR"
		]
	}
}
```

### Swagger Settings

The file `appsettings.{*environment*}.json` must contain the object:
```
{
   "BaseSwaggerSettings":{
      "ApiExplorer":{
         "GroupNameFormat":"V.v",
         "SubstitutionFormat":"V.v"
      },
      "Description":"rBR Scraper - Logo",
      "DocumentName":"rBR Scraper - Web API",
      "GenerateExamples":true,
      "GenerateXmlObjects":true,
      "OpenApiSecurityScheme":{
         "Description":"JWT authorization header using the Bearer schema. Type 'Bearer' [space] and then your token in the text input below. Example: Bearer {token}",
         "Name":"Authorization"
      },
      "Title":"rBR Scraper - Development",
      "UseControllerSummaryAsTagDescription":true,
      "UseRouteNameAsOperationId":false,
      "UseXmlDocumentation":true
   }
}
```

### APIFY System Connection Settings:

The file `appsettings.{*environment*}.json` must contain the object:
```
{
   "ApiFySettings":{
      "AccessToken":"apify_api_NEF0KDgXWsMDq6O6CMs3FzMurUhVij1NhxMR",
      "UrlBase":"https://api.apify.com/v2",
      "EndpointListActors":"/acts",
      "EndpointGetActor":"/acts/{0}",
      "EndpointListRuns":"/acts/{0}/runs",
      "EndpointGetDatasetItems":"/datasets/{0}/items?format=json&skipEmpty=true&skipHidden=true"
   }
}
```

### Active APIFY Scrapers Settings:
```
{
   "ScraperSettings":{
      "ActiveConfiguredScrapers":[
         "shu8hvrXbJbY3Eb9W",
         "dSCLg0C3YEZ83HzYX"
      ]
   }
}
```

## Features

1. **Admin - Instagram Datasets**:
> * <u>GET - /API/v1.0/Admin/InstagramDataset</u>: _listing of imported Instagram Scraper Datasets;_
> * <u>POST - /API/v1.0/Admin/InstagramDataset/{datasetDataId}</u>: _import of a single Instagram Scraper Dataset;_
> * <u>POST - /API/v1.0/Admin/InstagramDataset/ImportAll/{scraperId}</u>: _import of all Datasets from a single Scraper;_
> * <u>GET - /API/v1.0/Admin/InstagramDataset/ImportAll/{scraperId}</u>: _listing of Datasets by Run._

2. **Admin - Instagram Profile Datasets**:
> * <u>GET - /API/v1.0/Admin/InstagramProfileDataset</u>: _listing of imported Instagram Profile Scraper Datasets;_
> * <u>POST -/API/v1.0/Admin/InstagramProfileDataset/{datasetDataId}</u>: _import of a single Instagram Profile Scraper Dataset;_
> * <u>POST - /API/v1.0/Admin/InstagramProfileDataset/ImportAll/{scraperId}</u>: _import of all Datasets from a single Scraper;_
> * <u>GET - /API/v1.0/Admin/InstagramProfileDataset/ListByRun</u>: _listing of Datasets by Run._

3. **Admin - Runs**:
> * <u>POST - /API/v1.0/Admin/Run</u>: _creation of a list of Scraper Runs;_
> * <u>GET - /API/v1.0/Admin/Run</u>: _listing of Scraper Runs already created/imported;_
> * <u>GET - /API/v1.0/Admin/Run/{id}</u>: _retrieval of a single Run by Id;_
> * <u>GET - /API/v1.0/Admin/Run/ListFromApiFy</u>: _listing of APIFY Runs, filtered by Scraper._

4. **Admin - Scrapers**:
> * <u>POST - /API/v1.0/Admin/Scraper</u>: _creation of a list of Scrapers;_
> * <u>GET - /API/v1.0/Admin/Scraper</u>: _listing of Scrapers already created/imported;_
> * <u>GET - /API/v1.0/Admin/Scraper/{id}</u>: _retrieval of a single Scraper by Id;_
> * <u>GET - /API/v1.0/Admin/Scraper/GetFromApiFy/{id}</u>: _retrieval of a single Scraper from APIFY;_
> * <u>GET - /API/v1.0/Admin/Scraper/ListFromApiFy</u>: _listing of APIFY Scrapers._

## Data Structure - MySQL MariaDB

1. **CommonScrapers**

    |Name|Nullable|Type|Precision|Key|
    |--|--|--|--|--|
    |Id|	No|	varchar(36)|	-|	PRI|
    |Created|	No|	varchar(33)|	-|	-|
    |Modified|	Yes|	varchar(33)|	-|	-|
    |Removed|	Yes|	varchar(33)|	-|	-|
    |Status|	No|	int(11)|	10|	-|
    |DataId|	No|	varchar(50)|	-|	UNI|
    |Title|	No|	varchar(100)|	-|	-|
    |Description|	No|	text|	-|	-|
    |Name|	No|	varchar(100)|	-|	-|
    |CreatedAt|	No|	varchar(33)|	-|	-|
    |ModifiedAt|	Yes|	varchar(33)|	-|	-|

---

2. **InstagramProfileScraperDatasets**

    |Name|Nullable|Type|Precision|Key|
    |--|--|--|--|--|
    |Id|	No|	varchar(36)|	-|	PRI|
    |Created|	No|	varchar(33)|	-|	-|
    |Modified|	Yes|	varchar(33)|	-|	-|
    |Removed|	Yes|	varchar(33)|	-|	-|
    |Status|	No|	int(11)|	10|	-|
    |DataId|	No|	varchar(50)|	-|	-|
    |Url|	No|	varchar(300)|	-|	-|
    |InputUrl|	No|	varchar(300)|	-|	-|
    |UserName|	No|	varchar(150)|	-|	MUL|
    |FullName|	No|	varchar(150)|	-|	-|
    |FollowersCount|	Yes|	int(11)|	10|	-|
    |Verified|	Yes|	bit(1)|	1|	-|
    |Timestamp|	No|	varchar(33)|	-|	-|
    |RunId|	No|	varchar(36)|	-|	MUL|
    |FullObject|	No|	longtext|	-|	-|
    ```mermaid
    graph LR
    A(InstagramProfileScraperDatasets) === FK_RunId  ==> B(ScrapersRuns)
---

3. **InstagramScraperDatasets**

    |Name|Nullable|Type|Precision|Key|
    |--|--|--|--|--|
    |Id|	No|	varchar(36)|	-|	PRI|
    |Created|	No|	varchar(33)|	-|	-|
    |Modified|	Yes|	varchar(33)|	-|	-|
    |Removed|	Yes|	varchar(33)|	-|	-|
    |Status|	No|	int(11)|	10|	-|
    |DataId|	No|	varchar(50)|	-|	-|
    |Url|	No|	varchar(300)|	-|	-|
    |InputUrl|	No|	varchar(300)|	-|	-|
    |Timestamp|	No|	varchar(33)|	-|	-|
    |RunId|	No|	varchar(36)|	-|	MUL|
    |InstagramProfileId|	No|	varchar(36)|	-|	MUL|
    |FullObject|	No|	longtext|	-|	-|
    ```mermaid
    graph LR
    A(InstagramScraperDatasets) === FK_RunId  ==> B(ScrapersRuns)
    A(InstagramScraperDatasets) === FK_InstagramProfileId  ==> C(InstagramProfileScraperDatasets)
    ```
---

4. **ScrapersRuns**

    |Name|Nullable|Type|Precision|Key|
    |--|--|--|--|--|
    |Id|	No|	varchar(36)|	-|	PRI|
    |Created|	No|	varchar(33)|	-|	-|
    |Modified|	Yes|	varchar(33)|	-|	-|
    |Removed|	Yes|	varchar(33)|	-|	-|
    |Status|	No|	int(11)|	10|	-|
    |DataId|	No|	varchar(50)|	-|	MUL|
    |DataStatus|	No|	varchar(50)|	-|	-|
    |DatasetId|	No|	varchar(50)|	-|	-|
    |StartedAt|	No|	varchar(33)|	-|	-|
    |FinishedAt|	No|	varchar(33)|	-|	-|
    |Imported|	No|	bit(1)|	1|	-|
    |ImportingError|	Yes|	text|	-|	-|
    |ScraperId|	No|	varchar(36)|	-|	MUL|
    ```mermaid
    graph LR
    A(ScrapersRuns) === FK_ScraperId  ==> B(CommonScrapers)
    ```

## Local Environment and Execution

1. **Requirements**

   - Database - MySQL MariaDB v. 10.5.9 (can be obtained from the link [Download MariaDB](https://mariadb.org/download/?t=mariadb&p=mariadb&r=11.3.0)), and
   - .NET 8.0 Runtime installed (can be obtained from the link [Download .NET](https://dotnet.microsoft.com/en-us/download/dotnet)).

2. **Local Configuration**

   > At the current stage of the application, the only local configuration that needs to be done is the connection to the local database.

   - Navigate to the **appsettings.json** file, open it with any text editor, and modify the property **ContextSettings.ConnectionStrings[0].ConnectionString**, filling it with your local database connection credentials:
	```
	{
	   "BaseContextSettings":{
	      "ConnectionStrings":[
	         {
	            "ConnectionStringMySQL":"server=127.0.0.1;port=3306;database=rBRScraper_Development;uid={seu_usuário};password={sua_senha}",
	            "ConnectionStringSQLServer":"Data Source={sua_instância}; Initial Catalog=rBRScraper_Development; Integrated Security=False; User Id={seu_usuário}; Password={sua_senha}; TrustServerCertificate=true",
	            "ContextName":"rBRScraperContext",
	            "DatabaseType":0
	         }
	      ]
	   }
	}
	```
 
3. **Local Execution**

> The instructions provided are valid for:
> - Windows PowerShell;
> - Developer PowerShell for VS 2022 (inside or outside the VS 2022 IDE), and
> - Windows Command Prompt,
>
> and must be executed in the order presented.

- In the chosen command prompt, run the following command to verify that the .NET runtime is available:

`dotnet --info`

The command output should begin with information similar to:
```
.NET SDK:
Version:           9.0.201
Commit:            071aaccdc2
Workload version:  9.0.200-manifests.a3a1a094
MSBuild version:   17.13.13+1c2026462
Runtime Environment:
OS Name:     Windows
OS Version:  10.0.19045
OS Platform: Windows
RID:         win-x64
Base Path:   C:\Program Files\dotnet\sdk\9.0.201\
Host:
Version:      9.0.3
Architecture: x64
Commit:       831d23e561
.NET SDKs installed:
9.0.201 [C:\Program Files\dotnet\sdk]
```
- In the chosen command prompt, navigate to the project root folder where the solution file `*.sln` is located. After navigating, list the directory contents and confirm you are in the correct location before proceeding:

`cd {project_root_directory}`
`dir`

The `dir` command output should look something like:
		Mode                 LastWriteTime         Length Name
		----                 -------------         ------ ----
		d-----          5/6/2025  10:26 AM                rBR.BaseLibraries
		d-----         4/28/2025   1:29 PM                rBR.Scraper.Application
		d-----         4/24/2025   3:20 PM                rBR.Scraper.Data.External
		d-----          5/6/2025  10:59 AM                rBR.Scraper.Data.Internal
		d-----         4/29/2025   4:36 PM                rBR.Scraper.Domain
		d-----         4/28/2025   4:28 PM                rBR.Scraper.Infrastructure
		d-----          5/6/2025  11:52 AM                rBR.Scraper.Service
		-a----          5/2/2025  12:40 PM           5477 rBR.Scraper.sln

- To ensure all necessary dependencies are available, clean the project and restore dependencies by running the following two commands:
```
dotnet clean
dotnet restore
```

- After executing these operations, verify that the application can be built without errors by running:
`dotnet build`

The expected output is:
	Build succeeded.
	0 Warning(s)
	0 Error(s)


- After these steps, the final action is to run the application. Once started, it will be available at the address specified in the **launchSettings.json** file. To start the application, navigate to the folder containing the API service project `{*.Service.csproj}`. In that folder, run the application by passing the project file name as an argument. Assuming you are still in the project root folder, execute:
```
cd src\rBR.Analytics.Service
dotnet run --project rBR.Analytics.Service.csproj
```

Visit the application’s execution address and confirm it is working as expected:
`https://localhost:5002/swagger/index.html`

To stop the application, simply press `Ctrl+C` in the command prompt you are using.
