import type { OrderStatus } from "../types/api";

const classByStatus: Record<OrderStatus, string> = {
  Pending: "badge-pending",
  Confirmed: "badge-confirmed",
  Shipped: "badge-shipped",
  Cancelled: "badge-cancelled",
};

export function StatusBadge({ status }: { status: OrderStatus }) {
  return <span className={`badge ${classByStatus[status]}`}>{status}</span>;
}
