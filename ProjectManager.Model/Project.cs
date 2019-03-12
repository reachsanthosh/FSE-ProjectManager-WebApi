using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.Model
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Priority { get; set; }

        public int UserId { get; set; }


        public bool ActiveStatus { get; set; }

        public List<TaskDetail> TaskDetails { get; set; }

        public User UserDetail { get; set; }

    }
}
