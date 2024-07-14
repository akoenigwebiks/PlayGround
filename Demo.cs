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
    }
}
