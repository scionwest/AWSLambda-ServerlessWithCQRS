using System;

namespace Todo.Projects.Dtos
{
    public class CreateProjectCommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; }

        public bool IsFlagged { get; set; }

        public string Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public string Status { get; set; }

        public short PercentageCompleted { get; set; }

        public string Priority { get; set; }
    }
}
