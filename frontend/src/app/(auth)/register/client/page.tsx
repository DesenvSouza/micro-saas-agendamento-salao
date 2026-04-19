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
import { Zap } from "lucide-react";

export default function RegisterClientPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({ fullName: "", email: "", password: "", phone: "" });

  function set(field: keyof typeof form, value: string) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!form.fullName || !form.email || !form.password) {
      toast.error("Preencha nome, e-mail e senha.");
      return;
    }

    setLoading(true);
    try {
      await authApi.registerClient({
        fullName: form.fullName,
        email: form.email,
        password: form.password,
        phone: form.phone || undefined,
      });
      toast.success("Conta criada com sucesso! Faça login.");
      router.push("/auth/login");
    } catch (err: unknown) {
      const errors =
        (err as { response?: { data?: { errors?: string[] } } })?.response?.data?.errors;
      if (errors?.length) toast.error(errors.join(", "));
      else toast.error("Erro ao cadastrar.");
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
        <CardTitle>Criar conta de cliente</CardTitle>
        <CardDescription>Agende serviços em salões e barbearias</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-3">
          <div className="space-y-1">
            <Label>Nome completo *</Label>
            <Input placeholder="João Silva" value={form.fullName} onChange={(e) => set("fullName", e.target.value)} />
          </div>
          <div className="space-y-1">
            <Label>E-mail *</Label>
            <Input type="email" placeholder="joao@email.com" value={form.email} onChange={(e) => set("email", e.target.value)} />
          </div>
          <div className="space-y-1">
            <Label>Senha *</Label>
            <Input type="password" placeholder="Mínimo 8 caracteres" value={form.password} onChange={(e) => set("password", e.target.value)} />
          </div>
          <div className="space-y-1">
            <Label>Telefone (opcional)</Label>
            <Input placeholder="(11) 99999-9999" value={form.phone} onChange={(e) => set("phone", e.target.value)} />
          </div>
          <Button type="submit" className="w-full mt-1" loading={loading}>
            Criar conta
          </Button>
        </form>

        <p className="mt-4 text-center text-sm text-muted-foreground">
          Já tem conta?{" "}
          <Link href="/auth/login" className="text-primary hover:underline">
            Entrar
          </Link>
        </p>
      </CardContent>
    </Card>
  );
}
