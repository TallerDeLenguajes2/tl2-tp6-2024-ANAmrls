namespace TP6.Models
{
    public class User
    {
        public User()
        {
        }

        public User(int id, string userName, string password, AccessLevel accessLevel, string name)
        {
            Id = id;
            UserName = userName;
            Password = password;
            AccessLevel = accessLevel;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public AccessLevel AccessLevel { get; set; }
        
    }

    public enum AccessLevel
    {
        NoLogueado,
        Administrador,
        Cliente,
    }
}
