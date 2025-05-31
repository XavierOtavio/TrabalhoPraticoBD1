using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabalhoFinal3.Models
{
    public class User
    {
        // PK
        public int UserId { get; set; }

        // Dados de autenticação
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // FK para papéis/perfis e estado do utilizador
        public int UserRoleId { get; set; }
        public int UserStatusId { get; set; }

        // Dados pessoais
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Propriedades de navegação (EF ou manual)
        //public UserRole Role { get; set; }
        //public UserStatus Status { get; set; }
    }
}