# AWS Lambda with CQRS for Microservice architectures

This repository provides a basic CQRS scaffold for building Microservice architectures using Lambdas, Serverless Functions, within Amazon Web Services (AWS).

Everything in this example can be deployed into your own AWS account by installing the AWS Lambda tooling via the dotnet core CLI. The tooling can be installed with the following command:

> dotnet tool install -g Amazon.Lambda.Tools

Once installed, create an S3 bucket in your account for the deployment to land in. Note the name of the bucket. Once you've done that, you can deploy the example project by running the following command within the `examples/Todo.Projects` folder.

> dotnet lambda deploy-serverless

When prompted you need to enter the deployment S3 bucket you created above. The deployment process will take a few seconds but when it is completed you will be given a URL. For example:

> https://ofd78c0.execute-api.us-east-1.amazonaws.com/Prod/

The following routes are usable

### Commands:
- HTTP POST: `/projects`
  - The following JSON body can be used when posting: `{ "Title": "Hello World", "Type": "List", "Status": "Active", "Priority": "Medium", "PercentageCompleted": "50" }`
  - The route to query the created record will be included in the `Location` header from the HTTP Response.
  - All records have a hard-coded username of `janedoe` for the examples. In the future, authorization mechanics will be added for fetching user information.
- HTTP DELETE: `/projects/{projectId}`
  - Deletes a Project providing that it can find a matching ProjectId for `janedoe`.

### Queries
- HTTP GET: `/projects`
  - This will return back all projects for the user `janedoe`.
- HTTP GET: `/projects/{projectId}`
  - This will return back any project for `janedoe` that has a matching ProjectId
