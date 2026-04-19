"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { cn } from "@/lib/utils";
import {
  LayoutDashboard,
  Calendar,
  ClipboardList,
  Scissors,
  Users,
  LogOut,
  Zap,
} from "lucide-react";
import { useAuthStore } from "@/store/auth.store";
import { useRouter } from "next/navigation";
import { authApi } from "@/lib/api";

const nav = [
  { href: "/establishment/dashboard", label: "Dashboard", icon: LayoutDashboard },
  { href: "/establishment/calendar", label: "Calendário", icon: Calendar },
  { href: "/establishment/appointments", label: "Agendamentos", icon: ClipboardList },
  { href: "/establishment/services", label: "Serviços", icon: Scissors },
  { href: "/establishment/professionals", label: "Profissionais", icon: Users },
];

export function Sidebar() {
  const pathname = usePathname();
  const { user, clearAuth, refreshToken } = useAuthStore();
  const router = useRouter();

  async function handleLogout() {
    if (refreshToken) {
      try { await authApi.logout(refreshToken); } catch { /* ignore */ }
    }
    clearAuth();
    router.push("/auth/login");
  }

  return (
    <aside className="flex h-screen w-60 flex-col border-r bg-white">
      {/* Brand */}
      <div className="flex items-center gap-2 px-6 py-5 border-b">
        <Zap className="h-6 w-6 text-primary" />
        <span className="font-bold text-lg text-foreground">SalonBook</span>
      </div>

      {/* Nav */}
      <nav className="flex-1 space-y-1 p-3">
        {nav.map(({ href, label, icon: Icon }) => (
          <Link
            key={href}
            href={href}
            className={cn(
              "flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors",
              pathname.startsWith(href)
                ? "bg-primary text-primary-foreground"
                : "text-muted-foreground hover:bg-muted hover:text-foreground"
            )}
          >
            <Icon className="h-4 w-4" />
            {label}
          </Link>
        ))}
      </nav>

      {/* User footer */}
      <div className="border-t p-4">
        <div className="mb-2 px-1">
          <p className="text-sm font-medium truncate">{user?.name}</p>
          <p className="text-xs text-muted-foreground truncate">{user?.email}</p>
        </div>
        <button
          onClick={handleLogout}
          className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm text-muted-foreground hover:bg-muted hover:text-foreground transition-colors"
        >
          <LogOut className="h-4 w-4" />
          Sair
        </button>
      </div>
    </aside>
  );
}
