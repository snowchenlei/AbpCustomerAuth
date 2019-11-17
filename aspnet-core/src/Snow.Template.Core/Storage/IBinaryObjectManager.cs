using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Template.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);

        Task SaveAsync(BinaryObject file);

        Task DeleteAsync(Guid id);
    }
}
