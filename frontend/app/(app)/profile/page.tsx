"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";
import { api, ApiError, useSession } from "@/lib/api";
import type { Customer } from "@/lib/types";

export default function ProfilePage() {
  const router = useRouter();
  const { customer, setCustomer, logout } = useSession();
  const [form, setForm] = useState({ email: customer?.email ?? "", firstName: customer?.firstName ?? "", lastName: customer?.lastName ?? "", phone: customer?.phone ?? "" });
  const [notice, setNotice] = useState<{ kind: "ok" | "error"; text: string } | null>(null);
  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement>) => setForm({ ...form, [k]: e.target.value });

  const save = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      setCustomer(await api<Customer>("Customers/me", { method: "PUT", body: JSON.stringify(form) }));
      setNotice({ kind: "ok", text: "Profile saved." });
    } catch (err) {
      setNotice({ kind: "error", text: err instanceof ApiError ? err.messages.join(" ") : "Could not save." });
    }
  };

  const remove = async () => {
    if (!confirm("Delete your account? This cannot be undone.")) return;
    await api("Customers/me", { method: "DELETE" });
    logout();
    router.replace("/login");
  };

  return (
    <>
      <div className="page-head"><h1>Profile</h1></div>
      <form className="card form" onSubmit={save}>
        {notice && <div className={`notice ${notice.kind}`}>{notice.text}</div>}
        <div className="grid2">
          <label>First name<input required value={form.firstName} onChange={set("firstName")} /></label>
          <label>Last name<input required value={form.lastName} onChange={set("lastName")} /></label>
        </div>
        <label>Email<input type="email" required value={form.email} onChange={set("email")} /></label>
        <label>Phone<input value={form.phone} onChange={set("phone")} /></label>
        <div className="actions" style={{ justifyContent: "space-between" }}>
          <button className="btn">Save changes</button>
          <button type="button" className="btn danger" onClick={remove}>Delete account</button>
        </div>
      </form>
    </>
  );
}
