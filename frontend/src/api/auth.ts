import { apiClient } from "./client";
import type { AuthResponse } from "../types";

export const register = async (data: {
  username: string;
  email: string;
  password: string;
}) => {
  const res = await apiClient.post("/api/v1/auth/register", data);
  return res.data;
};

export const login = async (data: {
  email: string;
  password: string;
}): Promise<AuthResponse> => {
  const res = await apiClient.post<AuthResponse>("/api/v1/auth/login", data);
  localStorage.setItem("accessToken", res.data.accessToken);
  localStorage.setItem("refreshToken", res.data.refreshToken);
  return res.data;
};

export const logout = () => {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
};
