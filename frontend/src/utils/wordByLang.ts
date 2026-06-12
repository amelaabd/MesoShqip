import type { VocabularyItem } from "../types";
import type { SupportedLanguage } from "../i18n/translations";

export function getWordInLang(
  word: VocabularyItem,
  lang: SupportedLanguage,
): string {
  switch (lang) {
    case "de":
      return word.wordGerman || word.wordEnglish;
    case "it":
      return word.wordItalian || word.wordEnglish;
    case "fr":
      return word.wordFrench || word.wordEnglish;
    case "sv":
      return word.wordSwedish || word.wordEnglish;
    case "tr":
      return word.wordTurkish || word.wordEnglish;
    default:
      return word.wordEnglish;
  }
}
