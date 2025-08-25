using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.ProductServcie.Domain.Entities;

public class Brand : Entity
{
    
    public int Id { get; set; }
    public string Name { get; set; }
}
