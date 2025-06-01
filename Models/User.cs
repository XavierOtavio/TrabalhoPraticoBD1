using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3.Models
{
        /// <summary>
        /// Mapeia a tabela sc24_197.USERROLE
        /// </summary>
        public class UserRole
        {
            public int UserRoleId { get; set; }
            public string RoleName { get; set; }
            public string RoleDescription { get; set; }
        }

        /// <summary>
        /// Mapeia a tabela sc24_197.USER_STATUS
        /// </summary>
        public class UserStatus
        {
            public int UserStatusId { get; set; }
            public string StatusName { get; set; }
        }

    public class User
    {
        // Chave primária
        public int UserId { get; set; }

        // Credenciais
        public string Email { get; set; }
        public string Password { get; set; }   // texto-claro, para já
        public int UserRoleId { get; set; }
        public int UserStatusId { get; set; }

        // Identificação
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }   // cargo / função
        public string Bio { get; set; }
        public string PhotoPath { get; set; }

        // Contacto
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        // Preferências
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public string NotifyOptions { get; set; }   // “email,sms,push”

        // Propriedades de navegação (opcional)
        public string RoleName { get; set; }
        public string StatusName { get; set; }
    }
    public class NotifyOptions
    {
        public bool Email { get; set; }
        public bool SMS { get; set; }
        public bool Push { get; set; }
    }
}