"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { authApi } from "@/lib/api";
import { useAuthStore } from "@/store/auth.store";
import { Zap } from "lucide-react";
import type { LoginResponse } from "@/types/api";

export default function LoginPage() {
  const router = useRouter();
  const { setAuth } = useAuthStore();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({ email: "", password: "" });

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!form.email || !form.password) {
      toast.error("Preencha e-mail e senha.");
      return;
    }

    setLoading(true);
    try {
      const { data } = await authApi.loginEstablishment(form.email, form.password);
      const res = data as LoginResponse;

      setAuth(
        { id: res.userId, email: res.email, name: res.name, role: res.role },
        res.accessToken,
        res.refreshToken
      );

      if (res.role === "Establishment") {
        router.replace("/establishment/dashboard");
      } else {
        router.replace("/");
      }
    } catch (err: unknown) {
      const msg =
        (err as { response?: { data?: { message?: string } } })?.response?.data?.message ??
        "E-mail ou senha inválidos.";
      toast.error(msg);
    } finally {
      setLoading(false);
    }
  }

  return (
    <Card>
      <CardHeader className="text-center">
        <div className="flex justify-center mb-2">
          <Zap className="h-10 w-10 text-primary" />
        </div>
        <CardTitle className="text-2xl">SalonBook</CardTitle>
        <CardDescription>Entre na sua conta</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-1">
            <Label htmlFor="email">E-mail</Label>
            <Input
              id="email"
              type="email"
              placeholder="seu@email.com"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
            />
          </div>
          <div className="space-y-1">
            <Label htmlFor="password">Senha</Label>
            <Input
              id="password"
              type="password"
              placeholder="••••••••"
              value={form.password}
              onChange={(e) => setForm({ ...form, password: e.target.value })}
            />
          </div>
          <Button type="submit" className="w-full" loading={loading}>
            Entrar
          </Button>
        </form>

        <div className="mt-6 text-center text-sm text-muted-foreground space-y-1">
          <p>
            Não tem conta?{" "}
            <Link href="/auth/register/establishment" className="text-primary hover:underline">
              Cadastrar estabelecimento
            </Link>
          </p>
          <p>
            É cliente?{" "}
            <Link href="/auth/register/client" className="text-primary hover:underline">
              Criar conta
            </Link>
          </p>
        </div>
      </CardContent>
    </Card>
  );
}
