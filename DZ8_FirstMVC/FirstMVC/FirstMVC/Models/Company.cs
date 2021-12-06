using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstMVC.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int EstablishedYear { get; set; }


        public string Location { get; set; }
        public string Phone { get; set; }        

        public List<Departament> Departaments { get; set; } = new List<Departament>();
    }
}
