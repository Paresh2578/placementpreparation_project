using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Constant
{
    public class CommonMethods{
        public static bool IsValidEmail(string email){
            try{
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch{
                return false;
            }
        }
    }
}
