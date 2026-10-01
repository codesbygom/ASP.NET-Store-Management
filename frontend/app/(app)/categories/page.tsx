"use client";

import { useEffect, useState } from "react";
import { api, ApiError } from "@/lib/api";
import type { Category } from "@/lib/types";

export default function CategoriesPage() {
  const [items, setItems] = useState<Category[]>([]);
  const [form, setForm] = useState<{ id?: number; title: string; description: string }>({ title: "", description: "" });
  const [error, setError] = useState("");

  const load = () => api<Category[]>("Categories").then(setItems).catch(() => {});
  useEffect(() => { load(); }, []);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api(form.id ? `Categories/${form.id}` : "Categories", { method: form.id ? "PUT" : "POST", body: JSON.stringify({ title: form.title, description: form.description }) });
      setForm({ title: "", description: "" });
      setError("");
      load();
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(" ") : "Could not save.");
    }
  };

  const remove = async (c: Category) => {
    if (!confirm(`Delete "${c.title}"?`)) return;
    try {
      await api(`Categories/${c.id}`, { method: "DELETE" });
      load();
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(" ") : "Could not delete.");
    }
  };

  return (
    <>
      <div className="page-head"><div><h1>Categories</h1><p>{items.length} categories</p></div></div>
      <div className="grid2" style={{ alignItems: "start" }}>
        <div className="card" style={{ padding: 0 }}>
          <table>
            <thead><tr><th>Category</th><th className="num">Products</th><th /></tr></thead>
            <tbody>
              {items.map((c) => (
                <tr key={c.id}>
                  <td><b>{c.title}</b><div style={{ color: "var(--muted)", fontSize: ".85rem" }}>{c.description}</div></td>
                  <td className="num">{c.totalProducts}</td>
                  <td>
                    <div className="actions">
                      <button className="btn ghost small" onClick={() => setForm({ id: c.id, title: c.title, description: c.description })}>Edit</button>
                      <button className="btn danger small" onClick={() => remove(c)}>Delete</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {items.length === 0 && <p className="empty">No categories yet.</p>}
        </div>

        <form className="card form wide" onSubmit={submit}>
          <h2>{form.id ? "Edit category" : "New category"}</h2>
          {error && <div className="notice error">{error}</div>}
          <label>Title<input required value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} /></label>
          <label>Description<textarea rows={3} value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} /></label>
          <div className="actions" style={{ justifyContent: "flex-start" }}>
            <button className="btn">{form.id ? "Save changes" : "Add category"}</button>
            {form.id && <button type="button" className="btn ghost" onClick={() => setForm({ title: "", description: "" })}>Cancel</button>}
          </div>
        </form>
      </div>
    </>
  );
}
