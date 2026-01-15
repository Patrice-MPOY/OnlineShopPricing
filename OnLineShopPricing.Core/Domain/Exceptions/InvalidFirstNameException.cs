using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public sealed class InvalidFirstNameException : DomainException
    {
        public InvalidFirstNameException()
            : base("First name is required.") { }
    }

    
}
