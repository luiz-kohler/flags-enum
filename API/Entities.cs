using System.ComponentModel;

namespace API
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ERoles Roles { get; set; }
    }

    [Flags]
    public enum ERoles
    {
        [Description("None")]
        None = 0,                  // 0
        [Description("Hirer")]
        Hirer = 1 << 1,            // 1
        [Description("Passenger")]
        Passenger = 1 << 2,        // 2
        [Description("FinancialManager")]
        FinancialManager = 1 << 3  // 4
    }
}
