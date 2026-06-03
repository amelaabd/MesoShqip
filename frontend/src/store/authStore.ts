import { create } from "zustand";
import { persist } from "zustand/middleware";

interface AuthState {
  accessToken: string | null;
  username: string | null;
  isAuthenticated: boolean;
  setAuth: (token: string, username: string) => void;
  clearAuth: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      username: null,
      isAuthenticated: false,
      setAuth: (token, username) =>
        set({ accessToken: token, username, isAuthenticated: true }),
      clearAuth: () =>
        set({ accessToken: null, username: null, isAuthenticated: false }),
    }),
    { name: "auth-storage" },
  ),
);
