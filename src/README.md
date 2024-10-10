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
arc4u.cyphertool extract pfx "C:\temp\devCert.pfx" -p password -ca -f "folder"  | encrypt with  |                                         
                                                                                |                \ cert "devCert" -l LocalMachine -n My  
                                                                                       
                                                                                  => optional

```

Read a certificate from a pfx file or from the computer and extract the public private and certificate authorities.

