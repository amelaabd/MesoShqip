import { create } from "zustand";
import { persist } from "zustand/middleware";

interface AuthState {
  accessToken: string | null;
  username: string | null;
  role: string | null;
  nativeLanguage: string;
  onboardingCompleted: boolean;
  isAuthenticated: boolean;
  setAuth: (token: string, username: string, role: string) => void;
  setOnboarding: (language: string, completed: boolean) => void;
  clearAuth: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      username: null,
      role: null,
      nativeLanguage: "en",
      onboardingCompleted: false,
      isAuthenticated: false,
      setAuth: (token, username, role) =>
        set({ accessToken: token, username, role, isAuthenticated: true }),
      setOnboarding: (language, completed) =>
        set({ nativeLanguage: language, onboardingCompleted: completed }),
      clearAuth: () =>
        set({
          accessToken: null,
          username: null,
          role: null,
          isAuthenticated: false,
          onboardingCompleted: false,
          nativeLanguage: "en",
        }),
    }),
    { name: "auth-storage" },
  ),
);
