using Gateway.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using InventoryDemo.Protos;
using PaymentDemo.Protos;

namespace Gateway.Service
{
    public class OrderService: TrackingOrder.TrackingOrderBase
    {
        public override Task<Protos.Response> SendOrder(Order order, ServerCallContext context)
        {
            /// payment
            var paymentChannel = GrpcChannel.ForAddress("https://localhost:7286");
            var paymentClient = new PaymentProto.PaymentProtoClient(paymentChannel);


            var orderRequest = new OrderMassage()
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalMoney = order.TotalMoney,
                Stamp = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            foreach (var item in order.Products)
            {
                orderRequest.Product.Add(new PaymentDemo.Protos.Product()
                { Id = item.Id, Quantity = item.Quantity });
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

            var inventoryResponse = inventoryClient.CheckInventory(inventoryRequest);

            if (!orderResponse.Succeed)
                return  Task.FromResult(
                    new Protos.Response() { Succeed = orderResponse.Succeed, Response_ = orderResponse.Response_ 
                    });
            else if (!inventoryResponse.Succeed)
                return Task.FromResult(new Protos.Response() { Succeed = inventoryResponse.Succeed, Response_ = inventoryResponse.Response_ 
                });
            else
                return Task.FromResult( 
                    new Protos.Response() { Succeed = true, Response_ = "Congratolation" }
                    );
        }

    }
}
