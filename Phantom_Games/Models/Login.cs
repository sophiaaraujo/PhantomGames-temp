
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phantom_Games.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    

        public static void GerarCookie(HttpResponse response, int id, string token)
        {
            response.Cookies.Append("usuarioLogado", id + "|" + token, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(365),
                Secure = false,
                HttpOnly = false
            });
        }

        public static void ExcluirCookie(HttpResponse response)
        {
            response.Cookies.Append("usuarioLogado", "", new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(-100),
                Secure = false,
                HttpOnly = false
            });
        }

        public static Login ObterCookie(HttpRequest request)
        {
            var cookie = request.Cookies["usuarioLogado"];

            if (cookie == null)
                return null;

            int i = cookie.IndexOf('|');
            if (i <= 0)
                return null;

            string sid = cookie.Substring(0, i);
            string token = cookie.Substring(i + 1);

            if (!int.TryParse(sid, out int id))
                return null;

            if (token.Length <= 0)
                return null;

            using (MySqlConnection conn = Sql.Conn())
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT name, email FROM user WHERE id = @id AND token = @token", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@token", token);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Login()
                        {
                            Id = id,
                            Name = reader.GetString(0),
                            Email = reader.GetString(1)
                        };
                    }
                }
            }
        }
    }
}
