"use client";

import { useQuery } from "@tanstack/react-query";
import { dashboardApi } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { formatCurrency, formatDateTime } from "@/lib/utils";
import { AppointmentStatusLabel } from "@/types/api";
import type { DashboardDto } from "@/types/api";
import { CalendarDays, TrendingUp, Clock, CheckCircle } from "lucide-react";

const statusBadge: Record<number, "default" | "success" | "destructive" | "warning" | "secondary"> = {
  0: "warning",
  1: "default",
  2: "destructive",
  3: "success",
  4: "secondary",
};

export default function DashboardPage() {
  const { data, isLoading } = useQuery<DashboardDto>({
    queryKey: ["dashboard"],
    queryFn: async () => {
      const res = await dashboardApi.get();
      return res.data;
    },
  });

  if (isLoading) {
    return (
      <div className="flex h-full items-center justify-center">
        <div className="h-8 w-8 animate-spin rounded-full border-4 border-primary border-t-transparent" />
      </div>
    );
  }

  const metrics = [
    {
      title: "Agendamentos hoje",
      value: data?.appointmentsToday ?? 0,
      icon: CalendarDays,
      color: "text-blue-600",
    },
    {
      title: "Esta semana",
      value: data?.appointmentsThisWeek ?? 0,
      icon: TrendingUp,
      color: "text-green-600",
    },
    {
      title: "Receita da semana",
      value: formatCurrency(data?.revenueThisWeek ?? 0),
      icon: TrendingUp,
      color: "text-emerald-600",
      isText: true,
    },
    {
      title: "Pendentes",
      value: data?.pendingCount ?? 0,
      icon: Clock,
      color: "text-orange-500",
    },
    {
      title: "Confirmados",
      value: data?.confirmedCount ?? 0,
      icon: CheckCircle,
      color: "text-blue-500",
    },
  ];

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">Dashboard</h1>

      {/* Metrics */}
      <div className="grid grid-cols-2 gap-4 lg:grid-cols-5">
        {metrics.map((m) => (
          <Card key={m.title}>
            <CardContent className="p-4">
              <div className="flex items-center justify-between">
                <p className="text-xs text-muted-foreground">{m.title}</p>
                <m.icon className={`h-4 w-4 ${m.color}`} />
              </div>
              <p className="mt-1 text-2xl font-bold">{m.value}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Recent Appointments */}
      <Card>
        <CardHeader>
          <CardTitle className="text-base">Agendamentos recentes</CardTitle>
        </CardHeader>
        <CardContent>
          {!data?.recentAppointments?.length ? (
            <p className="text-sm text-muted-foreground text-center py-4">
              Nenhum agendamento ainda.
            </p>
          ) : (
            <div className="space-y-3">
              {data.recentAppointments.map((appt) => (
                <div
                  key={appt.id}
                  className="flex items-center justify-between rounded-lg border p-3"
                >
                  <div>
                    <p className="text-sm font-medium">
                      {appt.clientName ?? "Cliente"} · {appt.serviceName ?? "Serviço"}
                    </p>
                    <p className="text-xs text-muted-foreground">
                      {appt.professionalName} · {formatDateTime(appt.startTime)}
                    </p>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-sm font-medium">{formatCurrency(appt.price)}</span>
                    <Badge variant={statusBadge[appt.status]}>
                      {AppointmentStatusLabel[appt.status]}
                    </Badge>
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
