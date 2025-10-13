using System.ComponentModel;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.ProductService.API.Entities;

public class Product : Entity
{
    public static Product Create(int id,string name, long price)
    {
        if (name == null || name.Length <= 2)
            throw new ArgumentNullException("name");

        return new Product()
        {
            Id = id,
            Price = price,
            Name = name,
        };
    }
    public static Product Create(string name,long price)
    {
        if (name == null || name.Length <= 2) 
            throw new ArgumentNullException("name");
        
        return new Product()
        {
            Price = price,
            Name = name,
        };
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public long Price { get; private set; }
}
