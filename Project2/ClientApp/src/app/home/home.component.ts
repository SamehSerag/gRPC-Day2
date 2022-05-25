import { Component } from '@angular/core';
import { grpc } from '@improbable-eng/grpc-web';
import { Order, Product } from '../generated/src/app/protos/OrderProto_pb';
import { TrackingOrder } from '../generated/src/app/protos/OrderProto_pb_service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  logs: string[] = [];

  send() {
    var order = new Order();
    var product = new Product();
    product.setId(1);
    product.setQuantity(3);

    order.setUserid(1);
    order.setId(1);
    order.setTotalmoney(200);
    order.addProducts(product)

    grpc.unary(TrackingOrder.SendOrder, {
      request: order,
      host: "https://localhost:7233",
      onEnd: result => {
        console.log(result)
        const { status, message } = result;
        if (status == grpc.Code.OK && message) {
          this.logs.push(JSON.stringify(message.toObject()));
        }
      }
    });
  }
}
