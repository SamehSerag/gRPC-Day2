// package: 
// file: src/app/protos/OrderProto.proto

var src_app_protos_OrderProto_pb = require("../../../src/app/protos/OrderProto_pb");
var grpc = require("@improbable-eng/grpc-web").grpc;

var TrackingOrder = (function () {
  function TrackingOrder() {}
  TrackingOrder.serviceName = "TrackingOrder";
  return TrackingOrder;
}());

TrackingOrder.SendOrder = {
  methodName: "SendOrder",
  service: TrackingOrder,
  requestStream: false,
  responseStream: false,
  requestType: src_app_protos_OrderProto_pb.Order,
  responseType: src_app_protos_OrderProto_pb.Response
};

exports.TrackingOrder = TrackingOrder;

function TrackingOrderClient(serviceHost, options) {
  this.serviceHost = serviceHost;
  this.options = options || {};
}

TrackingOrderClient.prototype.sendOrder = function sendOrder(requestMessage, metadata, callback) {
  if (arguments.length === 2) {
    callback = arguments[1];
  }
  var client = grpc.unary(TrackingOrder.SendOrder, {
    request: requestMessage,
    host: this.serviceHost,
    metadata: metadata,
    transport: this.options.transport,
    debug: this.options.debug,
    onEnd: function (response) {
      if (callback) {
        if (response.status !== grpc.Code.OK) {
          var err = new Error(response.statusMessage);
          err.code = response.status;
          err.metadata = response.trailers;
          callback(err, null);
        } else {
          callback(null, response.message);
        }
      }
    }
  });
  return {
    cancel: function () {
      callback = null;
      client.close();
    }
  };
};

exports.TrackingOrderClient = TrackingOrderClient;

