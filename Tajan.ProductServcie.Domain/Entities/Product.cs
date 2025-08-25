using System.ComponentModel;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.ProductService.API.Entities;

public class Product : Entity
{

    public static Product Create(string name)
    {
        if (name == null || name.Length <= 2) 
            throw new ArgumentNullException("name");
        
        return new Product()
        {
            Name = name,
        };
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
}
