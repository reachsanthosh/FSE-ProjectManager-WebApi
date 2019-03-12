using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.Model
{
    public class TaskDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Priority { get; set; }

        public bool ActiveStatus { get; set; }

        public int? ParentId { get; set; }

        public User UserDetail { get; set; }

        public int UserId { get; set; }

        public Project ProjectDetail { get; set; }

        public int ProjectId { get; set; }
    }
}
