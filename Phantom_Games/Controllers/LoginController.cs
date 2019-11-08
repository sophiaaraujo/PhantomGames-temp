using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phantom_Games.Models;


namespace Phantom_Games.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult SaveResults()
        {
            return View();
        }

        public IActionResult TemporaryUser()
        {
            return View();
        }

        public IActionResult GoToOp()
        {
            Login user = Models.Login.ObterCookie(Request);

            ViewBag.user = user;

            return View("Home/Opcoes");

        }

        [HttpPost]
        public IActionResult SalvarNovo([FromForm] Login user)
        {
            if (user == null)
            {
                throw new Exception("Dados Faltantes");

            }
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new Exception("Nome Inválido");

            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Email Inválido");

            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("Senha Inválida");
            }

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=banco_site;Uid=root;Pwd=root;"))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO user (name, email, password) VALUES (@name, @email, @password)", conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password", user.Password);

                    cmd.ExecuteNonQuery();
                }


            }

            return View("Register");
        }

        [HttpPost]
        public IActionResult Salvar([FromForm] Login user)
        {
            if (user == null)
            {
                throw new Exception("Dados Faltantes");

            }
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new Exception("Nome Inválido");

            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Email Inválido");

            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("Senha Inválida");
            }

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=banco_site;Uid=root;Pwd=root;"))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE user SET name = @name, email = @email, password = @password WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);

                    cmd.Parameters.AddWithValue("@email", user.Email);

                    cmd.Parameters.AddWithValue("@password", user.Password);

                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();
                }


            }

            return View("Register");
        }

        [HttpPost]
        public IActionResult Logar([FromBody]Login user)
        {
            if (user == null)
            {
                throw new Exception("Dados Faltantes");

            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Email Inválido");

            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("Senha Inválida");
            }

            int id;
            string token;

            using (MySqlConnection conn = Sql.Conn())
            {

                using (MySqlCommand cmd = new MySqlCommand("SELECT id FROM user WHERE email = @email AND password = @password", conn))
                {
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    cmd.Parameters.AddWithValue("@password", user.Password);

                    object o = cmd.ExecuteScalar();
                    if (o == null || o == DBNull.Value)
                        throw new Exception("Usuário ou senha inválidos!");

                    id = (int)o;
                }

                token = Guid.NewGuid().ToString();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE user SET token = @token WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }


            }

            Models.Login.GerarCookie(Response, id, token);

            return View("Register");
        }

        public IActionResult Logout()
        {
            Login user = Models.Login.ObterCookie(Request);

            if (user != null)
            {
                using (MySqlConnection conn = Sql.Conn())
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE user SET token = NULL WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

                Models.Login.ExcluirCookie(Response);
            }

            return Redirect("/");
        }
    }
}
