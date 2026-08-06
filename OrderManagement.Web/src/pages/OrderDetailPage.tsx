import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { ordersApi } from "../api/orders";
import { productsApi } from "../api/products";
import { ApiError } from "../api/client";
import { ErrorBanner } from "../components/ErrorBanner";
import { StatusBadge } from "../components/StatusBadge";
import { formatDate, formatMoney } from "../utils/format";
import type { OrderDto, ProductDto } from "../types/api";

export function OrderDetailPage() {
  const { orderId } = useParams<{ orderId: string }>();
  const [order, setOrder] = useState<OrderDto | null>(null);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [productId, setProductId] = useState("");
  const [quantity, setQuantity] = useState("1");
  const [actionPending, setActionPending] = useState(false);

  async function loadOrder() {
    if (!orderId) return;
    setLoading(true);
    try {
      const [orderResult, productsResult] = await Promise.all([ordersApi.getById(orderId), productsApi.getAll()]);
      setOrder(orderResult);
      setProducts(productsResult);
      setError(null);
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to load order.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadOrder();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [orderId]);

  async function handleAddItem(e: React.FormEvent) {
    e.preventDefault();
    if (!orderId || !productId) return;
    setActionPending(true);
    setError(null);
    try {
      await ordersApi.addItem(orderId, { productId, quantity: Number(quantity) });
      setProductId("");
      setQuantity("1");
      await loadOrder();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to add item.");
    } finally {
      setActionPending(false);
    }
  }

  async function handleConfirm() {
    if (!orderId) return;
    setActionPending(true);
    setError(null);
    try {
      await ordersApi.confirm(orderId);
      await loadOrder();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to confirm order.");
    } finally {
      setActionPending(false);
    }
  }

  async function handleCancel() {
    if (!orderId) return;
    setActionPending(true);
    setError(null);
    try {
      await ordersApi.cancel(orderId);
      await loadOrder();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to cancel order.");
    } finally {
      setActionPending(false);
    }
  }

  if (loading) return <div className="empty-state">Loading…</div>;
  if (!order) return <ErrorBanner message={error ?? "Order not found."} />;

  const isPending = order.status === "Pending";

  return (
    <div>
      <div className="page-header">
        <div>
          <Link to="/orders" className="link-button">
            ← Back to orders
          </Link>
          <h1 style={{ marginTop: 8 }}>Order details</h1>
          <p>
            <StatusBadge status={order.status} /> · created {formatDate(order.createdOn)}
          </p>
        </div>
        {isPending && (
          <div className="btn-row">
            <button className="btn-secondary" onClick={handleCancel} disabled={actionPending}>
              Cancel order
            </button>
            <button className="btn-primary" onClick={handleConfirm} disabled={actionPending || order.items.length === 0}>
              Confirm order
            </button>
          </div>
        )}
      </div>

      <ErrorBanner message={error} />

      <div className="card">
        <h2>Items</h2>
        {order.items.length === 0 ? (
          <div className="empty-state">No items yet.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Product</th>
                <th className="text-right">Unit price</th>
                <th className="text-right">Quantity</th>
                <th className="text-right">Line total</th>
              </tr>
            </thead>
            <tbody>
              {order.items.map((item) => (
                <tr key={item.productId}>
                  <td>{item.productName}</td>
                  <td className="text-right">{formatMoney(item.unitPrice, order.currency)}</td>
                  <td className="text-right">{item.quantity}</td>
                  <td className="text-right">{formatMoney(item.lineTotal, order.currency)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
        <div className="text-right" style={{ marginTop: 12, fontWeight: 700 }}>
          Total: {formatMoney(order.totalAmount, order.currency)}
        </div>
      </div>

      {isPending && (
        <div className="card">
          <h2>Add item</h2>
          <form onSubmit={handleAddItem} style={{ display: "grid", gridTemplateColumns: "2fr 1fr auto", gap: 14, alignItems: "end" }}>
            <div>
              <label htmlFor="product">Product</label>
              <select id="product" required value={productId} onChange={(e) => setProductId(e.target.value)}>
                <option value="">Select a product…</option>
                {products.map((p) => (
                  <option key={p.id} value={p.id}>
                    {p.name} — {formatMoney(p.price, p.currency)} ({p.stockQuantity} in stock)
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label htmlFor="quantity">Quantity</label>
              <input id="quantity" type="number" min="1" required value={quantity} onChange={(e) => setQuantity(e.target.value)} />
            </div>
            <button type="submit" className="btn-primary" disabled={actionPending}>
              Add item
            </button>
          </form>
        </div>
      )}
    </div>
  );
}
