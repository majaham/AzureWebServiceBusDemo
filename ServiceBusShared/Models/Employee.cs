
namespace ServiceBusShared.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Employee
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public override string ToString()
        {
            return string.Format($"EmployeeId: {this.EmployeeId} Firstname: {this.Firstname} Lastname: {this.Lastname}");
        }
    }
}
