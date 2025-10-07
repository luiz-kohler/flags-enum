namespace API
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public bool IsHirer { get; set; }
        public bool IsPassenger { get; set; }
        public bool IsFinancialManager { get; set; }
    }

    public class UpdateUserDto
    {
        public string Name { get; set; }
        public bool IsHirer { get; set; }
        public bool IsPassenger { get; set; }
        public bool IsFinancialManager { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsHirer { get; set; }
        public bool IsPassenger { get; set; }
        public bool IsFinancialManager { get; set; }
        public List<string> RoleNames { get; set; } = new();
    }
}
