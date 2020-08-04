namespace TeisterMask.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        [RegularExpression(@"^[A-za-z\d]+$")]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^[\d]{3}-[\d]{3}-[\d]{4}$")]
        public string Phone { get; set; }

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}
//Id - integer, Primary Key
//Username - text with length[3, 40]. Should contain only lower or upper case letters and/or
//digits. (required)
//Email – text(required). Validate it! There is attribute for this job.
//Phone - text.Consists only of three groups (separated by '-'), the first two consist of three
//digits and the last one - of 4 digits. (required)
//EmployeesTasks - collection of type EmployeeTask