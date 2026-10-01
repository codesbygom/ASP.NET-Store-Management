"use client";

import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import { api, ApiError, useSession } from "@/lib/api";
import { money } from "@/lib/format";
import type { Category, Paginated, Product } from "@/lib/types";

const PAGE_SIZE = 10;

export default function ProductsPage() {
  const { addToCart } = useSession();
  const [categories, setCategories] = useState<Category[]>([]);
  const [data, setData] = useState<Paginated<Product> | null>(null);
  const [filters, setFilters] = useState({ search: "", categoryId: "", minPrice: "", maxPrice: "" });
  const [page, setPage] = useState(1);
  const [error, setError] = useState("");

  const load = useCallback(() => {
    const q = new URLSearchParams({ page: String(page), pageSize: String(PAGE_SIZE) });
    Object.entries(filters).forEach(([k, v]) => v && q.set(k, v));
    api<Paginated<Product>>(`Products?${q}`).then(setData).catch((e) => setError(e instanceof ApiError ? e.messages.join(" ") : "Could not load products."));
  }, [filters, page]);

  useEffect(() => { api<Category[]>("Categories").then(setCategories).catch(() => {}); }, []);
  useEffect(load, [load]);

  const remove = async (p: Product) => {
    if (!confirm(`Delete "${p.title}"?`)) return;
    try {
      await api(`Products/${p.id}`, { method: "DELETE" });
      load();
    } catch (e) {
      setError(e instanceof ApiError ? e.messages.join(" ") : "Could not delete.");
    }
  };

  const pages = data ? Math.max(1, Math.ceil(data.totalRecords / PAGE_SIZE)) : 1;
  const set = (k: keyof typeof filters) => (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => { setPage(1); setFilters({ ...filters, [k]: e.target.value }); };

  return (
    <>
      <div className="page-head">
        <div><h1>Products</h1><p>{data ? `${data.totalRecords} products` : "Loading…"}</p></div>
        <Link className="btn" href="/products/new">+ New product</Link>
      </div>

      <div className="toolbar">
        <input placeholder="Search…" value={filters.search} onChange={set("search")} style={{ flex: 1, minWidth: 180 }} />
        <select value={filters.categoryId} onChange={set("categoryId")}>
          <option value="">All categories</option>
          {categories.map((c) => <option key={c.id} value={c.id}>{c.title}</option>)}
        </select>
        <input type="number" min={0} placeholder="Min price" value={filters.minPrice} onChange={set("minPrice")} style={{ width: 120 }} />
        <input type="number" min={0} placeholder="Max price" value={filters.maxPrice} onChange={set("maxPrice")} style={{ width: 120 }} />
      </div>

      {error && <div className="notice error" style={{ marginBottom: "1rem" }}>{error}</div>}

      <div className="card" style={{ padding: 0 }}>
        <table>
          <thead><tr><th>Product</th><th>Category</th><th className="num">Price</th><th className="num">Stock</th><th /></tr></thead>
          <tbody>
            {data?.items.map((p) => (
              <tr key={p.id}>
                <td><Link href={`/products/${p.id}`}><b>{p.title}</b></Link></td>
                <td>{p.category}</td>
                <td className="num">{money(p.price)}</td>
                <td className={`num ${p.stock === 0 ? "stock-out" : p.stock <= 5 ? "stock-low" : ""}`}>{p.stock}</td>
                <td>
                  <div className="actions">
                    <button className="btn ghost small" disabled={p.stock === 0} onClick={() => addToCart({ productId: p.id, title: p.title, price: p.price, stock: p.stock })}>Add to order</button>
                    <Link className="btn ghost small" href={`/products/${p.id}`}>Edit</Link>
                    <button className="btn danger small" onClick={() => remove(p)}>Delete</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {data && data.items.length === 0 && <p className="empty">No products match your filters.</p>}
      </div>

      <div className="pager">
        <span>Page {page} of {pages}</span>
        <div className="actions">
          <button className="btn ghost small" disabled={page <= 1} onClick={() => setPage(page - 1)}>Previous</button>
          <button className="btn ghost small" disabled={page >= pages} onClick={() => setPage(page + 1)}>Next</button>
        </div>
      </div>
    </>
  );
}
