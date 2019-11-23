using System;

namespace Todo.Projects.Domain
{
    public class Project
    {
        public Project(Guid id, string owner, string priority, string title)
        {
            this.Id = id;
            this.OwningUser = owner;
            this.SetTitle(title);

            this.Priority = (Prioritization)Enum.Parse(typeof(Prioritization), priority);
        }

        public Guid Id { get; }

        public string OwningUser { get; }

        public string Title { get; private set; }

        public bool IsFlagged { get; private set; }

        public bool IsArchived { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? TargetDate { get; private set; }

        public DateTime? CompletionDate { get; private set; }

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
