﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Entity.Model
{
	public class Rol
	{
		public int Id { get; set; }
		public string NameRol { get; set; } = string.Empty;
		public string Description { get; set; }
		public bool Status { get; set; }
        public User User { get; set; }
        public ICollection<RolForm> RolForms { get; set; }
    }
}
