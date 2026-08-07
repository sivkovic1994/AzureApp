import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ordersApi } from "../api/orders";
import { customersApi } from "../api/customers";
import { ApiError } from "../api/client";
import { ErrorBanner } from "../components/ErrorBanner";
import { StatusBadge } from "../components/StatusBadge";
import { formatDate, formatMoney } from "../utils/format";
import type { CustomerDto, OrderDto } from "../types/api";

export function OrdersPage() {
  const [orders, setOrders] = useState<OrderDto[]>([]);
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [customerId, setCustomerId] = useState("");
  const [submitting, setSubmitting] = useState(false);

  async function loadData() {
    setLoading(true);
    try {
      const [ordersResult, customersResult] = await Promise.all([ordersApi.getAll(), customersApi.getAll()]);
      setOrders(ordersResult);
      setCustomers(customersResult);
      setError(null);
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to load orders.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadData();
  }, []);

  async function handleCreate(e: React.FormEvent) {
    e.preventDefault();
    if (!customerId) return;
    setSubmitting(true);
    setError(null);
    try {
      await ordersApi.create({ customerId, currency: "RSD" });
      setCustomerId("");
      await loadData();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to create order.");
    } finally {
      setSubmitting(false);
    }
  }

  const customerName = (id: string) => customers.find((c) => c.id === id)?.name ?? id;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>Orders</h1>
          <p>Create orders and track them through their lifecycle.</p>
        </div>
      </div>

      <ErrorBanner message={error} />

      <div className="card">
        <h2>New order</h2>
        {customers.length === 0 ? (
          <p className="muted">Add a customer first before creating an order.</p>
        ) : (
          <form onSubmit={handleCreate} style={{ display: "grid", gridTemplateColumns: "1fr auto", gap: 14, alignItems: "end" }}>
            <div>
              <label htmlFor="customer">Customer</label>
              <select id="customer" required value={customerId} onChange={(e) => setCustomerId(e.target.value)}>
                <option value="">Select a customer…</option>
                {customers.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name} ({c.email})
                  </option>
                ))}
              </select>
            </div>
            <button type="submit" className="btn-primary" disabled={submitting}>
              {submitting ? "Creating…" : "Create order"}
            </button>
          </form>
        )}
      </div>

      <div className="card">
        <h2>All orders</h2>
        {loading ? (
          <div className="empty-state">Loading…</div>
        ) : orders.length === 0 ? (
          <div className="empty-state">No orders yet.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Customer</th>
                <th>Status</th>
                <th>Created</th>
                <th className="text-right">Total</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {orders.map((o) => (
                <tr key={o.id}>
                  <td>{customerName(o.customerId)}</td>
                  <td>
                    <StatusBadge status={o.status} />
                  </td>
                  <td className="muted">{formatDate(o.createdOn)}</td>
                  <td className="text-right">{formatMoney(o.totalAmount, o.currency)}</td>
                  <td className="text-right">
                    <Link className="link-button" to={`/orders/${o.id}`}>
                      View →
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
