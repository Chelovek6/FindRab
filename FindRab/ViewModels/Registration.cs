using FindRab.Models;

namespace FindRab.ViewModels
{
    public class Registration
    {
        public SecurityModel Security = new SecurityModel();
        public string Login;
        public string Password;
        public Registration(SecurityModel security)
        {
            Security = security;
        }

    }
}
