namespace SIGES.Classes
{
    public class Paginacao<T> : List<T>
    {
        public int PaginaAtual { get; private set; }
        public int TotalPaginas { get; private set; }
        public int TotalRegistros { get; private set; }

        public Paginacao(List<T> items, int count, int paginaAtual, int registrosPorPagina)
        {
            PaginaAtual = paginaAtual;
            TotalRegistros = count;
            TotalPaginas = (int)Math.Ceiling(count / (double)registrosPorPagina);

            AddRange(items);
        }

        public bool HasPreviousPage => PaginaAtual > 1;
        public bool HasNextPage => PaginaAtual < TotalPaginas;

        public static async Task<Paginacao<T>> CreateAsync(IQueryable<T> source, int paginaAtual, int registrosPorPagina)
        {
            var count = source.Count();
            var items = source.Skip((paginaAtual - 1) * registrosPorPagina).Take(registrosPorPagina).ToList();
            return new Paginacao<T>(items, count, paginaAtual, registrosPorPagina);
        }

    }
}
