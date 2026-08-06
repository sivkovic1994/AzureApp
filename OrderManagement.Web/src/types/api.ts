export interface CustomerDto {
  id: string;
  name: string;
  email: string;
}

export interface ProductDto {
  id: string;
  name: string;
  sku: string;
  price: number;
  currency: string;
  stockQuantity: number;
}

export interface OrderItemDto {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export type OrderStatus = "Pending" | "Confirmed" | "Shipped" | "Cancelled";

export interface OrderDto {
  id: string;
  customerId: string;
  status: OrderStatus;
  createdOn: string;
  currency: string;
  totalAmount: number;
  items: OrderItemDto[];
}

export interface ApiProblem {
  title: string;
  status: number;
  detail: string;
}
