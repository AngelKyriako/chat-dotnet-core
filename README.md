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

##### Register & login
```sh
curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' -d '{
"username": "yolo",
"password": "yolo"
}' 'http://localhost:5000/api/v1/user'
```
```json
{
  "user": {
    "username": "string",
    "password": "string",
    "firstname": "string",
    "lastname": "string",
    "id": "string",
    "enabled": true,
    "createdAt": "2017-03-17T09:37:49.339Z",
    "updatedAt": "2017-03-17T09:37:49.339Z"
  },
  "token": {
    "accessToken": "string",
    "expiresInSeconds": 0
  }
}
```

##### Login
```sh
curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' -d '{
"username": "yolo",
"password": "yolo"
}' 'http://localhost:5000/api/v1/user/login'
```
```json
{
  "user": {
    "username": "string",
    "password": "string",
    "firstname": "string",
    "lastname": "string",
    "id": "string",
    "enabled": true,
    "createdAt": "2017-03-17T09:37:49.339Z",
    "updatedAt": "2017-03-17T09:37:49.339Z"
  },
  "token": {
    "accessToken": "string",
    "expiresInSeconds": 0
  }
}
```

##### Test Authentication

```sh
curl -X GET --header 'Accept: application/json' --header 'Authorization: Bearer accessTokenReceived' 'http://localhost:5000/api/v1/user'
```

##### Test Authorization (TODO: FIX [Authorize("somePolicy")]

```sh
curl -X GET --header 'Accept: application/json' --header 'Authorization: Bearer accessTokenReceived' 'http://localhost:5000/api/v1/message'
```