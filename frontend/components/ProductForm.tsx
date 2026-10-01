"use client";

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { api, ApiError } from "@/lib/api";
import type { Category, Product } from "@/lib/types";

// Create (no id) or edit (with id) a product.
export default function ProductForm({ id }: { id?: string }) {
  const router = useRouter();
  const [categories, setCategories] = useState<Category[]>([]);
  const [form, setForm] = useState({ title: "", description: "", price: "", stock: "0", categoryId: "" });
  const [error, setError] = useState("");
  const [loaded, setLoaded] = useState(!id);

  useEffect(() => {
    api<Category[]>("Categories").then(setCategories).catch(() => {});
    if (id) {
      api<Product>(`Products/${id}`).then((p) => {
        setForm({ title: p.title, description: p.description, price: String(p.price), stock: String(p.stock), categoryId: String(p.categoryId) });
        setLoaded(true);
      }).catch(() => router.replace("/products"));
    }
  }, [id, router]);

  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => setForm({ ...form, [k]: e.target.value });

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    const body = JSON.stringify({ title: form.title, description: form.description, price: Number(form.price), stock: Number(form.stock), categoryId: Number(form.categoryId) });
    try {
      await api(id ? `Products/${id}` : "Products", { method: id ? "PUT" : "POST", body });
      router.push("/products");
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(" ") : "Could not save.");
    }
  };

  if (!loaded) return <p>Loading…</p>;

  return (
    <>
      <div className="page-head"><h1>{id ? "Edit product" : "New product"}</h1></div>
      <form className="card form" onSubmit={submit}>
        {error && <div className="notice error">{error}</div>}
        <label>Title<input required value={form.title} onChange={set("title")} /></label>
        <label>Description<textarea rows={4} value={form.description} onChange={set("description")} /></label>
        <div className="grid2">
          <label>Price<input type="number" step="0.01" min="0.01" required value={form.price} onChange={set("price")} /></label>
          <label>Stock<input type="number" min="0" required value={form.stock} onChange={set("stock")} /></label>
        </div>
        <label>Category
          <select required value={form.categoryId} onChange={set("categoryId")}>
            <option value="">Choose…</option>
            {categories.map((c) => <option key={c.id} value={c.id}>{c.title}</option>)}
          </select>
        </label>
        <div className="actions" style={{ justifyContent: "flex-start" }}>
          <button className="btn">Save</button>
          <button type="button" className="btn ghost" onClick={() => router.push("/products")}>Cancel</button>
        </div>
      </form>
    </>
  );
}
