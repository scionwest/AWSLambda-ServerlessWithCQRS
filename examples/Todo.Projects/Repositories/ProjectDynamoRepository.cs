using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Projects.Domain;

namespace Todo.Projects.Commands
{
    public class ProjectDynamoRepository : IProjectRepository
    {
        private readonly IAmazonDynamoDB dynamoDB;
        private readonly string projectsTableName;

        public ProjectDynamoRepository(IAmazonDynamoDB dynamoDB, IConfiguration configuration)
        {
            this.dynamoDB = dynamoDB;
            this.projectsTableName = configuration["AWS:DynamoDb:ProjectsTable:TableName"];
        }

        public async Task CreateProjectAsync(string username, Project newProject)
        {
            Table projectsTable = Table.LoadTable(this.dynamoDB, this.projectsTableName);
            var condition = new Expression
            {
                ExpressionStatement = $"attribute_not_exists({nameof(Project.Id)})",
            };

            var putOperation = new PutItemOperationConfig { ConditionalExpression = condition, };
            var project = new Document();

            project[nameof(Project.Id)] = newProject.Id;
            project[nameof(Project.OwningUser)] = newProject.OwningUser;
            project[nameof(Project.CompletionDate)] = newProject.CompletionDate;
            project[nameof(Project.IsArchived)] = newProject.IsArchived;
            project[nameof(Project.IsFlagged)] = newProject.IsFlagged;
            project[nameof(Project.PercentageCompleted)] = newProject.PercentageCompleted;
            project[nameof(Project.Priority)] = newProject.Priority.ToString();
            project[nameof(Project.StartDate)] = newProject.StartDate;
            project[nameof(Project.TargetDate)] = newProject.TargetDate;
            project[nameof(Project.Title)] = newProject.Title;

            DateTime auditDate = DateTime.UtcNow;
            project[nameof(Project.CreateDate)] = auditDate;
            project[nameof(Project.UpdateDate)] = auditDate;

            await projectsTable.PutItemAsync(project, putOperation);
        }

        public Task DeleteProjectAsync(string username, string projectId)
        {
            // Query for all Project records owned by this user.
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = this.projectsTableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = projectId } },
                    { "OwningUser", new AttributeValue {S = username } },
                }
            };

            return this.dynamoDB.DeleteItemAsync(deleteItemRequest);
        }

        public async Task<Project> GetProjectByIdForUserAsync(string username, string projectId)
        {
            // Query for all Project records owned by this user.
            var getRequest = new GetItemRequest
            {
                TableName = this.projectsTableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = projectId } },
                    { "OwningUser", new AttributeValue {S = username } },
                }
            };

            GetItemResponse results = await this.dynamoDB.GetItemAsync(getRequest);

            // Convert the NoSql document into a domain model.
            Dictionary<string, AttributeValue> document = results.Item;
            Guid id = Guid.Parse(document["Id"].S);
            var project = new Project(id, document["OwningUser"].S, document["Priority"].S, document["Title"].S);

            return project;
        }

        public async Task<Project[]> GetProjectsForUserAsync(string username)
        {            // Query for all Project records owned by this user.
            var queryRequest = new QueryRequest
            {
                TableName = this.projectsTableName,
                KeyConditionExpression = $"OwningUser = :v_user",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":v_user", new AttributeValue { S = username } } },
            };

            QueryResponse results = await this.dynamoDB.QueryAsync(queryRequest);
            var projects = new List<Project>();

            // Convert the NoSql document into a domain model.
            foreach (Dictionary<string, AttributeValue> document in results.Items)
            {
                Guid id = Guid.Parse(document["Id"].S);
                var project = new Project(id, document["OwningUser"].S, document["Priority"].S, document["Title"].S);
                projects.Add(project);
            }

            return projects.ToArray();
        }
    }
}
