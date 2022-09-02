using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VR.DataAccess;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Base
{
    public abstract class BaseRequestHandler<T, V> : IRequestHandler<T, V> where T : IRequest<V>
                                                                           where V : class
    {
        protected IObjectMapper _mapper;
        protected readonly VRContext _context;

        public BaseRequestHandler(IObjectMapper mapper, VRContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public abstract Task<V> Handle(T request, CancellationToken cancellationToken);
    }
}
