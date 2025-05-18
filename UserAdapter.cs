using System;

namespace MedicalLaboratory
{
    // Адаптер с явным преобразованием в User
    public class UserAdapter
    {
        public User ToUser()
        {
            return new User
            {
                Id = this.Id,
                Name = this.Name,
                Login = this.Login,
                Password = this.Password,
                Type = this.Type
            };
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public int Type { get; private set; }

        public UserAdapter(SystemUsers systemUser)
        {
            if (systemUser == null)
                throw new ArgumentNullException(nameof(systemUser));

            this.Id = systemUser.UserId;
            this.Name = systemUser.FullName;
            this.Login = systemUser.Login;
            this.Password = systemUser.Password;
            this.Type = systemUser.RoleId ?? 0;
        }

        // Неявное преобразование
        public static implicit operator User(UserAdapter adapter)
        {
            return adapter.ToUser();
        }
    }
}
