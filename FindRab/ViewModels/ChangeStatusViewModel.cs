using System.Collections.Generic;

namespace FindRab.ViewModels
{
    public class ChangeStatusViewModel
    {
        public int EmployeeId { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
        public int StatusId { get; set; }
        public List<StatusViewModel> Statuses { get; set; }
    }

    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
    }
}
