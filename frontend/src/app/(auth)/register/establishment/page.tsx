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

const initial = {
  tradeName: "",
  email: "",
  password: "",
  street: "",
  city: "",
  state: "",
  zipCode: "",
};

export default function RegisterEstablishmentPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState(initial);

  function set(field: keyof typeof initial, value: string) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!form.tradeName || !form.email || !form.password || !form.street || !form.city || !form.state || !form.zipCode) {
      toast.error("Preencha todos os campos obrigatórios.");
      return;
    }

    setLoading(true);
    try {
      await authApi.registerEstablishment({
        ...form,
        latitude: 0,
        longitude: 0,
        zipCode: form.zipCode.replace(/\D/g, ""),
      });
      toast.success("Cadastro realizado! Aguarde aprovação e faça login.");
      router.push("/auth/login");
    } catch (err: unknown) {
      const errors =
        (err as { response?: { data?: { errors?: string[] } } })?.response?.data?.errors;
      if (errors?.length) toast.error(errors.join(", "));
      else toast.error("Erro ao cadastrar. Verifique os dados.");
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
        <CardTitle>Cadastrar Estabelecimento</CardTitle>
        <CardDescription>Crie sua conta e comece a receber agendamentos</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-3">
          <div className="space-y-1">
            <Label>Nome fantasia *</Label>
            <Input placeholder="Ex: Barbearia do João" value={form.tradeName} onChange={(e) => set("tradeName", e.target.value)} />
          </div>
          <div className="space-y-1">
            <Label>E-mail *</Label>
            <Input type="email" placeholder="contato@estabelecimento.com" value={form.email} onChange={(e) => set("email", e.target.value)} />
          </div>
          <div className="space-y-1">
            <Label>Senha *</Label>
            <Input type="password" placeholder="Mínimo 8 caracteres" value={form.password} onChange={(e) => set("password", e.target.value)} />
          </div>
          <div className="grid grid-cols-2 gap-2">
            <div className="space-y-1 col-span-2">
              <Label>Rua/Endereço *</Label>
              <Input placeholder="Rua das Flores, 123" value={form.street} onChange={(e) => set("street", e.target.value)} />
            </div>
            <div className="space-y-1">
              <Label>Cidade *</Label>
              <Input placeholder="São Paulo" value={form.city} onChange={(e) => set("city", e.target.value)} />
            </div>
            <div className="space-y-1">
              <Label>UF *</Label>
              <Input placeholder="SP" maxLength={2} value={form.state} onChange={(e) => set("state", e.target.value.toUpperCase())} />
            </div>
            <div className="space-y-1 col-span-2">
              <Label>CEP *</Label>
              <Input placeholder="00000-000" value={form.zipCode} onChange={(e) => set("zipCode", e.target.value)} />
            </div>
          </div>

          <Button type="submit" className="w-full mt-2" loading={loading}>
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
