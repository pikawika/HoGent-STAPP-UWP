using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace uwp_app_aalst_groep_a3.Utils
{
    public static class UserUtils
    {
        private static PasswordVault passwordVault = new PasswordVault();
        private static JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

        public static string GetUserToken()
        {
            PasswordCredential pc = passwordVault.Retrieve("Stapp", "Token");
            pc.RetrievePassword();
            return pc.Password;
        }

        public static string GetUserRole()
        {
            var token = jwtHandler.ReadJwtToken(GetUserToken());

            return token.Claims.SingleOrDefault(s => s.Type == "customRole").Value;
        }

        public static void RemoveUserToken()
        {
            PasswordCredential pc = passwordVault.Retrieve("Stapp", "Token");
            passwordVault.Remove(pc);
        }
    }
}
