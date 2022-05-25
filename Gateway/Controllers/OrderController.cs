using Gateway.Model;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using InventoryDemo.Protos;
using Microsoft.AspNetCore.Mvc;
using PaymentDemo.Protos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public static List<CustomOrder> OrdersList = new List<CustomOrder>();
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<CustomOrder> Get()
        {
            return OrdersList;
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public CustomOrder? Get(int id)
        {
            return OrdersList.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST api/<OrderController>
        [HttpPost]
        public Model.Response Post([FromBody] CustomOrder order)
        {

            /// payment
            var paymentChannel = GrpcChannel.ForAddress("https://localhost:7286");
            var paymentClient = new PaymentProto.PaymentProtoClient(paymentChannel);

                      
            var orderRequest = new OrderMassage()
            {
                Id = order.Id,
                UserId = order.User.Id,
                TotalMoney = order.TotalMoney,
                Stamp = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            foreach (var item in order.Products)
            {
                orderRequest.Product.Add( new PaymentDemo.Protos.Product() 
                { Id = item.Id, Quantity = item.Quantity});
            }

            var orderResponse = paymentClient.ApplyPayment(orderRequest);


            /// inventory

            var inventoryChannel = GrpcChannel.ForAddress("https://localhost:7174");
            var inventoryClient = new InventoryProto.InventoryProtoClient(inventoryChannel);

            var inventoryRequest = new Products();
            foreach (var item in order.Products)
            {
                inventoryRequest.ProductList.Add(new InventoryDemo.Protos.Product()
                { Id = item.Id, Quantity = item.Quantity });
            }

            var inventoryResponse =  inventoryClient.CheckInventory(inventoryRequest);

            if (!orderResponse.Succeed)
                return new Model.Response() { Succeed = orderResponse.Succeed, Data = orderResponse.Response_ };
            else if (!inventoryResponse.Succeed)
                return new Model.Response() { Succeed = inventoryResponse.Succeed, Data = inventoryResponse.Response_ };
            else
                return new Model.Response(){ Succeed = true, Data = "Congratolation"};

        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
