using System;
using Todo.Projects.Domain;

namespace Todo.Projects.Dtos
{
    public class GetProjectDto
    {
        public GetProjectDto(Project project)
        {
            this.Id = project.Id.ToString();
            this.Title = project.Title;
            this.IsFlagged = project.IsFlagged;
            this.IsArchived = project.IsArchived;
            this.StartDate = project.StartDate;
            this.TargetDate = project.TargetDate;
            this.CompletionDate = project.CompletionDate;
            this.PercentageCompleted = project.PercentageCompleted;
            this.Priority = project.Priority.ToString();
        }

        public string Id { get; }

        public string Title { get; private set; }

        public bool IsFlagged { get; private set; }

        public bool IsArchived { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? TargetDate { get; private set; }

        public DateTime? CompletionDate { get; private set; }

        public short PercentageCompleted { get; private set; }

        public string Priority { get; set; }
    }
}
