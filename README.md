# Arc4u.Encryptor

This dotnet tool project ease the encryption of string or file based on the Arc4u framework so the cypher text can be decrypted by code using Arc4u.

# Usages

The user can encrypt and decrypt, the option -d is there to inform the operation will be a decryption, default is encryption.  

## 1. Certificate store (windows) or keychain (linux).

The command will use the following default values.
- -c: friendly name of the certificate.
- -n or --name: the default value is "My"
- -s or --store: the defaut value is "Current User".
- -t or --text: the text to encrypt or decrypt.
- -f or --file: path file to encrypt.
- -o or --output: output file path to store the result.  
Text and file cannot be used together!  

arc4u.encryptor -c devCertName -t "clear text"  
Will encrypt the text "clear text" by using the certificate having a friendly name devCertName and the result will be displayed on the terminal window.  

arc4u.encryptor -c devCertName -f "C:\temp\file.txt"
Will encrypt the content of the text in the file C:\temp\file.txt by using the certificate having a friendly name devCertName and the result will be displayed on the terminal window.  

## 2. Using a pfx certificate file name

The command will use the following default values.
- -c: The full path name of the certificate ending by the extension pfx.
- -p or --password: The certificate password.
- -t or --text: the text to encrypt or decrypt.
- -f or --file: path file to encrypt.
- -o or --output: output file path to store the result.  
Text and file cannot be used together!  

## 3. Installing the tool.

### Globally.

dotnet tool install arc4u.encryptor -g --version 1.0.0-preview01

You have to close and restart your terminal window to be able to run the arc4u.encryptor tool.

to uninstall => dotnet tool uninstall arc4u.encryptor -g

### In a specific folder.

Create a folder where you want to store the encryptor tool!  
For example C:\PRJ\Tools  
If the folder is not yet part of the user environment path then run this in your terminal window: setx path "%path%;C:\PRJ\Tools"

Close your terminal and then run this command:

dotnet tool install arc4u.encryptor --tool-path c:\PRJ\Tools --version 1.0.0-preview01

There is no need to close your terminal because the path is already registered!

to uninstall => dotnet tool uninstall arc4u.encryptor --tool-path c:\PRJ\Tools

