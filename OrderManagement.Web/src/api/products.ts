import { apiClient } from "./client";
import type { ProductDto } from "../types/api";

export interface CreateProductRequest {
  name: string;
  sku: string;
  price: number;
  currency: string;
  initialStock: number;
}

export const productsApi = {
  getAll: () => apiClient.get<ProductDto[]>("/api/products"),
  create: (request: CreateProductRequest) => apiClient.post<ProductDto>("/api/products", request),
};
