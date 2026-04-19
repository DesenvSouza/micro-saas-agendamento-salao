import axios, { AxiosError } from "axios";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:8080/api/v1";

export const api = axios.create({
  baseURL: API_URL,
  headers: { "Content-Type": "application/json" },
});

// Inject access token on every request
api.interceptors.request.use((config) => {
  const token =
    typeof window !== "undefined" ? localStorage.getItem("access_token") : null;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Handle 401 — clear storage and redirect to login
api.interceptors.response.use(
  (res) => res,
  async (error: AxiosError) => {
    if (error.response?.status === 401 && typeof window !== "undefined") {
      localStorage.removeItem("access_token");
      localStorage.removeItem("refresh_token");
      localStorage.removeItem("user");
      window.location.href = "/auth/login";
    }
    return Promise.reject(error);
  }
);

// ─── Auth ─────────────────────────────────────────────────────────────────────

export const authApi = {
  loginEstablishment: (email: string, password: string) =>
    api.post("/auth/login", { email, password }),

  loginClient: (email: string, password: string) =>
    api.post("/auth/login", { email, password }),

  registerClient: (data: object) =>
    api.post("/auth/register/client", data),

  registerEstablishment: (data: object) =>
    api.post("/auth/register/establishment", data),

  refresh: (refreshToken: string) =>
    api.post("/auth/refresh", { refreshToken }),

  logout: (refreshToken: string) =>
    api.post("/auth/logout", { refreshToken }),

  me: () => api.get("/auth/me"),
};

// ─── Services ────────────────────────────────────────────────────────────────

export const servicesApi = {
  list: (establishmentId: string, onlyActive = true) =>
    api.get(`/establishments/${establishmentId}/services`, {
      params: { onlyActive },
    }),

  create: (data: object) =>
    api.post("/establishments/me/services", data),

  update: (id: string, data: object) =>
    api.put(`/establishments/me/services/${id}`, data),

  delete: (id: string) =>
    api.delete(`/establishments/me/services/${id}`),
};

// ─── Professionals ────────────────────────────────────────────────────────────

export const professionalsApi = {
  list: (establishmentId: string) =>
    api.get(`/establishments/${establishmentId}/professionals`),

  create: (data: object) =>
    api.post("/establishments/me/professionals", data),

  update: (id: string, data: object) =>
    api.put(`/establishments/me/professionals/${id}`, data),

  delete: (id: string) =>
    api.delete(`/establishments/me/professionals/${id}`),

  setSchedule: (professionalId: string, schedules: object[]) =>
    api.put(`/establishments/me/professionals/${professionalId}/schedule`, {
      schedules,
    }),

  getSchedule: (professionalId: string) =>
    api.get(`/professionals/${professionalId}/schedule`),

  addTimeOff: (professionalId: string, data: object) =>
    api.post(
      `/establishments/me/professionals/${professionalId}/timeoffs`,
      data
    ),

  deleteTimeOff: (professionalId: string, timeOffId: string) =>
    api.delete(
      `/establishments/me/professionals/${professionalId}/timeoffs/${timeOffId}`
    ),
};

// ─── Appointments ─────────────────────────────────────────────────────────────

export const appointmentsApi = {
  listEstablishment: (params?: object) =>
    api.get("/appointments/establishment", { params }),

  listClient: (onlyUpcoming = false) =>
    api.get("/appointments/mine", { params: { onlyUpcoming } }),

  getById: (id: string) => api.get(`/appointments/${id}`),

  create: (data: object) => api.post("/appointments", data),

  confirm: (id: string) => api.post(`/appointments/${id}/confirm`),

  cancel: (id: string) => api.delete(`/appointments/${id}`),

  complete: (id: string) => api.post(`/appointments/${id}/complete`),

  noShow: (id: string) => api.post(`/appointments/${id}/no-show`),

  slots: (professionalId: string, serviceId: string, date: string) =>
    api.get("/appointments/slots", {
      params: { professionalId, serviceId, date },
    }),
};

// ─── Dashboard ────────────────────────────────────────────────────────────────

export const dashboardApi = {
  get: () => api.get("/establishments/me/dashboard"),

  calendar: (from: string, to: string, professionalId?: string) =>
    api.get("/establishments/me/calendar", {
      params: { from, to, professionalId },
    }),
};

// ─── Notifications ────────────────────────────────────────────────────────────

export const notificationsApi = {
  list: (skip = 0, take = 20) =>
    api.get("/notifications", { params: { skip, take } }),

  markRead: (id: string) => api.put(`/notifications/${id}/read`),

  markAllRead: () => api.put("/notifications/read-all"),
};

// ─── Establishments ───────────────────────────────────────────────────────────

export const establishmentsApi = {
  getById: (id: string) => api.get(`/establishments/${id}`),

  updateProfile: (data: object) => api.put("/establishments/me", data),

  nearby: (lat: number, lng: number, radiusKm = 10) =>
    api.get("/establishments/nearby", {
      params: { latitude: lat, longitude: lng, radiusKm },
    }),
};
