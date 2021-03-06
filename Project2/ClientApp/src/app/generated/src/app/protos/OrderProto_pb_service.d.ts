// package: 
// file: src/app/protos/OrderProto.proto

import * as src_app_protos_OrderProto_pb from "../../../src/app/protos/OrderProto_pb";
import {grpc} from "@improbable-eng/grpc-web";

type TrackingOrderSendOrder = {
  readonly methodName: string;
  readonly service: typeof TrackingOrder;
  readonly requestStream: false;
  readonly responseStream: false;
  readonly requestType: typeof src_app_protos_OrderProto_pb.Order;
  readonly responseType: typeof src_app_protos_OrderProto_pb.Response;
};

export class TrackingOrder {
  static readonly serviceName: string;
  static readonly SendOrder: TrackingOrderSendOrder;
}

export type ServiceError = { message: string, code: number; metadata: grpc.Metadata }
export type Status = { details: string, code: number; metadata: grpc.Metadata }

interface UnaryResponse {
  cancel(): void;
}
interface ResponseStream<T> {
  cancel(): void;
  on(type: 'data', handler: (message: T) => void): ResponseStream<T>;
  on(type: 'end', handler: (status?: Status) => void): ResponseStream<T>;
  on(type: 'status', handler: (status: Status) => void): ResponseStream<T>;
}
interface RequestStream<T> {
  write(message: T): RequestStream<T>;
  end(): void;
  cancel(): void;
  on(type: 'end', handler: (status?: Status) => void): RequestStream<T>;
  on(type: 'status', handler: (status: Status) => void): RequestStream<T>;
}
interface BidirectionalStream<ReqT, ResT> {
  write(message: ReqT): BidirectionalStream<ReqT, ResT>;
  end(): void;
  cancel(): void;
  on(type: 'data', handler: (message: ResT) => void): BidirectionalStream<ReqT, ResT>;
  on(type: 'end', handler: (status?: Status) => void): BidirectionalStream<ReqT, ResT>;
  on(type: 'status', handler: (status: Status) => void): BidirectionalStream<ReqT, ResT>;
}

export class TrackingOrderClient {
  readonly serviceHost: string;

  constructor(serviceHost: string, options?: grpc.RpcOptions);
  sendOrder(
    requestMessage: src_app_protos_OrderProto_pb.Order,
    metadata: grpc.Metadata,
    callback: (error: ServiceError|null, responseMessage: src_app_protos_OrderProto_pb.Response|null) => void
  ): UnaryResponse;
  sendOrder(
    requestMessage: src_app_protos_OrderProto_pb.Order,
    callback: (error: ServiceError|null, responseMessage: src_app_protos_OrderProto_pb.Response|null) => void
  ): UnaryResponse;
}

