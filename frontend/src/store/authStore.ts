import { create } from "zustand";
import { persist } from "zustand/middleware";

interface AuthState {
  accessToken: string | null;
  username: string | null;
  isAuthenticated: boolean;
  selectedChildId: string | null;
  selectedChildLanguage: string;
  setAuth: (token: string, username: string) => void;
  clearAuth: () => void;
  setSelectedChild: (id: string, language: string) => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      username: null,
      isAuthenticated: false,
      selectedChildId: null,
      selectedChildLanguage: "en",
      setAuth: (token, username) =>
        set({ accessToken: token, username, isAuthenticated: true }),
      clearAuth: () =>
        set({
          accessToken: null,
          username: null,
          isAuthenticated: false,
          selectedChildId: null,
        }),
      setSelectedChild: (id, language) =>
        set({ selectedChildId: id, selectedChildLanguage: language }),
    }),
    { name: "auth-storage" },
  ),
);
