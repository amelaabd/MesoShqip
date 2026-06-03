import { apiClient } from "./client";

export const getAdminStats = async () => {
  const res = await apiClient.get("/api/v1/admin/stats");
  return res.data;
};

export const getAdminUsers = async () => {
  const res = await apiClient.get("/api/v1/admin/users");
  return res.data;
};

export const getAdminChildren = async () => {
  const res = await apiClient.get("/api/v1/admin/children");
  return res.data;
};

export const getAdminLessons = async () => {
  const res = await apiClient.get("/api/v1/admin/lessons");
  return res.data;
};

export const changeUserRole = async (userId: string, role: string) => {
  const res = await apiClient.put(`/api/v1/admin/users/${userId}/role`, {
    role,
  });
  return res.data;
};

export const deleteUser = async (userId: string) => {
  const res = await apiClient.delete(`/api/v1/admin/users/${userId}`);
  return res.data;
};

export const toggleLessonPublish = async (lessonId: string) => {
  const res = await apiClient.put(`/api/v1/admin/lessons/${lessonId}/publish`);
  return res.data;
};
