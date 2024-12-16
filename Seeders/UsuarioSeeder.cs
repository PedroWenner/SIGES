using Microsoft.EntityFrameworkCore;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Seeders
{
    public class UsuarioSeeder : ISeeder
    {
        public void Seed(ModelBuilder modelBuilder)
        {
            // Seed para a tabela de Permissões
            modelBuilder.Entity<PermissaoModel>().HasData(
                new PermissaoModel
                {
                    Id = 1,
                    Nome = "Master",
                    Situacao = 1
                }
            );

            // Seed para a tabela de Usuários (com senha hasheada)
            modelBuilder.Entity<UsuarioModel>().HasData(
                new UsuarioModel
                {
                    Id = 1,
                    Nome = "Admin",
                    Cpf = "000.000.000-00",
                    Email = "teste@teste.com",
                    Senha = BCrypt.Net.BCrypt.HashPassword("123"),
                    Situacao = 1,
                    PermissaoId = 1,
                    Master = "S"
                }
            );
        }
    }
}
