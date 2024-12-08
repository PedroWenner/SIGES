using SIGES.Interfaces;

namespace SIGES.BaseDados
{
    public class UnidadeMedida : IUnidadeMedida
    {
        //public List<UnidadeMedida> GetUnidadesMedida()
        //{
        //    return new List<UnidadeMedida>
        //    {
        //        new UnidadeMedida("Quilograma", "kg"),
        //        new UnidadeMedida("Grama", "g"),
        //        new UnidadeMedida("Tonelada", "t"),
        //        new UnidadeMedida("Litro", "l"),
        //        new UnidadeMedida("Mililitro", "ml"),
        //        new UnidadeMedida("Metro", "m"),
        //        new UnidadeMedida("Centímetro", "cm"),
        //        new UnidadeMedida("Milímetro", "mm"),
        //        new UnidadeMedida("Metro quadrado", "m²"),
        //        new UnidadeMedida("Centímetro quadrado", "cm²"),
        //        new UnidadeMedida("Metro cúbico", "m³"),
        //        new UnidadeMedida("Centímetro cúbico", "cm³"),
        //        new UnidadeMedida("Unidade", "un"),
        //        new UnidadeMedida("Dúzia", "dz"),
        //        new UnidadeMedida("Par", "pr"),
        //        new UnidadeMedida("Pacote", "pct"),
        //        new UnidadeMedida("Caixa", "cx"),
        //        new UnidadeMedida("Litro por metro cúbico", "l/m³"),
        //        new UnidadeMedida("Quilograma por metro cúbico", "kg/m³"),
        //        new UnidadeMedida("Jarro", "jr")
        //    };
        //}

        List<Classes.UnidadeMedida> IUnidadeMedida.GetUnidadesMedida()
        {
            throw new NotImplementedException();
        }
    }
}
