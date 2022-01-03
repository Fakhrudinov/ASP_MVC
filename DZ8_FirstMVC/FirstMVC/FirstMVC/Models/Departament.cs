using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstMVC.Models
{
    public class Departament
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartamentID { get; set; }
        public string DepartamentName { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
