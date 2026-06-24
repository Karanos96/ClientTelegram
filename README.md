# ClientTelegram

## Project Goal

Build a Telegram client capable of:

1. Authenticating the user via phone number
2. Listening to the authorization state, so the user can authenticate using the
   Telegram code provided to them
3. Receiving incoming messages in real time

## Setup

To get the application running, follow these steps:

1. Log in at https://my.telegram.org/auth?to=apps
2. Create an application and add the `api_id` and `api_hash` to `appsettings.json`.
   This is what lets you authenticate through the app.
3. Once the project is running, call the `api/Session/Register` endpoint with the
   POST method, sending this object:

```json
   {
       "Phonenumber": "+39xxxxxxxxxx"
   }
```
   You will recive an object with id and phonenumber:
```json
   {
       "id": 1,
       "Phonenumber": "+39xxxxxxxxxx"
   }
```
4. Now you must required the telegram code, call the  `/api/Auth/Phonenumber` endpoint with the
   POST method, sending this object:
```json
   {
    "Phonenumber": "+39xxxxxxxxxx",
    "AccessCode": "",
    "SessionId":1
   }
```
You will recive the access code from Telegram.
5. To complete authentication, call the `api/Auth/AccessCode` endpoint with the
   POST method, sending the following object:

```json
   {
      "SessionId": 1,
      "Phonenumber": "+39xxxxxxxxxx",
      "AccessCode": "your authentication code"
   }
```

   You will now be authenticated.

## Additional Configuration

`appsettings.json` contains some custom configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Telegram": {
    "ApiId": 0,                      // Telegram api_id
    "ApiHash": "hash_code",          // Telegram api_hash
    "DatabaseDirectory": "Messages", // folder where messages are stored
    "FilesDirectory": "Files"        // folder where media is stored
  },
  "Log": {
    "PathLog": "Logs"                // system log folder
  }
}
```

The base path is fixed: files are stored under `AppData/Local/ClientTelegram`.

## IMPORTANT
put your real api_id and api_hash in appsettings.Development.json (git-ignored), not in appsettings.json, which is only a template.
