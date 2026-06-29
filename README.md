# ClientTelegram

## Project Goal

Build a Telegram client capable of:

1. Authenticating the user via phone number
2. Listening to the authorization state, so the user can authenticate using the
   Telegram code provided to them
3. Receiving incoming messages in real time
4. Receiving messages in real time
5. Saving message content encrypted in SQL Server
6. Decrypting message content only when it needs to be displayed
7. Keeping logs only for application state and exceptions, not for message content


## Setup

To get the application running, follow these steps:

1. Database configuration:
   1. First time you should put in appsettings.Development.json your db connection string,
   2. Open the solution , open the console with NuGet package and launch
      ``` bash
         update-database
      ```
   3. After the databse is created run this query in sql server
   ``` sql
      CREATE TABLE NonceCounter
      (
          KeyId       INT          NOT NULL PRIMARY KEY,
          NextValue   BIGINT       NOT NULL
      );
      INSERT INTO NonceCounter (KeyId, NextValue) VALUES (1, 0);
   ```
   this query is very important to generate a correct nonce with multiple restart of application.

2. Cryptography Key:
   1. open the project KeyGenerator and run. you'll see in console Base64 AES-256 key used to encrypt/decrypt message       content. Copy the value.
   2. Paste the value in appsettings.Development.json in MasterKeyBase64

3. Managment Apllication
   
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
    "DatabaseDirectory": "Messages", // folder use by TDLib
    "FilesDirectory": "Files"        // folder where media is stored
  },
  "Log": {
    "PathLog": "Logs"                // system log folder
},
 "ConnectionStrings": {
   "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ClientTelegram;Trusted_Connection=True;TrustServerCertificate=True;" //database conn string
 },
 "Crypto": {
   "ActiveKeyId": 1,
   "MasterKeyBase64" : "insert here the key was generated with the key generator." //key for crypting message
 }
```

The base path is fixed: files are stored under `AppData/Local/ClientTelegram`.

## IMPORTANT
put your real api_id and api_hash in appsettings.Development.json (git-ignored), not in appsettings.json, which is only a template.
