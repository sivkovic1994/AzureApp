import { useEffect, useState } from "react";
import { productsApi } from "../api/products";
import { ApiError } from "../api/client";
import { ErrorBanner } from "../components/ErrorBanner";
import { formatMoney } from "../utils/format";
import type { ProductDto } from "../types/api";

const emptyForm = { name: "", sku: "", price: "", currency: "EUR", initialStock: "" };

export function ProductsPage() {
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [submitting, setSubmitting] = useState(false);

  async function loadProducts() {
    setLoading(true);
    try {
      setProducts(await productsApi.getAll());
      setError(null);
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to load products.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadProducts();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSubmitting(true);
    setError(null);
    try {
      await productsApi.create({
        name: form.name,
        sku: form.sku,
        price: Number(form.price),
        currency: form.currency,
        initialStock: Number(form.initialStock),
      });
      setForm(emptyForm);
      await loadProducts();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to create product.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>Products</h1>
          <p>Manage the catalog and stock levels.</p>
        </div>
      </div>

      <ErrorBanner message={error} />

      <div className="card">
        <h2>Add product</h2>
        <form className="form-grid" onSubmit={handleSubmit} style={{ gridTemplateColumns: "2fr 1fr 1fr 1fr auto", alignItems: "end", display: "grid" }}>
          <div>
            <label htmlFor="name">Name</label>
            <input id="name" required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
          </div>
          <div>
            <label htmlFor="sku">SKU</label>
            <input id="sku" required value={form.sku} onChange={(e) => setForm({ ...form, sku: e.target.value })} />
          </div>
          <div>
            <label htmlFor="price">Price</label>
            <input id="price" type="number" min="0" step="0.01" required value={form.price} onChange={(e) => setForm({ ...form, price: e.target.value })} />
          </div>
          <div>
            <label htmlFor="stock">Initial stock</label>
            <input id="stock" type="number" min="0" required value={form.initialStock} onChange={(e) => setForm({ ...form, initialStock: e.target.value })} />
          </div>
          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Adding…" : "Add"}
          </button>
        </form>
      </div>

      <div className="card">
        <h2>Catalog</h2>
        {loading ? (
          <div className="empty-state">Loading…</div>
        ) : products.length === 0 ? (
          <div className="empty-state">No products yet — add one above.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>SKU</th>
                <th className="text-right">Price</th>
                <th className="text-right">Stock</th>
              </tr>
            </thead>
            <tbody>
              {products.map((p) => (
                <tr key={p.id}>
                  <td>{p.name}</td>
                  <td className="muted">{p.sku}</td>
                  <td className="text-right">{formatMoney(p.price, p.currency)}</td>
                  <td className="text-right">{p.stockQuantity}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
