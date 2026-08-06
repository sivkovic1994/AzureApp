import { useEffect, useState } from "react";
import { customersApi } from "../api/customers";
import { ApiError } from "../api/client";
import { ErrorBanner } from "../components/ErrorBanner";
import type { CustomerDto } from "../types/api";

const emptyForm = { name: "", email: "" };

export function CustomersPage() {
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [submitting, setSubmitting] = useState(false);

  async function loadCustomers() {
    setLoading(true);
    try {
      setCustomers(await customersApi.getAll());
      setError(null);
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to load customers.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadCustomers();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSubmitting(true);
    setError(null);
    try {
      await customersApi.create(form);
      setForm(emptyForm);
      await loadCustomers();
    } catch (err) {
      setError(err instanceof ApiError ? err.message : "Failed to create customer.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>Customers</h1>
          <p>People who can place orders.</p>
        </div>
      </div>

      <ErrorBanner message={error} />

      <div className="card">
        <h2>Add customer</h2>
        <form onSubmit={handleSubmit} style={{ display: "grid", gridTemplateColumns: "1fr 1fr auto", gap: 14, alignItems: "end" }}>
          <div>
            <label htmlFor="name">Name</label>
            <input id="name" required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
          </div>
          <div>
            <label htmlFor="email">Email</label>
            <input id="email" type="email" required value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          </div>
          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Adding…" : "Add"}
          </button>
        </form>
      </div>

      <div className="card">
        <h2>All customers</h2>
        {loading ? (
          <div className="empty-state">Loading…</div>
        ) : customers.length === 0 ? (
          <div className="empty-state">No customers yet — add one above.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
              </tr>
            </thead>
            <tbody>
              {customers.map((c) => (
                <tr key={c.id}>
                  <td>{c.name}</td>
                  <td className="muted">{c.email}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
