namespace TeisterMask.DataProcessor.ImportDto
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ImportEmplyeesDto
    {
        [MinLength(3), MaxLength(40)]
        [RegularExpression(@"^[A-z\d]+$")]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^[\d]{3}-[\d]{3}-[\d]{4}$")]
        public string Phone { get; set; }

        public ICollection<int> Tasks { get; set; }
    }
}
//"Username": "jstanett0",
//    "Email": "kknapper0@opera.com",
//    "Phone": "819-699-1096",
//    "Tasks": [
//      34,
//      32,
//      65,
//      30,
//      30,
//      45,
//      36,
//      67
//    ]