using WebAppDia2.Entities;

namespace WebAppDia3.Entities
{
    public class Supplier
    {

        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public ICollection<Product> Products { get; set ;}
    }
}
