using System;

namespace Todo.Projects.Domain
{
    public class Project
    {
        public Project(Guid id, string owner, string priority, string status, string title, string type)
        {
            this.Id = id;
            this.OwningUser = owner;
            this.SetTitle(title);

            this.Priority = (Prioritization)Enum.Parse(typeof(Prioritization), priority);
            this.Type = (ProjectType)Enum.Parse(typeof(ProjectType), type);

            var projectStatus = (Status)Enum.Parse(typeof(Status), status);
            switch (projectStatus)
            {
                case Status.Active:
                    this.ActivateProject();
                    break;
                case Status.Cancelled:
                    this.CancelProject();
                    break;
                case Status.Completed:
                    this.CompleteProject();
                    break;
                case Status.Paused:
                    this.PauseProject();
                    break;
            }
        }

        public Guid Id { get; }

        public string OwningUser { get; }

        public string Title { get; private set; }

        public bool IsFlagged { get; private set; }

        public ProjectType Type { get; set; }

        public bool IsArchived { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? TargetDate { get; private set; }

        public DateTime? CompletionDate { get; private set; }

        public Status Status { get; private set; }

        public short PercentageCompleted { get; private set; }

        public Prioritization Priority { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public override string ToString() => $"{this.Id}: {this.Title}";

        public void SetPercentageComplete(short percent)
        {
            if (percent > 100)
            {
                this.PercentageCompleted = 100;
            }

            if (percent < 0)
            {
                this.PercentageCompleted = 0;
            }

            this.PercentageCompleted = percent;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.Title = title;
        }

        public void FlagProject() => this.IsFlagged = true;

        public void UnflagProject() => this.IsFlagged = false;

        public void ArchiveProject() => this.IsArchived = true;

        public void ActivateProject()
        {
            if (this.IsArchived)
            {
                this.IsArchived = false;
            }

            this.Status = Status.Active;
            this.CompletionDate = null;
        }

        public void PauseProject()
        {
            this.Status = Status.Paused;
            this.CompletionDate = null;
        }

        public void CancelProject()
        {
            this.Status = Status.Cancelled;
            this.CompletionDate = null;
            this.ArchiveProject();
        }

        public void CompleteProject()
        {
            this.PercentageCompleted = 100;
            this.Status = Status.Completed;
            this.CompletionDate = DateTime.UtcNow;
        }

        public void StartProject(DateTime startDate)
        {
            if (startDate > this.TargetDate)
            {
                this.TargetDate = startDate;
            }

            if (this.CompletionDate < startDate)
            {
                this.CompletionDate = startDate;
            }

            this.StartDate = startDate;
        }

        public void SetTargetDate(DateTime targetDate)
        {
            this.TargetDate = targetDate;

            if (targetDate < this.StartDate)
            {
                this.StartDate = targetDate;
            }

            this.CompletionDate = null;
        }
    }
}
