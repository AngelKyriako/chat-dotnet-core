# chat-dotnet-core

Playground for server side dotnet core programming.

## Getting Started

```sh 
cd ChatApp.Web
dotnet run
```

Hit the displayed server's url on your browser to be moved to the API's swagger UI.
From there it is possible to test out certain requests.

## Authentication
```
POST /auth/v1/login
```
No parameters are required. The second time you should get the JWT data.

Use the token received in any request that requires it by setting the header:
```
Authorization Bearer somebigggggggggggggggggtoken
```

## Test Authentication
```
GET /auth/v1/user
```
## Test Authorization
```
GET /auth/v1/message
```