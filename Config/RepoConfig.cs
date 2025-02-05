using SIGES.Interfaces;
using SIGES.Repositorios;
using SIGES.Services;

namespace SIGES.Config
{
    public static class RepoConfig
    {
        public static void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<IPermissaoRepositorio, PermissaoRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<ILogService, LogServiceRepositorio>();
            services.AddScoped<IServico, ServicoRepositorio>();
            services.AddScoped<IOs, OsRepositorio>();
            services.AddScoped<IVenda, VendaRepositorio>();
            services.AddScoped<IItensVenda, IItensVendaRepositorio>();
            services.AddScoped<IEmitente, EmitenteRepositorio>();
            services.AddScoped<IGarantia, GarantiaRepositorio>();
            services.AddScoped<IConfiguracao, ConfiguracaoRepositorio>();
            services.AddScoped<ICobranca, CobrancaRepository>();
            services.AddScoped<IPaymentGateway, MercadoPagoService>();
            services.AddScoped<IEmailQueueRepository, EmailQueueRepository>();
            services.AddScoped<ILancamento, LancamentoRepositorio>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
