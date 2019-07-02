# TestOkur

![alt text](high-level-diagram.png "TestOkur High Level Diagram")

## Setting up the environment

Follow this steps to setup your development environment.

1. Install Docker Latest Version
2. Install Visual Studio 2017 (Preferably the Latest Version)
3. Install .NET Core 2.2 (Both SDK and Runtime)
4. Open Solution
5. Open terminal at the root of the folder  and run 'docker-compose up redis rabbitmq postgres' command

## How to run integration tests
1. Install Docker Latest Version
2. Open terminal at the root of the folder  and run 'docker-compose down'
3. ###### To run in docker container
* Run 'docker-compose up --build test'
3. ###### To run by using Visual Studio test explorer
* Open terminal at the root of the folder  and run 'docker-compose up redis rabbitmq postgres' command
* Run tests by using test explorer window of Visual Studio 2017

## How to Add Db Migration

Run  'Add-Migration {MIGRATION_NAME} -Context ApplicationDbContext' command on package manager console

## How to Generate New X509 Certificate for Identity Server

1. Run PowerShell in administrator mode
2. Execute 'New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname "testokur"'
3. $pwd = ConvertTo-SecureString -String "{YOUR_STRONG_PASSWORD}" -Force -AsPlainText
4. Export-PfxCertificate -cert cert:\localMachine\my\09EA2AF8E646F5DB24774C0BE9EF332C1874AA90  -FilePath testokur.pfx -Password $pwd