"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { toast } from "sonner";
import { Plus, Pencil, Trash2, X } from "lucide-react";
import { professionalsApi } from "@/lib/api";
import { useAuthStore } from "@/store/auth.store";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { formatCurrency } from "@/lib/utils";
import type { ProfessionalDto } from "@/types/api";

const professionalSchema = z.object({
  name: z.string().min(2, "Nome obrigatório"),
  bio: z.string().optional(),
});

type ProfessionalForm = z.infer<typeof professionalSchema>;

export default function ProfessionalsPage() {
  const qc = useQueryClient();
  const { user } = useAuthStore();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ProfessionalDto | null>(null);

  const { data: professionals = [], isLoading } = useQuery<ProfessionalDto[]>({
    queryKey: ["professionals", user?.id],
    queryFn: async () => {
      const res = await professionalsApi.list(user!.id);
      return res.data;
    },
    enabled: !!user?.id,
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ProfessionalForm>({ resolver: zodResolver(professionalSchema) });

  const createMutation = useMutation({
    mutationFn: (data: ProfessionalForm) => professionalsApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["professionals"] });
      toast.success("Profissional criado!");
      closeModal();
    },
    onError: () => toast.error("Não foi possível criar o profissional."),
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: ProfessionalForm }) =>
      professionalsApi.update(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["professionals"] });
      toast.success("Profissional atualizado!");
      closeModal();
    },
    onError: () => toast.error("Não foi possível atualizar o profissional."),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => professionalsApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["professionals"] });
      toast.success("Profissional removido.");
    },
    onError: () => toast.error("Não foi possível remover o profissional."),
  });

  function openCreate() {
    setEditing(null);
    reset({ name: "", bio: "" });
    setModalOpen(true);
  }

  function openEdit(prof: ProfessionalDto) {
    setEditing(prof);
    reset({ name: prof.name, bio: prof.bio ?? "" });
    setModalOpen(true);
  }

  function closeModal() {
    setModalOpen(false);
    setEditing(null);
  }

  function onSubmit(data: ProfessionalForm) {
    if (editing) {
      updateMutation.mutate({ id: editing.id, data });
    } else {
      createMutation.mutate(data);
    }
  }

  const isPending = createMutation.isPending || updateMutation.isPending;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Profissionais</h1>
        <Button onClick={openCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Novo profissional
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{professionals.length} profissional(is)</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="flex justify-center py-8">
              <div className="h-6 w-6 animate-spin rounded-full border-4 border-primary border-t-transparent" />
            </div>
          ) : professionals.length === 0 ? (
            <p className="py-8 text-center text-sm text-muted-foreground">
              Nenhum profissional cadastrado.
            </p>
          ) : (
            <div className="space-y-3">
              {professionals.map((prof) => (
                <div
                  key={prof.id}
                  className="flex items-center justify-between rounded-lg border p-4"
                >
                  <div className="space-y-1">
                    <div className="flex items-center gap-2">
                      <p className="font-medium">{prof.name}</p>
                      {!prof.isActive && (
                        <Badge variant="secondary">Inativo</Badge>
                      )}
                    </div>
                    {prof.bio && (
                      <p className="text-sm text-muted-foreground">{prof.bio}</p>
                    )}
                    {prof.services.length > 0 && (
                      <div className="flex flex-wrap gap-1 pt-1">
                        {prof.services.map((s) => (
                          <Badge key={s.serviceId} variant="outline">
                            {s.serviceName} · {formatCurrency(s.price)}
                          </Badge>
                        ))}
                      </div>
                    )}
                  </div>
                  <div className="flex shrink-0 gap-2">
                    <Button size="sm" variant="outline" onClick={() => openEdit(prof)}>
                      <Pencil className="h-4 w-4" />
                    </Button>
                    <Button
                      size="sm"
                      variant="destructive"
                      onClick={() => deleteMutation.mutate(prof.id)}
                      loading={deleteMutation.isPending}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>

      {/* Modal */}
      {modalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
          <div className="w-full max-w-md rounded-xl bg-card p-6 shadow-lg">
            <div className="mb-4 flex items-center justify-between">
              <h2 className="text-lg font-semibold">
                {editing ? "Editar profissional" : "Novo profissional"}
              </h2>
              <button onClick={closeModal} className="rounded p-1 hover:bg-muted">
                <X className="h-4 w-4" />
              </button>
            </div>

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              <div className="space-y-1">
                <Label htmlFor="name">Nome *</Label>
                <Input id="name" {...register("name")} placeholder="Ex: Ana Paula" />
                {errors.name && <p className="text-xs text-destructive">{errors.name.message}</p>}
              </div>

              <div className="space-y-1">
                <Label htmlFor="bio">Bio</Label>
                <Input id="bio" {...register("bio")} placeholder="Especialidade, experiência..." />
              </div>

              <div className="flex justify-end gap-2 pt-2">
                <Button type="button" variant="outline" onClick={closeModal}>
                  Cancelar
                </Button>
                <Button type="submit" loading={isPending}>
                  {editing ? "Salvar" : "Criar"}
                </Button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
