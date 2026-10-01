"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { ApiError, useSession } from "@/lib/api";

export default function RegisterPage() {
  const router = useRouter();
  const { register } = useSession();
  const [form, setForm] = useState({ firstName: "", lastName: "", email: "", phone: "", password: "" });
  const [error, setError] = useState("");
  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement>) => setForm({ ...form, [k]: e.target.value });

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await register(form);
      router.replace("/");
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(" ") : "Could not reach the server.");
    }
  };

  return (
    <div className="auth">
      <div className="card">
        <div className="brand" style={{ color: "var(--ink)", padding: 0 }}><i style={{ color: "#fff" }}>S</i> Store Management</div>
        <h1>Create account</h1>
        <form className="form" onSubmit={submit}>
          {error && <div className="notice error">{error}</div>}
          <div className="grid2">
            <label>First name<input required value={form.firstName} onChange={set("firstName")} /></label>
            <label>Last name<input required value={form.lastName} onChange={set("lastName")} /></label>
          </div>
          <label>Email<input type="email" required value={form.email} onChange={set("email")} /></label>
          <label>Phone<input value={form.phone} onChange={set("phone")} /></label>
          <label>Password (min 8 characters)<input type="password" required minLength={8} value={form.password} onChange={set("password")} /></label>
          <button className="btn">Create account</button>
        </form>
        <p className="alt">Already registered? <Link href="/login">Sign in</Link></p>
      </div>
    </div>
  );
}
