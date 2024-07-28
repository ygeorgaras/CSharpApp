# CSharpApp

An application for C# knowledge assessment.

# Description

This is a web application that interacts with a 3nd party service (https://jsonplaceholder.typicode.com) and serves data

The application has some problems described below that need to be addressed plus some new features that need implementation.

## Problems

**#1**

Seems that there is a problem in the current implementation of HttpClient that not handles the requests properly.

Resolution:
The issue here is that we are setting the BaseAddress on each call to a method in the service. You can only modify the HttpClient properties before you send the first request. This is why it works the first time but fails on the next call.

To resolve this issue, you must set the BaseAddress when the HttpClient is instantiated during the TodoService constructor.

**#2**

Τhere is one or more problems/bugs that exist! It would be great if you catch them and attempt to fix.

1. GetTodoById lets you enter an invalid ID and throws a 404 not found exception. To get around this we check if the reponse if successful, otherwise we return a null record.
2. Now we will return a null record if anything is returned other than a successful response.

## New features

**#1**

Create a new service that handles the "Posts" endpoint (/posts)

The following must be supported:

- [x] Get all
- [x] Get by id
- [x] Add new post
- [x] Delete post

**#2**

Create a generic http client wrapper handling all types of requests.

The following verbs must be supported:
- Get
- Post
- Put
- Delete
