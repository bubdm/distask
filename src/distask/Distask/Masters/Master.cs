using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskRegistrationService;

namespace Distask.Masters
{
    public class Master : DistaskRegistrationServiceBase, IMaster
    {
        public Task<ResponseMessage> ExecuteAsync(RequestMessage requestMessage, string group = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<RegistrationResponse> Register(RegistrationRequest request, ServerCallContext context)
        {
            return base.Register(request, context);
        }
    }
}
