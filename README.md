# Arc4u.Cyphertool

The Arc4u.Standard library provides a set of tools to encrypt and decrypt strings.
The encryption is based on the X509Certificate2 implementation of .NET.
The Arc4u library is using the RSA algorithm to encrypt and decrypt the content.

From version 8.2.0, when the content is too long, the framework is encrypting via the Aes encryption symetric key.
The initialization vector and key are encrypted by the certifcate.

The framework contains also a feature Arc4u.Standard.Configuration.Decryptor to decrypt content in a configuration section of an application.

The Arc4u.Cyphertool is a dotnet tool working around the certifiate and is able to perform 3 actions.

1) Encrypt a string or a file.
2) Decrypt the cypher text.
3) Extract certificate informations.



## 1) Encryption

```console
                               / pfx "C:\temp\devCert.pfx" -p password \ / text "clear text" \
arc4u.cyphertool encrypt with |                                         |                     | -o "file"
                               \ cert "devCert" -l LocalMachine -n My  / \ file "path"       /

```

Complete documentation, execute

arc4u.cyphertool encrypt --help

## 2) Decryption

```console

                               / pfx "C:\temp\devCert.pfx" -p password \ / text "clear text" \
arc4u.cyphertool decrypt with |                                         |                     | -o "file"
                               \ cert "devCert" -l LocalMachine -n My  / \ file "path"       /


```
Complete documentation, execute

arc4u.cyphertool decrypt --help

## 3) Extract

```console

                                                                               |                / pfx "C:\temp\devCert.pfx" -p password 
arc4u.cyphertool extract pfx "C:\temp\devCert.pfx" -p password -ca -f "folder" | encrypt with  |                                         
                                                                               |                \ cert "devCert" -l LocalMachine -n My  
                                                                                       
                                                                               => optional

```

Read a certificate from a pfx file and extract the public, private and certificate authorities.

The reason for this is to avoid the openssl that doesn't exist natively on Windows but also to extract the certificate authorities.

This is useful when you want to use the certificate in dapr for some components like RabbitMQ for example
where you have to extract the public, the private key but also the certificate authorities.


```yaml
  - name: saslExternal
    value: true
  - name: clientCert
    value: |-
      -----BEGIN PUBLIC KEY-----
      [...]
      -----END PUBLIC KEY-----
  - name: clientKey
    value: |-
      -----BEGIN PRIVATE KEY-----
      [...]
      -----END PRIVATE KEY-----
  - name: caCert
    value: |-
      -----BEGIN PUBLIC KEY-----
      [...]
      -----END PUBLIC KEY-----
```

### Openssl commands

Equivalent openssl commands to extract the public, private key from a pfx file.
#### Extract the public key

openssl pkcs12 -in devCert.pfx -clcerts -nokeys -out devCert.crt.pem


#### Extract the private key

openssl pkcs12 -in devCert.pfx -nocerts -out devCert.key.pem

# Installing the tool.

## Globally.
dotnet tool install arc4u.cyphertool -g --version 1.0.0-preview01

You have to close and restart your terminal window to be able to run the Arc4u.Cyphertool tool.

to uninstall => dotnet tool uninstall Arc4u.Cyphertool -g

## In a specific folder.
Create a folder where you want to store the encryptor tool!  
For example C:\PRJ\Tools
If the folder is not yet part of the user environment path then run this in your terminal window:

setx path "%path%;C:\PRJ\Tools"

Close your terminal and then run this command:

dotnet tool install arc4u.cyphertool --tool-path c:\PRJ\Tools --version 1.0.0-preview01

There is no need to close your terminal because the path is already registered!

to uninstall => dotnet tool uninstall Arc4u.Cyphertool --tool-path c:\PRJ\Tools