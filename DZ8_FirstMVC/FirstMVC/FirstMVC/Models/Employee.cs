namespace FirstMVC.Models
{
    public enum Gender
    {
        Male,
        Female,
        RoboMale,
        RoboFemale
    }

    public class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public Gender Gender { get; set; }
        public int Salary { get; set; }

        public int DepartamentID { get; set; }
        public Departament Departament { get; set; }
    }
}
