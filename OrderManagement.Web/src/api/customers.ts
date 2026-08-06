import { apiClient } from "./client";
import type { CustomerDto } from "../types/api";

export interface CreateCustomerRequest {
  name: string;
  email: string;
}

export const customersApi = {
  getAll: () => apiClient.get<CustomerDto[]>("/api/customers"),
  create: (request: CreateCustomerRequest) => apiClient.post<CustomerDto>("/api/customers", request),
};
