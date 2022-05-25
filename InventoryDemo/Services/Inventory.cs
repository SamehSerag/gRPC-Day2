using Grpc.Core;
using InventoryDemo.Protos;

namespace InventoryDemo.Services
{
    public class Inventory: InventoryProto.InventoryProtoBase
    {
        List<Product> ProductsInInventory = new List<Product>()
        {
            new Product(){ Id = 1, Name = "Product 1", Quantity = 3 },
            new Product(){ Id = 2, Name = "Product 2", Quantity = 0},
            new Product(){ Id = 3, Name = "Product 3", Quantity = 1}
        };
        public override Task<Response> CheckInventory(Products request, ServerCallContext context)
        {
            Response response = new Response() { Succeed = false};
            foreach (Product item in request.ProductList)
            {
                var productInStock = ProductsInInventory.Where(x => x.Id == item.Id).FirstOrDefault();

                if (productInStock == null || productInStock.Quantity < item.Quantity)
                {
                    response.Response_ = "Product not found";
                    return Task.FromResult(response);
                }
            }

            response.Succeed = true;
            response.Response_ = "Found in Inventory";
            return Task.FromResult(response);
        }
    }
}
