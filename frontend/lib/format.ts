export const money = (n: number) => n.toLocaleString("en-US", { style: "currency", currency: "USD" });

export const formatDate = (iso: string) =>
  new Date(iso).toLocaleString("en-US", { dateStyle: "medium", timeStyle: "short" });
