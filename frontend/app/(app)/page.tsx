"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { money, formatDate } from "@/lib/format";
import { api, useSession } from "@/lib/api";
import type { Category, Order, Paginated, Product } from "@/lib/types";

export default function DashboardPage() {
  const { customer } = useSession();
  const [products, setProducts] = useState<Paginated<Product> | null>(null);
  const [categories, setCategories] = useState<Category[]>([]);
  const [orders, setOrders] = useState<Order[]>([]);
  const [lowStock, setLowStock] = useState<Product[]>([]);

  useEffect(() => {
    api<Paginated<Product>>("Products?page=1&pageSize=100").then((p) => {
      setProducts(p);
      setLowStock(p.items.filter((x) => x.stock <= 5).sort((a, b) => a.stock - b.stock).slice(0, 5));
    }).catch(() => {});
    api<Category[]>("Categories").then(setCategories).catch(() => {});
    api<Order[]>("Orders").then(setOrders).catch(() => {});
  }, []);

  const revenue = orders.filter((o) => o.status !== "Cancelled").reduce((s, o) => s + o.totalAmount, 0);

  return (
    <>
      <div className="page-head">
        <div><h1>Welcome back, {customer?.firstName}</h1><p>Here is what is happening in your store.</p></div>
        <Link className="btn" href="/products/new">+ New product</Link>
      </div>

      <div className="stats">
        <div className="card stat"><b>{products?.totalRecords ?? "–"}</b><span>Products</span></div>
        <div className="card stat"><b>{categories.length}</b><span>Categories</span></div>
        <div className="card stat"><b>{orders.length}</b><span>My orders</span></div>
        <div className="card stat"><b>{money(revenue)}</b><span>Order value</span></div>
      </div>

      <div className="grid2">
        <div className="card">
          <h2 style={{ marginBottom: ".6rem" }}>Recent orders</h2>
          {orders.length === 0 ? <p className="empty">No orders yet.</p> : (
            <table>
              <thead><tr><th>#</th><th>Placed</th><th>Status</th><th className="num">Total</th></tr></thead>
              <tbody>
                {orders.slice(0, 5).map((o) => (
                  <tr key={o.id}>
                    <td><Link href={`/orders/${o.id}`}>#{o.id}</Link></td>
                    <td>{formatDate(o.createdAt)}</td>
                    <td><span className={`pill ${o.status}`}>{o.status}</span></td>
                    <td className="num">{money(o.totalAmount)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
        <div className="card">
          <h2 style={{ marginBottom: ".6rem" }}>Low stock</h2>
          {lowStock.length === 0 ? <p className="empty">Everything is well stocked.</p> : (
            <table>
              <thead><tr><th>Product</th><th className="num">In stock</th></tr></thead>
              <tbody>
                {lowStock.map((p) => (
                  <tr key={p.id}>
                    <td><Link href={`/products/${p.id}`}>{p.title}</Link></td>
                    <td className={`num ${p.stock === 0 ? "stock-out" : "stock-low"}`}>{p.stock}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </>
  );
}
