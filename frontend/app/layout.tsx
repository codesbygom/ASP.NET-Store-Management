import type { Metadata } from "next";
import { SessionProvider } from "@/lib/api";
import "./globals.css";

export const metadata: Metadata = {
  title: { template: "%s · Store Management", default: "Store Management" },
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <SessionProvider>{children}</SessionProvider>
      </body>
    </html>
  );
}
