"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { toast } from "sonner";
import { Plus, Pencil, Trash2, X } from "lucide-react";
import { servicesApi } from "@/lib/api";
import { useAuthStore } from "@/store/auth.store";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select } from "@/components/ui/select";
import { formatCurrency } from "@/lib/utils";
import { ServiceCategoryLabel } from "@/types/api";
import type { ServiceDto } from "@/types/api";

const serviceSchema = z.object({
  name: z.string().min(2, "Nome obrigatório"),
  category: z.coerce.number().min(0),
  durationMinutes: z.coerce.number().min(5, "Mínimo 5 minutos"),
  slotIntervalMinutes: z.coerce.number().min(5).optional(),
  price: z.coerce.number().min(0, "Preço inválido"),
  description: z.string().optional(),
});

type ServiceForm = z.infer<typeof serviceSchema>;

export default function ServicesPage() {
  const qc = useQueryClient();
  const { user } = useAuthStore();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ServiceDto | null>(null);

  const { data: services = [], isLoading } = useQuery<ServiceDto[]>({
    queryKey: ["services", user?.id],
    queryFn: async () => {
      const res = await servicesApi.list(user!.id, false);
      return res.data;
    },
    enabled: !!user?.id,
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ServiceForm>({ resolver: zodResolver(serviceSchema) });

  const createMutation = useMutation({
    mutationFn: (data: ServiceForm) => servicesApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["services"] });
      toast.success("Serviço criado!");
      closeModal();
    },
    onError: () => toast.error("Não foi possível criar o serviço."),
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: ServiceForm }) =>
      servicesApi.update(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["services"] });
      toast.success("Serviço atualizado!");
      closeModal();
    },
    onError: () => toast.error("Não foi possível atualizar o serviço."),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => servicesApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["services"] });
      toast.success("Serviço removido.");
    },
    onError: () => toast.error("Não foi possível remover o serviço."),
  });

  function openCreate() {
    setEditing(null);
    reset({ name: "", category: 0, durationMinutes: 30, slotIntervalMinutes: 30, price: 0, description: "" });
    setModalOpen(true);
  }

  function openEdit(svc: ServiceDto) {
    setEditing(svc);
    reset({
      name: svc.name,
      category: svc.category,
      durationMinutes: svc.durationMinutes,
      slotIntervalMinutes: svc.slotIntervalMinutes,
      price: svc.price,
      description: svc.description ?? "",
    });
    setModalOpen(true);
  }

  function closeModal() {
    setModalOpen(false);
    setEditing(null);
  }

  function onSubmit(data: ServiceForm) {
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
        <h1 className="text-2xl font-bold">Serviços</h1>
        <Button onClick={openCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Novo serviço
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{services.length} serviço(s)</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="flex justify-center py-8">
              <div className="h-6 w-6 animate-spin rounded-full border-4 border-primary border-t-transparent" />
            </div>
          ) : services.length === 0 ? (
            <p className="py-8 text-center text-sm text-muted-foreground">
              Nenhum serviço cadastrado.
            </p>
          ) : (
            <div className="space-y-3">
              {services.map((svc) => (
                <div
                  key={svc.id}
                  className="flex items-center justify-between rounded-lg border p-4"
                >
                  <div className="space-y-0.5">
                    <div className="flex items-center gap-2">
                      <p className="font-medium">{svc.name}</p>
                      {!svc.isActive && (
                        <Badge variant="secondary">Inativo</Badge>
                      )}
                    </div>
                    <p className="text-sm text-muted-foreground">
                      {svc.categoryName} · {svc.durationMinutes} min · {formatCurrency(svc.price)}
                    </p>
                    {svc.description && (
                      <p className="text-xs text-muted-foreground">{svc.description}</p>
                    )}
                  </div>
                  <div className="flex gap-2">
                    <Button size="sm" variant="outline" onClick={() => openEdit(svc)}>
                      <Pencil className="h-4 w-4" />
                    </Button>
                    <Button
                      size="sm"
                      variant="destructive"
                      onClick={() => deleteMutation.mutate(svc.id)}
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
                {editing ? "Editar serviço" : "Novo serviço"}
              </h2>
              <button onClick={closeModal} className="rounded p-1 hover:bg-muted">
                <X className="h-4 w-4" />
              </button>
            </div>

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              <div className="space-y-1">
                <Label htmlFor="name">Nome *</Label>
                <Input id="name" {...register("name")} placeholder="Ex: Corte feminino" />
                {errors.name && <p className="text-xs text-destructive">{errors.name.message}</p>}
              </div>

              <div className="space-y-1">
                <Label htmlFor="category">Categoria *</Label>
                <Select id="category" {...register("category")}>
                  {Object.entries(ServiceCategoryLabel).map(([k, v]) => (
                    <option key={k} value={k}>{v}</option>
                  ))}
                </Select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-1">
                  <Label htmlFor="durationMinutes">Duração (min) *</Label>
                  <Input id="durationMinutes" type="number" min={5} {...register("durationMinutes")} />
                  {errors.durationMinutes && <p className="text-xs text-destructive">{errors.durationMinutes.message}</p>}
                </div>
                <div className="space-y-1">
                  <Label htmlFor="slotIntervalMinutes">Intervalo (min)</Label>
                  <Input id="slotIntervalMinutes" type="number" min={5} {...register("slotIntervalMinutes")} />
                </div>
              </div>

              <div className="space-y-1">
                <Label htmlFor="price">Preço (R$) *</Label>
                <Input id="price" type="number" step="0.01" min={0} {...register("price")} />
                {errors.price && <p className="text-xs text-destructive">{errors.price.message}</p>}
              </div>

              <div className="space-y-1">
                <Label htmlFor="description">Descrição</Label>
                <Input id="description" {...register("description")} placeholder="Opcional" />
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
