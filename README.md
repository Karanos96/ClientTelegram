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
3. Once the project is running, call the `api/Auth/Phonenumber` endpoint with the
   POST method, sending this object:

```json
   {
       "Phonenumber": "+39xxxxxxxxxx",
       "AccessCode": ""
   }
```

   You will receive an authentication code from Telegram.
4. To complete authentication, call the `api/Auth/AccessCode` endpoint with the
   POST method, sending the following object:

```json
   {
       "Phonenumber": "",
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
