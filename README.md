# Relatório Brasil - Sistema Web: rBR Scraper .NET API

![description](https://github.com/relatorio-brasil/rbr-api-scraper/blob/dev/rBR.Scraper/rBR.Scraper.Service/wwwroot/Image/01.png)

**Serviço Web (API) de ferramentas de importação de dados de redes sociais .NET API.**

> Repositório destinado aos serviços .NET de obtenção de dados de redes sociais. Atualmente, tais dados são oriundos do sistema APIFY de aquisição de dados por scrapers.
> **Atualizar este arquivo com instruções importantes à equipe de desenvolvimento.**

# Requisitos técnicos do projeto

* .NET 8.0 LTS;
* Seguir estilo de código proposto pela Microsoft;
* Seguir padrões de arquitetura;
* Manter documentação XML do código.

## Autenticação
A autenticação de cada aplicação será realizada pela associação das informações: **Token de Acesso, Políticas de Autenticação e Autorização e Chaves de Assinatura**.
Para autenticar-se na aplicação rBR Scraper com Políticas de Autenticação e Autorização Configuradas, assim como as Chaves de Assinatura, é necessário possuir o Token de Acesso obtido diretamente do sistema **rBR Auth Manager**.

### Obtenção do Token de Autenticação:
> No presente momento, o sistema de autenticação rBR Auth Manager ainda não está em ambientes de Desenvolvimento, Homologação e/ou Produção. Portanto, deve ser executado localmente para permitir a obtenção do Token de Acesso.

* Local: http://localhost:5001/API/v1.0/User/Account/Login
* Método: POST;
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

## Configurações estáticas de ambientes
Diversas configurações estáticas são necessárias para o funcionamento correto da aplicação. Entre estas, estão: Autenticação e Autorização, Banco de Dados, CORS, Culture e Swagger.

### Configurações de segurança para o token JWT (JWKS)
O arquivo de configuração appsettings.{*environment*}.json deve conter, dentro do objeto *BaseAuthSettings*, o objeto:
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
### Configurações de segurança para as Políticas de Autorização
Visto que o controle completo dos perfis de autorização ainda não está em operação atualmente, as aplicações contam com todos os perfis que já foram apresentados/usados em algum momento. Portanto, o arquivo de configuração appsettings.{*environment*}.json deve conter, dentro do objeto *BaseAuthSettings*, o objeto
```
{
  "AuthorizationPolicySettings": {
    "AuthPolicies": [
      {
        "Claims": [
          {
            "Type": "Admin",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "Application",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "Client",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "DisplayName"
          },
          {
            "Type": "Email"
          },
          {
            "Type": "Status",
            "Values": [
              "1"
            ]
          },
          {
            "Type": "UserName"
          }
        ],
        "Name": "rBR Admin"
      },
      {
        "Claims": [
          {
            "Type": "Admin",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "Application",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "Client",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "DisplayName"
          },
          {
            "Type": "Email"
          },
          {
            "Type": "Status",
            "Values": [
              "1"
            ]
          },
          {
            "Type": "UserName"
          }
        ],
        "Name": "rBR Support"
      },
      {
        "Claims": [
          {
            "Type": "Application",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "Client",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "DisplayName"
          },
          {
            "Type": "Email"
          },
          {
            "Type": "Status",
            "Values": [
              "1"
            ]
          },
          {
            "Type": "UserName"
          }
        ],
        "Name": "rBR Client"
      },
      {
        "Claims": [
          {
            "Type": "Application",
            "Values": [
              "rBR"
            ]
          },
          {
            "Type": "DisplayName"
          },
          {
            "Type": "Email"
          },
          {
            "Type": "Status",
            "Values": [
              "1"
            ]
          },
          {
            "Type": "UserName"
          }
        ],
        "Name": "rBR User"
      }
    ]
  }
}
```
### Configurações de validação para a Autenticação dos tokens JWT
O objecto *BaseAuthSettings* deve conter o objeto:
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
### Configurações de Cross-Origin Resource Sharing - CORS
A configuração de CORS foi montada como provisionamento futuro. Ou seja, não é enforçada mas está disponível. Para que seja disponibilizada em código, o arquivo appsettings.{*environment*}.json deve conter o objeto:
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
### Configurações de bancos de dados
As configurações de bancos de dados ainda são locais, visto que os ambientes de Desenvolvimento, Homologação e/ou Produção ainda não estão disponíveis. A configuração suporta bancos MySQL e MSSQL Server. Para aplicá-las, o campo *DatabaseType* pode ser alterado de 0 (MySQL) para 1 (MSSQL). O arquivo appsettings.{*environment*}.json deve conter o objeto:
```
{
	"BaseContextSettings": {
		"ConnectionStrings": [
			{
		      "ConnectionStringMySQL": "server=127.0.0.1;port=3306;database=rBRScraper_Development;uid={seu_usuário};password={sua_senha}",
		      "ConnectionStringSQLServer": "Data Source={sua_instância}; Initial Catalog=rBRScraper_Development; Integrated Security=False; User Id={seu_usuário}; Password={sua_senha}; TrustServerCertificate=true",
		      "ContextName": "rBRScraperContext",
		      "DatabaseType": 0
	    }
	  ]
	}
}
```
### Configurações de cultura
As configurações de cultura estabelecem quais as culturas suportadas pelos controladores da API. O arquivo appsettings.{*environment*}.json deve conter o objeto:
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
### Configurações de Swagger
O arquivo appsettings.{*environment*}.json deve conter o objeto:
```
{
	"BaseSwaggerSettings": {
		"ApiExplorer": {
			"GroupNameFormat": "V.v",
			"SubstitutionFormat": "V.v"
		},
		"Description": "<br>![rBR Scraper - Logo](/Image/01.png)",
		"DocumentName": "rBR Scraper - Web API",
		"GenerateExamples": true,
		"GenerateXmlObjects": true,
		"OpenApiSecurityScheme": {
			"Description": "JWT authorization header using the Bearer schema. Type 'Bearer' [space] and then your token in the text input below. Example: Bearer {token}",
			"Name": "Authorization"
		},
		"Title": "rBR Scraper - Development",
		"UseControllerSummaryAsTagDescription": true,
		"UseRouteNameAsOperationId": false,
		"UseXmlDocumentation": true
	}
}
```
### Configurações de conexão com o sistema APIFY:
O arquivo appsettings.{*environment*}.json deve conter o objeto:
```
{
	"ApiFySettings": {
		"AccessToken": "apify_api_NEF0KDgXWsMDq6O6CMs3FzMurUhVij1NhxMR",
		"UrlBase": "https://api.apify.com/v2",
		"EndpointListActors": "/acts",
		"EndpointGetActor": "/acts/{0}",
		"EndpointListRuns": "/acts/{0}/runs",
		"EndpointGetDatasetItems": "/datasets/{0}/items?format=json&skipEmpty=true&skipHidden=true"
	}
}
```
### Configurações de Scrapers APIFY ativos:
```
{
	"ScraperSettings": {
		"ActiveConfiguredScrapers": [
			"shu8hvrXbJbY3Eb9W",
			"dSCLg0C3YEZ83HzYX"
		]
	}
}
```

## Funcionalidades

1. **Admin - Instagram Datasets**:
> * <u>GET - /API/v1.0/Admin/InstagramDataset</u>: _listagem de Instagram Scraper Datasets importados;_
> * <u>POST - /API/v1.0/Admin/InstagramDataset/{datasetDataId}</u>: _importação de um único Instagram Scraper Dataset;_
> * <u>POST - /API/v1.0/Admin/InstagramDataset/ImportAll/{scraperId}</u>: _importação de todos os Datasets de um único Scraper;_
> * <u>GET - /API/v1.0/Admin/InstagramDataset/ImportAll/{scraperId}</u>: _listagem de Datasets por Run._

2. **Admin - Instagram Profile Datasets**:
> * <u>GET - /API/v1.0/Admin/InstagramProfileDataset</u>: _listagem de Instagram Profile Scraper Datasets importados;_
> * <u>POST -/API/v1.0/Admin/InstagramProfileDataset/{datasetDataId}</u>: _importação de um único Instagram Profile Scraper Dataset;_
> * <u>POST - /API/v1.0/Admin/InstagramProfileDataset/ImportAll/{scraperId}</u>: _importação de todos os Datasets de um único Scraper;_
> * <u>GET - /API/v1.0/Admin/InstagramProfileDataset/ListByRun</u>: _listagem de Datasets por Run._

3. **Admin - Runs**:
> * <u>POST - /API/v1.0/Admin/Run</u>: _criação de uma lista de Scraper Runs_;
> * <u>GET - /API/v1.0/Admin/Run</u>: _listagem de Scraper Runs já criadas/importadas_;
> * <u>GET - /API/v1.0/Admin/Run/{id}</u>: _obtenção de uma única Run por Id_;
> * <u>GET - /API/v1.0/Admin/Run/ListFromApiFy</u>: _listagem de Runs do APIFY, filtradas por Scraper_.

4. **Admin - Scrapers**:
> * <u>POST - /API/v1.0/Admin/Scraper</u>: _criação de uma lista de Scrapers_;
> * <u>GET - /API/v1.0/Admin/Scraper</u>: _listagem de Scrapers já criados/importados_;
> * <u>GET - /API/v1.0/Admin/Scraper/{id}</u>: _obtenção de um único Scraper por Id_;
> * <u>GET - /API/v1.0/Admin/Scraper/GetFromApiFy/{id}</u>: _obtenção de um único Scraper do APIFY_;
> * <u>GET - /API/v1.0/Admin/Scraper/ListFromApiFy</u>: _listagem de Scrapers do APIFY_.

## Estrutura de dados - MySQL Maria DB

1.	**CommonScrapers**

    |Nome|Anulável|Tipo|Precisão|Chave|
    |--|--|--|--|--|
    |Id|	Não|	varchar(36)|	-|	PRI|
    |Created|	Não|	varchar(33)|	-|	-|
    |Modified|	Sim|	varchar(33)|	-|	-|
    |Removed|	Sim|	varchar(33)|	-|	-|
    |Status|	Não|	int(11)|	10|	-|
    |DataId|	Não|	varchar(50)|	-|	UNI|
    |Title|	Não|	varchar(100)|	-|	-|
    |Description|	Não|	text|	-|	-|
    |Name|	Não|	varchar(100)|	-|	-|
    |CreatedAt|	Não|	varchar(33)|	-|	-|
    |ModifiedAt|	Sim|	varchar(33)|	-|	-|

---

2. **InstagramProfileScraperDatasets**

    |Nome|Anulável|Tipo|Precisão|Chave|
    |--|--|--|--|--|
    |Id|	Não|	varchar(36)|	-|	PRI|
    |Created|	Não|	varchar(33)|	-|	-|
    |Modified|	Sim|	varchar(33)|	-|	-|
    |Removed|	Sim|	varchar(33)|	-|	-|
    |Status|	Não|	int(11)|	10|	-|
    |DataId|	Não|	varchar(50)|	-|	-|
    |Url|	Não|	varchar(300)|	-|	-|
    |InputUrl|	Não|	varchar(300)|	-|	-|
    |UserName|	Não|	varchar(150)|	-|	MUL|
    |FullName|	Não|	varchar(150)|	-|	-|
    |FollowersCount|	Sim|	int(11)|	10|	-|
    |Verified|	Sim|	bit(1)|	1|	-|
    |Timestamp|	Não|	varchar(33)|	-|	-|
    |RunId|	Não|	varchar(36)|	-|	MUL|
    |FullObject|	Não|	longtext|	-|	-|
    ```mermaid
    graph LR
    A(InstagramProfileScraperDatasets) === FK_RunId  ==> B(ScrapersRuns)
    ```
---

3. **InstagramScraperDatasets**

    |Nome|Anulável|Tipo|Precisão|Chave|
    |--|--|--|--|--|
    |Id|	Não|	varchar(36)|	-|	PRI|
    |Created|	Não|	varchar(33)|	-|	-|
    |Modified|	Sim|	varchar(33)|	-|	-|
    |Removed|	Sim|	varchar(33)|	-|	-|
    |Status|	Não|	int(11)|	10|	-|
    |DataId|	Não|	varchar(50)|	-|	-|
    |Url|	Não|	varchar(300)|	-|	-|
    |InputUrl|	Não|	varchar(300)|	-|	-|
    |Timestamp|	Não|	varchar(33)|	-|	-|
    |RunId|	Não|	varchar(36)|	-|	MUL|
    |InstagramProfileId|	Não|	varchar(36)|	-|	MUL|
    |FullObject|	Não|	longtext|	-|	-|
    ```mermaid
    graph LR
    A(InstagramScraperDatasets) === FK_RunId  ==> B(ScrapersRuns)
    A(InstagramScraperDatasets) === FK_InstagramProfileId  ==> C(InstagramProfileScraperDatasets)
    ```
---

4. **ScrapersRuns**

    |Nome|Anulável|Tipo|Precisão|Chave|
    |--|--|--|--|--|
    |Id|	Não|	varchar(36)|	-|	PRI|
    |Created|	Não|	varchar(33)|	-|	-|
    |Modified|	Sim|	varchar(33)|	-|	-|
    |Removed|	Sim|	varchar(33)|	-|	-|
    |Status|	Não|	int(11)|	10|	-|
    |DataId|	Não|	varchar(50)|	-|	MUL|
    |DataStatus|	Não|	varchar(50)|	-|	-|
    |DatasetId|	Não|	varchar(50)|	-|	-|
    |StartedAt|	Não|	varchar(33)|	-|	-|
    |FinishedAt|	Não|	varchar(33)|	-|	-|
    |Imported|	Não|	bit(1)|	1|	-|
    |ImportingError|	Sim|	text|	-|	-|
    |ScraperId|	Não|	varchar(36)|	-|	MUL|
    ```mermaid
    graph LR
    A(ScrapersRuns) === FK_ScraperId  ==> B(CommonScrapers)
    ```	
	

## Ambiente local e execução

 1. **Requisitos**

 - Banco de dados - My SQL Maria DB v. 10.5.9 (pode ser obtido no link [Download Maria DB](https://mariadb.org/download/?t=mariadb&p=mariadb&r=11.3.0)), e
 - Runtime do .NET Core 8.0 instalado (pode ser obtido no link [Download do .NET](https://dotnet.microsoft.com/en-us/download/dotnet)).

 2. **Configurações locais**

	> No presente momento da aplicação, a única configuração local a ser
	> feita é a de conexão com o banco de dados local.
	
 - Navegue até o arquivo **appsettings.json**, abra-o com algum editor de texto e altere a propriedade **ContextSettings.ConnectionStrings[0].ConnectionString**, preenchendo seu valor com as suas credenciais locais de conexão ao seu banco de dados:
	```
	{
		"BaseContextSettings": {
			"ConnectionStrings": [
				{
				"ConnectionStringMySQL": "server=127.0.0.1;port=3306;database=rBRScraper_Development;uid={seu_usuário};password={sua_senha}",
				"ConnectionStringSQLServer": "Data Source={sua_instância}; Initial Catalog=rBRScraper_Development; Integrated Security=False; User Id={seu_usuário}; Password={sua_senha}; TrustServerCertificate=true",
				"ContextName": "rBRScraperContext",
				"DatabaseType": 0
				}
			]
		}
	}
	```
 3. **Execução local**

	>As instruções dadas são válidas para:
	> - Windows PowerShell;
	> - Developer PowerShell  for VS 2022 (dentro ou fora da IDE do VS 2022), e
	> - Windows Command Prompt,
	> 
	>e devem ser executas na ordem dada.

- Dentro do prompt de comando escolhido, execute o comando seguinte para verificar se o runtime do .NET está disponível:

		dotnet --info
		
	O resultado do comando deve iniciar com informações similares a:

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
  
- Dentro do prompt de comando escolhido, navegue até a pasta raiz do projeto, onde se localiza o arquivo de solução do mesmo `*.sln`. Após navegar, liste os itens presentes no diretório e verifique se está no local correto antes de continuar:

		cd {diretório raiz do projeto}
		dir
	
	O resultado da listagem (comando: `dir`) deverá exibir algo como:

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

- Para garantir que todas as dependências necessárias estão disponíveis, limpe o projeto e restaure as dependências, executando os 02 comandos seguintes:

	    dotnet clean
	    dotnet restore

- Após a execução destas operações, bastará certificar-se de que a aplicação está possível de ser compilada sem erros. Para isso, execute o comando:

		dotnet build

	O resultado deve exibir:

		Build succeeded.
			0 Warning(s)
			0 Error(s)

- Após a execução destas operações, restará executar a aplicação. Após o início da execução, o mesmo já estará disponível no endereço determinado no arquivo **launchSettings.json**. Para iniciar a aplicação, navegue até a pasta que contém o projeto de serviço de API `{*.Service.csproj}`. Nesta pasta, execute a aplicação passando o nome do arquivo de projeto como parâmetro/argumento. Considerando que ainda "estamos" na pasta raiz do projeto, siga os comandos:

		cd src\rBR.Analytics.Service
		dotnet run --project rBR.Analytics.Service.csproj
		
	Visite o endereço de execução da aplicação e certifique-se de que a mesma funciona como esperado:
	https://localhost:5002/swagger/index.html
	para encerrar a execução da aplicação, basta executar o comando `Ctrl+C` no prompt de comando usado.
