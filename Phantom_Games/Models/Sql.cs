using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phantom_Games.Models
{
    public static class Sql
    {
        public static MySqlConnection Conn()
        {
            MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=banco_site;Uid=root;Pwd=root;");
            conn.Open();
            return conn;
        }
    }
}
