﻿namespace WebAppDia2.Contract.Dtos
{


    public class ProductDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string CategoryName {get;set;}

        public string SupplierName { get; set; }

    }
}
