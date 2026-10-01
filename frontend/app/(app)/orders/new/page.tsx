"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { api, ApiError, useSession } from "@/lib/api";
import { money } from "@/lib/format";
import type { Order } from "@/lib/types";

// The "cart": products added from the product list, turned into an order here.
export default function NewOrderPage() {
  const router = useRouter();
  const { cart, setQuantity, clearCart } = useSession();
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const total = cart.reduce((s, l) => s + l.price * l.quantity, 0);

  const place = async () => {
    setBusy(true);
    try {
      const order = await api<Order>("Orders", {
        method: "POST",
        body: JSON.stringify({ items: cart.map((l) => ({ productId: l.productId, quantity: l.quantity })) }),
      });
      clearCart();
      router.push(`/orders/${order.id}`);
    } catch (e) {
      setError(e instanceof ApiError ? e.messages.join(" ") : "Could not place the order.");
      setBusy(false);
    }
  };

  return (
    <>
      <div className="page-head"><div><h1>New order</h1><p>Review the items and place the order.</p></div></div>
      {error && <div className="notice error" style={{ marginBottom: "1rem" }}>{error}</div>}

      {cart.length === 0 ? (
        <div className="card empty">Your order is empty. <Link href="/products">Add products</Link></div>
      ) : (
        <div className="card" style={{ padding: 0 }}>
          <table>
            <thead><tr><th>Product</th><th className="num">Price</th><th className="num">Quantity</th><th className="num">Subtotal</th><th /></tr></thead>
            <tbody>
              {cart.map((l) => (
                <tr key={l.productId}>
                  <td><b>{l.title}</b></td>
                  <td className="num">{money(l.price)}</td>
                  <td className="num">
                    <input type="number" min={1} max={l.stock} value={l.quantity} style={{ width: 80 }} onChange={(e) => setQuantity(l.productId, Math.min(l.stock, Number(e.target.value) || 1))} />
                  </td>
                  <td className="num">{money(l.price * l.quantity)}</td>
                  <td><div className="actions"><button className="btn danger small" onClick={() => setQuantity(l.productId, 0)}>Remove</button></div></td>
                </tr>
              ))}
              <tr><td colSpan={3} className="num"><b>Total</b></td><td className="num"><b>{money(total)}</b></td><td /></tr>
            </tbody>
          </table>
          <div className="actions" style={{ padding: "1rem" }}>
            <button className="btn ghost" onClick={clearCart}>Clear</button>
            <button className="btn" disabled={busy} onClick={place}>{busy ? "Placing…" : "Place order"}</button>
          </div>
        </div>
      )}
    </>
  );
}
