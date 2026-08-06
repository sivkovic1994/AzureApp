import { apiClient } from "./client";
import type { OrderDto } from "../types/api";

export interface CreateOrderRequest {
  customerId: string;
  currency: string;
}

export interface AddOrderItemRequest {
  productId: string;
  quantity: number;
}

export const ordersApi = {
  getAll: () => apiClient.get<OrderDto[]>("/api/orders"),
  getById: (orderId: string) => apiClient.get<OrderDto>(`/api/orders/${orderId}`),
  create: (request: CreateOrderRequest) => apiClient.post<OrderDto>("/api/orders", request),
  addItem: (orderId: string, request: AddOrderItemRequest) =>
    apiClient.post<OrderDto>(`/api/orders/${orderId}/items`, request),
  confirm: (orderId: string) => apiClient.post<OrderDto>(`/api/orders/${orderId}/confirm`),
  cancel: (orderId: string) => apiClient.post<OrderDto>(`/api/orders/${orderId}/cancel`),
};
