using System;
using System.Collections.Generic;

namespace ProjectManager.Model
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeeId { get; set; }

        public IList<Project> Projects { get; set; }

        public IList<TaskDetail> TaskDetails { get; set; }
    }
}
