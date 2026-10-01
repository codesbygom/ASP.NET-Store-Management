// Shapes of the DTOs in backend/Store/Store.Application/DTOs.

export interface ApiEnvelope<T> {
  status: string;
  message: string;
  errors: string[];
  data: T;
}

export interface Paginated<T> {
  items: T[];
  totalRecords: number;
  page: number;
  pageSize: number;
}

export interface Category {
  id: number;
  title: string;
  description: string;
  totalProducts: number;
  createdAt: string;
  updatedAt: string;
}

export interface Product {
  id: number;
  title: string;
  description: string;
  price: number;
  stock: number;
  categoryId: number;
  category: string;
  createdAt: string;
  updatedAt: string;
}

export type OrderStatus = "Pending" | "Paid" | "Shipped" | "Delivered" | "Cancelled";
export const ORDER_STATUSES: OrderStatus[] = ["Pending", "Paid", "Shipped", "Delivered", "Cancelled"];

export interface OrderItem {
  productId: number;
  productTitle: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: number;
  customerId: number;
  status: OrderStatus;
  totalAmount: number;
  items: OrderItem[];
  createdAt: string;
  updatedAt: string;
}

export interface Customer {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  nickname: string;
}

export interface AuthResult {
  token: string;
  customer: Customer;
}
