using FindRab.Models;

namespace FindRab.ViewModels
{
    public class Autorization
    {
        public SecurityModel Security = new SecurityModel();
        public string Login;
        public string Password;
        public Autorization(SecurityModel security)
        {
            Security = security;
        }
    }
}
