"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { formatDate, money } from "@/lib/format";
import type { Order } from "@/lib/types";

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[] | null>(null);
  useEffect(() => { api<Order[]>("Orders").then(setOrders).catch(() => setOrders([])); }, []);

  return (
    <>
      <div className="page-head">
        <div><h1>Orders</h1><p>{orders ? `${orders.length} orders` : "Loading…"}</p></div>
        <Link className="btn" href="/orders/new">+ New order</Link>
      </div>
      <div className="card" style={{ padding: 0 }}>
        <table>
          <thead><tr><th>Order</th><th>Placed</th><th>Items</th><th>Status</th><th className="num">Total</th><th /></tr></thead>
          <tbody>
            {orders?.map((o) => (
              <tr key={o.id}>
                <td><b>#{o.id}</b></td>
                <td>{formatDate(o.createdAt)}</td>
                <td>{o.items.reduce((n, i) => n + i.quantity, 0)}</td>
                <td><span className={`pill ${o.status}`}>{o.status}</span></td>
                <td className="num">{money(o.totalAmount)}</td>
                <td><div className="actions"><Link className="btn ghost small" href={`/orders/${o.id}`}>View</Link></div></td>
              </tr>
            ))}
          </tbody>
        </table>
        {orders?.length === 0 && <p className="empty">No orders yet.</p>}
      </div>
    </>
  );
}
