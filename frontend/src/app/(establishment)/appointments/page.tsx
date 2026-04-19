"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { appointmentsApi } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Select } from "@/components/ui/select";
import { formatCurrency, formatDateTime } from "@/lib/utils";
import { AppointmentStatusLabel } from "@/types/api";
import type { AppointmentDto } from "@/types/api";

const statusBadge: Record<number, "default" | "success" | "destructive" | "warning" | "secondary"> = {
  0: "warning",
  1: "default",
  2: "destructive",
  3: "success",
  4: "secondary",
};

export default function AppointmentsPage() {
  const qc = useQueryClient();
  const [statusFilter, setStatusFilter] = useState<string>("");

  const { data: appointments = [], isLoading } = useQuery<AppointmentDto[]>({
    queryKey: ["appointments", "establishment", statusFilter],
    queryFn: async () => {
      const params = statusFilter !== "" ? { status: Number(statusFilter) } : {};
      const res = await appointmentsApi.listEstablishment(params);
      return res.data;
    },
  });

  const confirmMutation = useMutation({
    mutationFn: (id: string) => appointmentsApi.confirm(id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ["appointments"] }); toast.success("Agendamento confirmado!"); },
    onError: () => toast.error("Não foi possível confirmar."),
  });

  const completeMutation = useMutation({
    mutationFn: (id: string) => appointmentsApi.complete(id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ["appointments"] }); toast.success("Marcado como concluído."); },
    onError: () => toast.error("Não foi possível concluir."),
  });

  const noShowMutation = useMutation({
    mutationFn: (id: string) => appointmentsApi.noShow(id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ["appointments"] }); toast.success("No-show registrado."); },
    onError: () => toast.error("Não foi possível registrar."),
  });

  const cancelMutation = useMutation({
    mutationFn: (id: string) => appointmentsApi.cancel(id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ["appointments"] }); toast.success("Agendamento cancelado."); },
    onError: () => toast.error("Não foi possível cancelar."),
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Agendamentos</h1>
        <Select
          className="w-44"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
        >
          <option value="">Todos os status</option>
          <option value="0">Pendente</option>
          <option value="1">Confirmado</option>
          <option value="2">Cancelado</option>
          <option value="3">Concluído</option>
          <option value="4">Não compareceu</option>
        </Select>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{appointments.length} agendamento(s)</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="flex justify-center py-8">
              <div className="h-6 w-6 animate-spin rounded-full border-4 border-primary border-t-transparent" />
            </div>
          ) : appointments.length === 0 ? (
            <p className="text-center text-sm text-muted-foreground py-8">
              Nenhum agendamento encontrado.
            </p>
          ) : (
            <div className="space-y-3">
              {appointments.map((appt) => (
                <div key={appt.id} className="rounded-lg border p-4">
                  <div className="flex items-start justify-between gap-4">
                    <div className="space-y-0.5">
                      <p className="font-medium">{appt.clientName ?? "Cliente"}</p>
                      <p className="text-sm text-muted-foreground">
                        {appt.serviceName} · {appt.professionalName}
                      </p>
                      <p className="text-sm text-muted-foreground">
                        {formatDateTime(appt.startTime)} → {new Date(appt.endTime).toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" })}
                      </p>
                      {appt.clientNotes && (
                        <p className="text-xs text-muted-foreground italic">&ldquo;{appt.clientNotes}&rdquo;</p>
                      )}
                    </div>
                    <div className="flex flex-col items-end gap-2">
                      <Badge variant={statusBadge[appt.status]}>
                        {AppointmentStatusLabel[appt.status]}
                      </Badge>
                      <span className="text-sm font-medium">{formatCurrency(appt.price)}</span>
                    </div>
                  </div>

                  {/* Actions */}
                  <div className="mt-3 flex gap-2">
                    {appt.status === 0 && (
                      <Button
                        size="sm"
                        onClick={() => confirmMutation.mutate(appt.id)}
                        loading={confirmMutation.isPending}
                      >
                        Confirmar
                      </Button>
                    )}
                    {appt.status === 1 && (
                      <>
                        <Button
                          size="sm"
                          onClick={() => completeMutation.mutate(appt.id)}
                          loading={completeMutation.isPending}
                        >
                          Concluir
                        </Button>
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => noShowMutation.mutate(appt.id)}
                          loading={noShowMutation.isPending}
                        >
                          No-show
                        </Button>
                      </>
                    )}
                    {(appt.status === 0 || appt.status === 1) && (
                      <Button
                        size="sm"
                        variant="destructive"
                        onClick={() => cancelMutation.mutate(appt.id)}
                        loading={cancelMutation.isPending}
                      >
                        Cancelar
                      </Button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
