using Microsoft.Data.SqlClient;

namespace PlayGround
{
    internal class Demo
    {
        private readonly DBContext _dbContext;
        public Demo(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public void GetAllEmployees()
        {
            string json = _dbContext.MakeQuery("SELECT * FROM Employees").ToJson();
            Console.WriteLine(json);
        }

        public void GetEmployeeById()
        {
            string promptGetBy = $@"Select Key:
                    1.[EmployeeID]
                    2.[LastName]
                    3.[FirstName]";
            Console.WriteLine(promptGetBy);
            string? selectedCol = Console.ReadLine();

            string column = selectedCol switch
            {
                "1" => "EmployeeID",
                "2" => "LastName",
                "3" => "FirstName",
                _ => ""
            };

            if (string.IsNullOrEmpty(selectedCol))
            {
                Console.WriteLine("Invalid col selected,try again");
                GetEmployeeById();
                return;
            }

            string prompValue = $"Enter value to search by {column}";
            Console.WriteLine(prompValue);

            string? cellValue = Console.ReadLine();

            if (string.IsNullOrEmpty(selectedCol) || string.IsNullOrEmpty(cellValue))
            {
                Console.WriteLine("Error input,please try again");
                GetEmployeeById();
                return;
            }

            string query = @$"SELECT EmployeeID,LastName,FirstName
                                FROM Employees
                                WHERE {column} = @Value";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@Col",column),
                new SqlParameter("@Value",cellValue)
            };

            string json = _dbContext.MakeQuery(query, sqlParameters).ToJson();
            Console.WriteLine(json);
        }

        public void GetEmployeesBornBetweenYears()
        {
            // Prompt the user for the start year
            Console.WriteLine("Enter the start year (between 1948 and 1966):");
            if (!int.TryParse(Console.ReadLine(), out int startYear) || startYear < 1948 || startYear > 1966)
            {
                Console.WriteLine("Invalid input. Please enter a valid start year between 1948 and 1966.");
                GetEmployeesBornBetweenYears();
                return;
            }

            // Prompt the user for the end year
            Console.WriteLine("Enter the end year (between 1948 and 1966):");
            if (!int.TryParse(Console.ReadLine(), out int endYear) || endYear < 1948 || endYear > 1966 || endYear < startYear)
            {
                Console.WriteLine("Invalid input. Please enter a valid end year between 1948 and 1966 that is greater than or equal to the start year.");
                GetEmployeesBornBetweenYears();
                return;
            }

            // Create DateTime objects for the start and end dates
            DateTime startDate = new DateTime(startYear, 1, 1);
            DateTime endDate = new DateTime(endYear, 12, 31);

            // Construct the query to get employees born between the specified dates
            string query = @"SELECT EmployeeID, LastName, FirstName, BirthDate
                                FROM Employees
                                WHERE BirthDate BETWEEN @StartDate AND @EndDate";

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            // Execute the query and convert the result to JSON
            string json = _dbContext.MakeQuery(query, sqlParameters).ToJson();
            Console.WriteLine(json);
        }

    }
}
