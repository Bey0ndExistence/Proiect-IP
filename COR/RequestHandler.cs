using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageNamespace;

namespace ServerRequestHandler
{
    public abstract class RequestHandler
    {
        protected RequestHandler _next;

        protected RequestHandler(RequestHandler next = null)
        {
            _next = next;
        }

        public void SetNext(RequestHandler next)
        {
            _next = next;
        }

        public abstract void Handle(Message msg);
    }
}
