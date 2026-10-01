"use client";

import { createContext, useCallback, useContext, useEffect, useMemo, useState } from "react";
import type { ApiEnvelope, AuthResult, Customer } from "./types";

const TOKEN = "store_token";
const CART = "store_cart";

export class ApiError extends Error {
  constructor(public status: number, public messages: string[]) {
    super(messages[0] ?? `Request failed with ${status}`);
  }
}

function read(key: string): string | null {
  try {
    return localStorage.getItem(key);
  } catch {
    return null;
  }
}

function write(key: string, value: string | null) {
  try {
    if (value) localStorage.setItem(key, value);
    else localStorage.removeItem(key);
  } catch {
    /* storage blocked: state just won't survive a reload */
  }
}

// Flattens the API's Response<T> errors and ASP.NET validation ProblemDetails.
function messagesFrom(body: unknown): string[] {
  const b = body as { errors?: unknown; message?: string; title?: string } | null;
  if (!b) return ["Request failed."];
  if (Array.isArray(b.errors) && b.errors.length) return b.errors.map(String);
  if (b.errors && typeof b.errors === "object") return Object.values(b.errors as Record<string, string[]>).flat();
  return [b.message ?? b.title ?? "Request failed."];
}

// Calls the .NET API through the Next proxy and unwraps Response<T>.data.
export async function api<T = unknown>(path: string, init: RequestInit = {}): Promise<T> {
  const headers = new Headers(init.headers);
  const token = read(TOKEN);
  if (token) headers.set("Authorization", `Bearer ${token}`);
  if (init.body) headers.set("Content-Type", "application/json");
  const res = await fetch(`/api/${path}`, { ...init, headers });
  const text = await res.text();
  const body = text ? JSON.parse(text) : null;
  if (res.status === 401) window.dispatchEvent(new Event("store:unauthorized"));
  if (!res.ok) throw new ApiError(res.status, messagesFrom(body));
  return (body as ApiEnvelope<T>)?.data;
}

export interface CartLine {
  productId: number;
  title: string;
  price: number;
  stock: number;
  quantity: number;
}

interface Session {
  ready: boolean;
  customer: Customer | null;
  cart: CartLine[];
  login: (email: string, password: string) => Promise<void>;
  register: (input: Record<string, string>) => Promise<void>;
  logout: () => void;
  setCustomer: (c: Customer) => void;
  addToCart: (line: Omit<CartLine, "quantity">) => void;
  setQuantity: (productId: number, quantity: number) => void;
  clearCart: () => void;
}

const Ctx = createContext<Session | null>(null);

export function SessionProvider({ children }: { children: React.ReactNode }) {
  const [ready, setReady] = useState(false);
  const [customer, setCustomerState] = useState<Customer | null>(null);
  const [cart, setCart] = useState<CartLine[]>([]);

  const persist = (next: CartLine[]) => {
    setCart(next);
    write(CART, next.length ? JSON.stringify(next) : null);
  };

  const logout = useCallback(() => {
    write(TOKEN, null);
    setCustomerState(null);
  }, []);

  useEffect(() => {
    try {
      setCart(JSON.parse(read(CART) ?? "[]"));
    } catch {
      setCart([]);
    }
    if (read(TOKEN)) {
      api<Customer>("Customers/me").then(setCustomerState).catch(() => write(TOKEN, null)).finally(() => setReady(true));
    } else {
      setReady(true);
    }
    const onUnauthorized = () => logout();
    window.addEventListener("store:unauthorized", onUnauthorized);
    return () => window.removeEventListener("store:unauthorized", onUnauthorized);
  }, [logout]);

  const authenticate = async (path: string, body: object) => {
    const result = await api<AuthResult>(path, { method: "POST", body: JSON.stringify(body) });
    write(TOKEN, result.token);
    setCustomerState(result.customer);
  };

  const value = useMemo<Session>(
    () => ({
      ready,
      customer,
      cart,
      login: (email, password) => authenticate("Auth/login", { email, password }),
      register: (input) => authenticate("Auth/register", input),
      logout,
      setCustomer: setCustomerState,
      addToCart: (line) => {
        const existing = cart.find((l) => l.productId === line.productId);
        persist(
          existing
            ? cart.map((l) => (l.productId === line.productId ? { ...l, quantity: Math.min(l.stock, l.quantity + 1) } : l))
            : [...cart, { ...line, quantity: 1 }],
        );
      },
      setQuantity: (productId, quantity) =>
        persist(quantity <= 0 ? cart.filter((l) => l.productId !== productId) : cart.map((l) => (l.productId === productId ? { ...l, quantity } : l))),
      clearCart: () => persist([]),
    }),
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [ready, customer, cart, logout],
  );
  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useSession(): Session {
  const s = useContext(Ctx);
  if (!s) throw new Error("useSession must be used inside <SessionProvider>");
  return s;
}
