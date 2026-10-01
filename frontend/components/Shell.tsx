"use client";

import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useEffect } from "react";
import { useSession } from "@/lib/api";

const NAV = [
  { href: "/", label: "Dashboard", exact: true },
  { href: "/products", label: "Products" },
  { href: "/categories", label: "Categories" },
  { href: "/orders", label: "Orders" },
  { href: "/profile", label: "Profile" },
];

// Sidebar + content area; every page inside needs a signed-in customer.
export default function Shell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const router = useRouter();
  const { ready, customer, cart, logout } = useSession();

  useEffect(() => {
    if (ready && !customer) router.replace("/login");
  }, [ready, customer, router]);

  if (!customer) return <div className="auth"><span>Loading…</span></div>;

  const count = cart.reduce((n, l) => n + l.quantity, 0);

  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="brand"><i>S</i> Store Management</div>
        <nav className="nav">
          {NAV.map((item) => (
            <Link key={item.href} href={item.href} className={(item.exact ? pathname === item.href : pathname.startsWith(item.href)) ? "active" : ""}>
              {item.label}
            </Link>
          ))}
          <Link href="/orders/new" className={pathname === "/orders/new" ? "active" : ""}>
            New order {count > 0 && <span className="badge-count">{count}</span>}
          </Link>
        </nav>
        <div className="userbox">
          <b>{customer.firstName} {customer.lastName}</b>
          {customer.email}
          <div style={{ marginTop: ".6rem" }}>
            <button className="btn ghost small" onClick={() => { logout(); router.push("/login"); }}>Log out</button>
          </div>
        </div>
      </aside>
      <main className="main">{children}</main>
    </div>
  );
}
