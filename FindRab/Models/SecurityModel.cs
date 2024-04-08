using System.ComponentModel.DataAnnotations;

namespace FindRab.Models
{
    public class SecurityModel
    {
        [Key]
        public int ID_Client { get; set; }
        public string Log_in { get; set; }
        public string Pass_word { get; set; }

        public string Code(string pass)
        {
            string password = "";
            foreach (char item in pass)
            {
                if (item < 100)
                {
                    password += Convert.ToChar(item + 'a'); //английская
                }
                else if (item > 99 & item < 200)
                {
                    password += Convert.ToChar(item + 'а'); //русская
                }
                else
                {
                    password += Convert.ToChar(item + '5');
                }

            }
            return password;
        }
    }
}
