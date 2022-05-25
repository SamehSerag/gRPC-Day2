using PaymentDemo.Protos;
using Grpc.Core;
using PaymentDemo.Model;

namespace PaymentService.Service
{
    public class Payment: PaymentProto.PaymentProtoBase
    {
        public static List<User> UsersList = new List<User>()
        {
            new User(1,"sameh", 5000),
            new User(2,"serag", 2000),
            new User(3,"mohamed", 3000),
        };
        public override Task<Response> ApplyPayment(OrderMassage request, ServerCallContext context)
        {
            var user = UsersList.Where(u => u.Id == request.UserId).FirstOrDefault();
            Response response = new Response() {Succeed = false};
           
            if (user == null)
            {
                response.Response_ = "User Not Found";
            }
            else if (user.Balance < request.TotalMoney)
            {
                response.Response_ = "Balance not enough";
            }
            else
            {
                user.Balance -= request.TotalMoney;
                response.Succeed = true;
                response.Response_ = "Done";
            }



            return Task.FromResult(response);
        }
    }
}
