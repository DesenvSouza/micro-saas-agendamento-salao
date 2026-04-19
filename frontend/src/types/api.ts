// ─── Auth ────────────────────────────────────────────────────────────────────

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  role: string;
  userId: string;
  email: string;
  name: string;
}

export interface RegisterClientRequest {
  fullName: string;
  email: string;
  password: string;
  phone?: string;
}

export interface RegisterEstablishmentRequest {
  tradeName: string;
  email: string;
  password: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  latitude: number;
  longitude: number;
}

// ─── Services ────────────────────────────────────────────────────────────────

export interface ServiceDto {
  id: string;
  establishmentId: string;
  name: string;
  description?: string;
  category: number;
  categoryName: string;
  durationMinutes: number;
  slotIntervalMinutes: number;
  price: number;
  currency: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateServiceRequest {
  name: string;
  category: number;
  durationMinutes: number;
  price: number;
  description?: string;
  slotIntervalMinutes?: number;
}

// ─── Professionals ────────────────────────────────────────────────────────────

export interface ProfessionalDto {
  id: string;
  establishmentId: string;
  name: string;
  bio?: string;
  photoUrl?: string;
  isActive: boolean;
  createdAt: string;
  services: ProfessionalServiceItemDto[];
}

export interface ProfessionalServiceItemDto {
  serviceId: string;
  serviceName: string;
  durationMinutes: number;
  price: number;
}

export interface WorkingScheduleDto {
  id: string;
  dayOfWeek: number;
  dayName: string;
  isWorkingDay: boolean;
  startTime: string;
  endTime: string;
  lunchBreakStart?: string;
  lunchBreakEnd?: string;
}

export interface TimeOffDto {
  id: string;
  startDateTime: string;
  endDateTime: string;
  reason?: string;
}

// ─── Appointments ─────────────────────────────────────────────────────────────

export interface AppointmentDto {
  id: string;
  clientId: string;
  clientName?: string;
  clientPhone?: string;
  establishmentId: string;
  establishmentName?: string;
  professionalId: string;
  professionalName?: string;
  serviceId: string;
  serviceName?: string;
  startTime: string;
  endTime: string;
  status: number;
  statusName: string;
  price: number;
  currency: string;
  clientNotes?: string;
  establishmentNotes?: string;
  createdAt: string;
}

export interface CalendarEventDto {
  id: string;
  title: string;
  start: string;
  end: string;
  color: string;
  status: string;
  clientName?: string;
  professionalName?: string;
  serviceName?: string;
  price: number;
}

// ─── Dashboard ────────────────────────────────────────────────────────────────

export interface DashboardDto {
  appointmentsToday: number;
  appointmentsThisWeek: number;
  revenueThisWeek: number;
  pendingCount: number;
  confirmedCount: number;
  recentAppointments: AppointmentDto[];
}

// ─── Establishment ────────────────────────────────────────────────────────────

export interface EstablishmentDto {
  id: string;
  tradeName: string;
  legalName?: string;
  cnpj?: string;
  phone?: string;
  description?: string;
  street: string;
  number?: string;
  complement?: string;
  neighborhood?: string;
  city: string;
  state: string;
  zipCode: string;
  latitude: number;
  longitude: number;
  logoUrl?: string;
  autoConfirmAppointments: boolean;
  planType: number;
  isApproved: boolean;
  createdAt: string;
}

// ─── Notifications ────────────────────────────────────────────────────────────

export interface NotificationDto {
  id: string;
  type: number;
  title: string;
  body: string;
  isRead: boolean;
  relatedId?: string;
  createdAt: string;
}

export interface GetUserNotificationsResponse {
  items: NotificationDto[];
  unreadCount: number;
}

// ─── Enums ────────────────────────────────────────────────────────────────────

export const AppointmentStatus = {
  Pending: 0,
  Confirmed: 1,
  Cancelled: 2,
  Completed: 3,
  NoShow: 4,
} as const;

export const AppointmentStatusLabel: Record<number, string> = {
  0: "Pendente",
  1: "Confirmado",
  2: "Cancelado",
  3: "Concluído",
  4: "Não compareceu",
};

export const ServiceCategory = {
  Hair: 0,
  Nails: 1,
  Skin: 2,
  Makeup: 3,
  Massage: 4,
  Barber: 5,
  Other: 6,
} as const;

export const ServiceCategoryLabel: Record<number, string> = {
  0: "Cabelo",
  1: "Unhas",
  2: "Pele",
  3: "Maquiagem",
  4: "Massagem",
  5: "Barbearia",
  6: "Outro",
};
