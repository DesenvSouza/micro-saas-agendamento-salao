"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { Sidebar } from "@/components/layout/sidebar";
import { useAuthStore } from "@/store/auth.store";

export default function EstablishmentLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const { isAuthenticated, user } = useAuthStore();

  useEffect(() => {
    if (!isAuthenticated()) {
      router.replace("/auth/login");
    } else if (user?.role !== "Establishment") {
      router.replace("/auth/login");
    }
  }, [isAuthenticated, user, router]);

  if (!isAuthenticated()) return null;

  return (
    <div className="flex h-screen overflow-hidden">
      <Sidebar />
      <main className="flex-1 overflow-y-auto bg-muted/20 p-6">
        {children}
      </main>
    </div>
  );
}
