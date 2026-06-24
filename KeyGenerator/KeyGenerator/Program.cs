// See https://aka.ms/new-console-template for more information
// Execute this for the first time, after save in the secret.
byte[] key = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
string base64Key = Convert.ToBase64String(key);
Console.WriteLine(base64Key); // copy this and put in the secret / system environment 
