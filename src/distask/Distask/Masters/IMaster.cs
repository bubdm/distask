using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.Masters
{
    public interface IMaster : IDisposable
    {
        Task<ResponseMessage> ExecuteAsync(RequestMessage requestMessage, string group = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
