using Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	namespace Entity.Model
	{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PhoneNumber { get; set; } = 0;
        public TypeIdentification TypeIdentification { get; set; }
        public bool Status { get; set; }
        public int NumberIndification { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }

    }
}
