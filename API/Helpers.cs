using System.ComponentModel;

namespace API
{
    public static class Helpers
    {
        public static List<string> GetRoleDescriptions(ERoles roles)
        {
            var roleList = new List<string>();

            if (roles == ERoles.None)
            {
                roleList.Add("None");
                return roleList;
            }

            foreach (ERoles role in Enum.GetValues(typeof(ERoles)))
            {
                if (role != ERoles.None && roles.HasFlag(role))
                {
                    var fieldInfo = role.GetType().GetField(role.ToString());
                    var descriptionAttribute = fieldInfo?
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    roleList.Add(descriptionAttribute?.Description ?? role.ToString());
                }
            }

            return roleList;
        }

        public static ERoles ConvertToRoles(bool isHirer, bool isPassenger, bool isFinancialManager)
        {
            ERoles roles = ERoles.None;

            if (isHirer) roles |= ERoles.Hirer;
            if (isPassenger) roles |= ERoles.Passenger;
            if (isFinancialManager) roles |= ERoles.FinancialManager;

            return roles;
        }

        public static UserDto ConvertToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                IsHirer = user.Roles.HasFlag(ERoles.Hirer),
                IsPassenger = user.Roles.HasFlag(ERoles.Passenger),
                IsFinancialManager = user.Roles.HasFlag(ERoles.FinancialManager),
                RoleNames = GetRoleDescriptions(user.Roles)
            };
        }
    }
}
