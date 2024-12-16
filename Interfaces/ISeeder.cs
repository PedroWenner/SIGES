using Microsoft.EntityFrameworkCore;

namespace SIGES.Interfaces
{
    public interface ISeeder
    {
        void Seed(ModelBuilder modelBuilder);
    }
}
