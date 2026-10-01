"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import { api, ApiError } from "@/lib/api";
import { formatDate, money } from "@/lib/format";
import { ORDER_STATUSES, type Order, type OrderStatus } from "@/lib/types";

export default function OrderDetailPage() {
  const { id } = useParams<{ id: string }>();
  const [order, setOrder] = useState<Order | null>(null);
  const [notice, setNotice] = useState<{ kind: "ok" | "error"; text: string } | null>(null);

  useEffect(() => { api<Order>(`Orders/${id}`).then(setOrder).catch(() => setNotice({ kind: "error", text: "Order not found." })); }, [id]);

  const setStatus = async (status: OrderStatus) => {
    try {
      setOrder(await api<Order>(`Orders/${id}/status`, { method: "PUT", body: JSON.stringify({ status }) }));
      setNotice({ kind: "ok", text: `Status changed to ${status}.` });
    } catch (e) {
      setNotice({ kind: "error", text: e instanceof ApiError ? e.messages.join(" ") : "Could not update the status." });
    }
  };

  if (!order) return notice ? <div className="notice error">{notice.text}</div> : <p>Loading…</p>;

  return (
    <>
      <div className="page-head">
        <div><h1>Order #{order.id}</h1><p>Placed {formatDate(order.createdAt)}</p></div>
        <Link className="btn ghost" href="/orders">← All orders</Link>
      </div>
      {notice && <div className={`notice ${notice.kind}`} style={{ marginBottom: "1rem" }}>{notice.text}</div>}

      <div className="card" style={{ marginBottom: "1rem", display: "flex", gap: "1rem", alignItems: "center", flexWrap: "wrap" }}>
        <span className={`pill ${order.status}`}>{order.status}</span>
        <label style={{ display: "flex", alignItems: "center", gap: ".6rem" }}>
          Change status
          <select value={order.status} onChange={(e) => setStatus(e.target.value as OrderStatus)}>
            {ORDER_STATUSES.map((s) => <option key={s}>{s}</option>)}
          </select>
        </label>
      </div>

      <div className="card" style={{ padding: 0 }}>
        <table>
          <thead><tr><th>Product</th><th className="num">Unit price</th><th className="num">Qty</th><th className="num">Subtotal</th></tr></thead>
          <tbody>
            {order.items.map((i) => (
              <tr key={i.productId}>
                <td><Link href={`/products/${i.productId}`}>{i.productTitle}</Link></td>
                <td className="num">{money(i.unitPrice)}</td>
                <td className="num">{i.quantity}</td>
                <td className="num">{money(i.unitPrice * i.quantity)}</td>
              </tr>
            ))}
            <tr><td colSpan={3} className="num"><b>Total</b></td><td className="num"><b>{money(order.totalAmount)}</b></td></tr>
          </tbody>
        </table>
      </div>
    </>
  );
}
