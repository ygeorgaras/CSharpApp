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

To resolve this issue, on startup we instatiate the HttpClient during startup and use a dependency injection into the HttpClientWrapper to ensure that the correct HttpClient is being used.

**#2**

Τhere is one or more problems/bugs that exist! It would be great if you catch them and attempt to fix.

1. GetTodoById lets you enter an invalid ID and throws a 404 not found exception. Now when we issue a Get and it fails we will log the exception and endpoint. We will then return a Status404NotFound.
2. Fixed bug where you can issue a BadHttpRequest by entering values greater than Int32.MaxVal. Now we setup the max value you can enter to be int.MaxValue to prevent this error.

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
- [x] Get
- [x] Post
- [x] Put
- [x] Delete

Added all of the HttpClientWrapper functionality to both PostService and TodoService through a dependency injection.
