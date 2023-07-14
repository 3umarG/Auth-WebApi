# Auth Web Api with JWT

This repository contains a demo .NET API project that demonstrates the usage of JSON Web Tokens (JWT), Identity framework with EF Core, and various authentication and authorization techniques. The project also incorporates the usage of refresh tokens to enhance security and extend the lifetime of JWT access tokens.

## Table of Contents

- [Introduction](#introduction)
- [Technologies Used](#technologies-used)
- [Authentication and Authorization](#authentication-and-authorization)
- [JSON Web Tokens (JWT)](#json-web-tokens-jwt)
- [Identity Framework with EF Core](#identity-framework-with-ef-core)
- [Authorization Based on Roles and Policies](#authorization-based-on-roles-and-policies)
- [Refresh Tokens](#refresh-tokens)

## Introduction

The demo .NET API project serves as a learning resource and example implementation for building secure APIs using various authentication and authorization techniques. It showcases the usage of JSON Web Tokens (JWT) for authentication, Identity framework with EF Core for managing user authentication and authorization, and the usage of refresh tokens to enhance security and provide extended access token lifetimes.

## Technologies Used

The following technologies and frameworks are utilized in this project:

- .NET: A free, cross-platform, open-source framework for building modern applications.
- ASP.NET Core: A web framework for building cloud-based, internet-connected applications.
- Entity Framework Core (EF Core): An object-relational mapper (ORM) that simplifies database access in .NET applications.
- JSON Web Tokens (JWT): A compact, URL-safe means of representing claims to be transferred between two parties.
- Identity Framework: A framework for managing user authentication, authorization, and user accounts.
- Swagger: A tool for designing, building, documenting, and consuming RESTful web services.

## Authentication and Authorization

Authentication and authorization are crucial aspects of building secure APIs. This project demonstrates how to implement these concepts using industry-standard techniques and best practices.

### JSON Web Tokens (JWT)

JSON Web Tokens (JWT) are used for authentication in this project. JWT is a compact, self-contained token that can securely transmit information between parties as a JSON object. It is digitally signed, allowing the recipient to verify its authenticity and integrity.

JWTs consist of three parts: the header, the payload, and the signature. The header contains metadata about the token, such as the signing algorithm used. The payload contains claims, which are statements about the user and additional data. The signature is created by combining the encoded header, payload, and a secret key, ensuring the token's integrity.

### Identity Framework with EF Core

Identity Framework with EF Core is used for managing user authentication and authorization in this project. Identity Framework simplifies the implementation of user accounts, roles, and permissions in ASP.NET Core applications.

EF Core is an ORM that enables developers to work with databases using .NET objects. It provides a convenient way to perform database operations and seamlessly integrates with Identity Framework to store user-related data, such as user credentials, roles, and claims.

### Authorization Based on Roles and Policies

This project demonstrates how to implement authorization based on roles and policies. Roles define a set of permissions or access levels, while policies define the rules for granting access based on various criteria.

By utilizing roles and policies, you can ensure that only authorized users with specific roles or meeting certain criteria can access certain API endpoints or perform specific actions.

## Refresh Tokens

To enhance security and extend the lifetime of JWT access tokens, this project incorporates the usage of refresh tokens. Refresh tokens are long-lived tokens that are used to obtain new access tokens when they expire.

When a user authenticates and receives an access token, a refresh token is also generated and associated with the user. When the access token expires, the user can use the refresh token to obtain a new access token without re-authenticating. This technique reduces the active lifetime of access tokens, limiting their exposure and improving security.
