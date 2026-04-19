"use client";

import { useRef, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import ptBrLocale from "@fullcalendar/core/locales/pt-br";
import { dashboardApi } from "@/lib/api";
import type { CalendarEventDto } from "@/types/api";
import { formatCurrency } from "@/lib/utils";

export default function CalendarPage() {
  const calRef = useRef<FullCalendar>(null);
  const [range, setRange] = useState<{ from: string; to: string }>(() => {
    const now = new Date();
    const from = new Date(now.getFullYear(), now.getMonth(), 1).toISOString();
    const to = new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString();
    return { from, to };
  });

  const { data: events = [] } = useQuery<CalendarEventDto[]>({
    queryKey: ["calendar", range.from, range.to],
    queryFn: async () => {
      const res = await dashboardApi.calendar(range.from, range.to);
      return res.data;
    },
  });

  const fcEvents = events.map((e) => ({
    id: e.id,
    title: e.title,
    start: e.start,
    end: e.end,
    backgroundColor: e.color,
    borderColor: e.color,
    extendedProps: {
      clientName: e.clientName,
      professionalName: e.professionalName,
      serviceName: e.serviceName,
      price: e.price,
      status: e.status,
    },
  }));

  return (
    <div className="space-y-4">
      <h1 className="text-2xl font-bold">Calendário</h1>
      <div className="rounded-xl border bg-card p-4 shadow">
        <FullCalendar
          ref={calRef}
          plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
          initialView="timeGridWeek"
          locale={ptBrLocale}
          headerToolbar={{
            left: "prev,next today",
            center: "title",
            right: "dayGridMonth,timeGridWeek,timeGridDay",
          }}
          slotMinTime="06:00:00"
          slotMaxTime="22:00:00"
          allDaySlot={false}
          height="auto"
          events={fcEvents}
          datesSet={(info) => {
            setRange({
              from: info.startStr,
              to: info.endStr,
            });
          }}
          eventContent={(info) => {
            const { professionalName, price } = info.event.extendedProps;
            return (
              <div className="overflow-hidden px-1 text-xs leading-tight text-white">
                <p className="font-semibold truncate">{info.event.title}</p>
                {professionalName && (
                  <p className="truncate opacity-90">{professionalName}</p>
                )}
                <p>{formatCurrency(price)}</p>
              </div>
            );
          }}
        />
      </div>
    </div>
  );
}
