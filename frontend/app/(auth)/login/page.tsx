"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { ApiError, useSession } from "@/lib/api";

export default function LoginPage() {
  const router = useRouter();
  const { login } = useSession();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await login(email, password);
      router.replace("/");
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(" ") : "Could not reach the server.");
    }
  };

  return (
    <div className="auth">
      <div className="card">
        <div className="brand" style={{ color: "var(--ink)", padding: 0 }}><i style={{ color: "#fff" }}>S</i> Store Management</div>
        <h1>Sign in</h1>
        <form className="form" onSubmit={submit}>
          {error && <div className="notice error">{error}</div>}
          <label>Email<input type="email" required value={email} onChange={(e) => setEmail(e.target.value)} autoComplete="email" /></label>
          <label>Password<input type="password" required value={password} onChange={(e) => setPassword(e.target.value)} autoComplete="current-password" /></label>
          <button className="btn">Sign in</button>
        </form>
        <p className="alt">No account? <Link href="/register">Create one</Link></p>
      </div>
    </div>
  );
}
