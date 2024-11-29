using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace приложение
{
    public class CurrentUser
    {
        private static CurrentUser _instance;
        public static int ID { get; set; }
        public string Role { get; set; }
        private CurrentUser(int id, string role)
        {
            ID = id;
            Role = role;
        }
        public static CurrentUser Instance(int id = 0, string role = "")
        {
            if (_instance == null)
            {
                _instance = new CurrentUser(id, role); 
            }

            return _instance;
        }
    }
}
