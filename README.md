# ChatApp .NET Core

### Playground for server side dotnet core programming.

## Prerequisites

- [dotnet core](https://www.microsoft.com/net/core)
- Add dotnet core to system path. (```C:\Program Files\dotnet```) 

## Getting Started

### Run the server

```sh 
cd ChatApp.Web
dotnet.exe run
```

*Hit the displayed server's url on your browser to be moved to the API's swagger UI.
From there it is possible to test out certain requests.*

### Run the console demo

```sh 
cd ChatApp.ConsoleDemo
dotnet.exe run
```

## Architecture

The project is setup with a layer based architecture.

The main idea is to keep a core layer with all the logic of the app, so that it can be shared between the server and client. Both the server and the client will depend on the core layer and can be build on it independently.

### Common (Class Library)

Contains common utilities among all the projects.

| Module | Description |
|:-------|:------------|
| JsonSerializer | A wrapper on top of newtonsoft.json JsonConvert that is configured with the same serialization settings with dotnetcore MVC. It is needed for serializing the model's of the application on the client side and the server side for the web socket controllers which are custom made. |

### Model (Class Library)

Includes all the models that are part of the application, be it database entities or helper classes. 

This module is going to be used by both the server and the client. Therefore, it should be setup to be database agnostic. Mo database dependencies are found on this module.

### Repository (Class Library)

Dependencies:
- Model

Includes anything related to persistence. From database to plain file storage.

At the moment it is setup to support:
- in memory DB
- MSSQL server

*TODO: MongoDB support*

Database(non business logic) related rules & constraints regarding the models should be defined here.

### Auth (Class Library)

Dependencies:
- Model
- Repository

Includes anything authentication related.

At the moment it supports authentication/authorization based on Json Web Token.

### Service (Class Library)

Dependencies:
- Model
- Repository
- Auth

This module includes the high level API of the application.

This is the transport protocol agnostic interface, meaning it is just a bunch of classes that can be used on any context. No HTTP, websocket, TCP or any protocol whatsoever should be found on this layer.

When a part of the service layer can be abstracted out as a seperate module, it is encouraged to do so. 

For example the Auth module is a service module but lives in its own project. This could be the case for other useful services like matchmaking or billing.

### WS (Middleware)

Dependencies:
- Common

A Middleware implementation to abstract out the low level implementation of a web socket server.

It introduces too classes:
- WSController: Includes methods for listening & emitting web socket events with string payload.
- WSControllerGeneric: Includes methods for listening & emitting web socket events of a generic type. It is setup with JSON serialization by default, but this can be overriden.

This module should be independent of the application code.

*TODO: Find a way to bind the WSConnection with a custom type (IWSConnection, WSConnection<T>).*
*TODO: Add to nuget and remove from project.*

### Web (MVC)

Dependencies:
- Model
- Service
- WS

Includes the web interface of the application.

This module should delegate method calls from the Service layer based on HTTP requests or web socket messages sent from a client.

Its a dummy module and NO logic should be found on this module.

### Client (Class Library)

Dependencies:
- Common
- Model

Includes anything that is needed for an client application to communicate with the server.

TODO: Unity support

### Console Demo (Console App)

Dependencies:
- Common
- Model
- Client
- WebSocket4Net

Includes a demo console chat application that uses the server api.

*TODO: Abstract out WebSocket4Net in the client class library*

### Dependencies Overview

|            | Common | Model | Repository | Auth | Service | Web | WS | Client | ConsoleDemo |
|:-----------|:------:|:-----:|:----------:|:----:|:-------:|:---:|:--:|:------:|:-----------:|
| Common     |   -    |       |            |      |         |     |    |        |             |
| Model      |   *    |   -   |            |      |         |     |    |        |             |
| Repository |   *    |   O   |     -      |      |         |     |    |        |             |
| Auth       |   *    |       |     O      |  -   |         |     |    |        |             |
| Service    |   *    |       |     O      |  O   |    -    |     |    |        |             |
| Web        |   *    |       |            |  O   |    O    |  -  | O  |        |             |
| WS         |   O    |       |            |      |         |     | -  |        |             |
| Client     |   O    |   O   |            |      |         |     |    |   -    |             |
| ConsoleDemo|   O    |   O   |            |      |         |     |    |   O    |     -       |

*Does bot depend at the moment, but could be if the module is further expanded.

### Layers Overview

|   Layer    | Common | Model  | Repository | Auth | Service | Web |
|:-----------|:------:|:------:|:----------:|:----:|:-------:|:---:|
| Core       |   O    |   O    |            |      |         |     |
| Repository |        |        |     O      |      |         |     |
| Service    |        |        |            |  O   |   O     |     |
| Interface  |        |        |            |      |         |  O  |

### To be Continued

1. Abstract out WebSocket4Net from the ConsoleDemo into the ChatApp.Client library.
2. Update the Client module or create a new one that supports Unity (.net3.5)
3. Authorize the web socket listeners #1
4. Bind WSConnection with the user model #1
5. Add WS Middleware in nuget and remove from solution
6. MongoDB repositories with the same model layer.

**Link References can be found in the code, in important class header comments.**