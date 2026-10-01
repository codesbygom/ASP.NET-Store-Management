import type { NextConfig } from "next";

const backend = process.env.BACKEND_URL ?? "http://localhost:5100";

const nextConfig: NextConfig = {
  // The browser only talks to Next; /api is proxied to the .NET API,
  // so no extra CORS setup is needed.
  async rewrites() {
    return [{ source: "/api/:path*", destination: `${backend}/api/:path*` }];
  },
};

export default nextConfig;
