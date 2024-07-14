using Microsoft.Extensions.Configuration;

namespace PlayGround
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                               .AddUserSecrets<Program>()
                               .Build();
            string connString = config["ConnectionString"]!;
            DBContext dbc = new DBContext(connString);
            Demo demo = new Demo(dbc);
            demo.GetAllEmployees();
        }
    }
}
