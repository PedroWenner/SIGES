using SIGES.Interfaces;
using SIGES.Repositorios;

namespace SIGES.Config
{
    public static class RepoConfig
    {
        public static void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            //services.AddScoped<IUnidadeMedidaService, UnidadeMedidaService>();
            services.AddScoped<IPermissaoRepositorio, PermissaoRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            services.AddScoped<ILogService, LogServiceRepositorio>();
        }
    }
}
