using FirstMVC.Models;
using System.Linq;

namespace FirstMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CompanyContext context)
        {
            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return;   // DB has been seeded
            }

            var company = new Company() {
                CompanyID=1,

                Name = "Planet Express, Inc.",
                Description = "Planet Express, Inc. is the delivery compаny started by Professor Hubert J. Farnsworth " +
                    "to help fund his research and employs a range of individuals to help deliver packages to clients. " +
                    "Our crew is replaceable. Your package isn't.",
                ShortDescription = "Intergalactic delivery company",
                EstablishedYear = 2961,

                Location = "Planet Express headquarters, Manhattan, 57th Street, New New York, United States, Earth, Solar System, Milky Way, Universe Γ",
                Phone = "+01 232 456 7980",
            };

            context.Company.Add(company);
            context.SaveChanges();


            var departaments = new Departament[]
            {
                new Departament{CompanyID=1, DepartamentID=1, DepartamentName ="Office Management"},
                new Departament{CompanyID=1, DepartamentID=2, DepartamentName ="Space Ship Crew"},
                new Departament{CompanyID=1, DepartamentID=11, DepartamentName ="Office Crew"},
                new Departament{CompanyID=1, DepartamentID=12, DepartamentName ="Involved employees"}
            };
            foreach (Departament c in departaments)
            {
                context.Departaments.Add(c);
            }
            context.SaveChanges();


            var employee = new Employee[]
            {
                new Employee{LastName="Farnsworth",FirstName="Hubert J.",DepartamentID=1,Gender=Gender.Male,Position="Scientist, owner and founder of the Planet Express delivery ",Salary=50000},
                new Employee{LastName="Conrad",FirstName="Hermes",DepartamentID=1,Gender=Gender.Male,Position="Bureaucrat and Accountant",Salary=5000},

                new Employee{LastName="Turanga",FirstName="Leela",DepartamentID=2,Gender=Gender.Female,Position="Captain and pilot of the Planet Express delivery ship",Salary=10000},
                new Employee{LastName="Fry",FirstName="Philip J.",DepartamentID=2,Gender=Gender.Male,Position="Delivery boy",Salary=500},
                new Employee{LastName="Rodríguez",FirstName="Bender Bending",DepartamentID=2,Gender=Gender.RoboMale,Position="Ship's Robot/Temporary Ship's Cook/Temporary Ship's Captain/Co-Pilot/Assistant Manager of Sales/Gunnery Chief",Salary=1500},

                new Employee{LastName="Wong-Kroker",FirstName="Amy ",DepartamentID=11,Gender=Gender.Female,Position="internship, Mars University student",Salary=1000},
                new Employee{LastName="Zoidberg",FirstName="John A.",DepartamentID=11,Gender=Gender.Male,Position="Staff doctor",Salary=1500},
                new Employee{LastName="Scruffington",FirstName="Scruffy",DepartamentID=11,Gender=Gender.Male,Position="Janitor",Salary=800},

                new Employee{LastName="Nibbler",FirstName="Lord",DepartamentID=12,Gender=Gender.Male,Position="Member, Pet, Ambassador to Earth",Salary=0}
            };
            foreach (Employee s in employee)
            {
                context.Employees.Add(s);
            }
            context.SaveChanges();
        }
    }
}