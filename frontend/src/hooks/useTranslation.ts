import { useAuthStore } from "../store/authStore";
import {
  t,
  type SupportedLanguage,
  type TranslationKeys,
} from "../i18n/translations";

export function useTranslation() {
  const nativeLanguage = useAuthStore(
    (s) => s.nativeLanguage,
  ) as SupportedLanguage;

  return {
    t: (key: TranslationKeys) => t(nativeLanguage, key),
    lang: nativeLanguage,
  };
}
