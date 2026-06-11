import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import { generateStory } from "../../api/stories";
import { useTranslation } from "../../hooks/useTranslation";
import toast from "react-hot-toast";
import type { StoryResponse } from "../../types";

export default function StoryPage() {
  const navigate = useNavigate();
  const { t, lang } = useTranslation();
  const [story, setStory] = useState<StoryResponse | null>(null);
  const [showTrans, setShowTrans] = useState(false);

  const mutation = useMutation({
    mutationFn: generateStory,
    onSuccess: (data) => {
      setStory(data);
      toast.success("✓");
    },
    onError: () => toast.error("Gabim. Provo sërish."),
  });

  const langLabel: Record<string, string> = {
    en: "🇬🇧 English",
    de: "🇩🇪 Deutsch",
    it: "🇮🇹 Italiano",
    fr: "🇫🇷 Français",
    sv: "🇸🇪 Svenska",
    tr: "🇹🇷 Türkçe",
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3 sticky top-0 z-10">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <div className="flex-1">
          <p className="font-black text-gray-800 text-sm">{t("aiStory")}</p>
          <p className="text-xs text-gray-400">{t("generatedBy")}</p>
        </div>
        <span className="text-xs font-bold bg-teal-100 text-teal-700 px-3 py-1 rounded-full">
          +3 XP
        </span>
      </div>

      <div className="max-w-lg mx-auto px-4 py-6">
        {!story && (
          <div className="text-center py-12 md:py-16">
            <div className="text-6xl md:text-7xl mb-5">🦅</div>
            <h2 className="text-xl md:text-2xl font-black text-gray-800 mb-3">
              {t("todaysStory")}
            </h2>
            <p className="text-gray-500 mb-8 max-w-xs mx-auto leading-relaxed text-sm">
              {t("storyDesc")}
            </p>
            <button
              onClick={() =>
                mutation.mutate({
                  wordsToIntroduce: [
                    "shtëpia",
                    "familja",
                    "dashuria",
                    "miqësia",
                    "gëzimi",
                  ],
                })
              }
              disabled={mutation.isPending}
              className="px-8 py-4 bg-teal-500 text-white font-black rounded-2xl hover:bg-teal-600 transition-all disabled:opacity-50"
            >
              {mutation.isPending ? (
                <span className="flex items-center gap-2">
                  <svg
                    className="animate-spin h-4 w-4"
                    viewBox="0 0 24 24"
                    fill="none"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    />
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8v8H4z"
                    />
                  </svg>
                  {t("generating")}
                </span>
              ) : (
                t("generateStory")
              )}
            </button>
          </div>
        )}

        {story && (
          <div className="space-y-4">
            <div className="bg-teal-500 rounded-3xl p-5 md:p-6 text-white relative overflow-hidden">
              <div className="absolute right-4 top-1/2 -translate-y-1/2 text-5xl opacity-20">
                🦅
              </div>
              <p className="text-xs font-bold uppercase tracking-widest opacity-75 mb-2">
                ✨ {t("todaysStory")}
              </p>
              <h2 className="text-xl md:text-2xl font-black leading-tight">
                {story.titleAlbanian}
              </h2>
            </div>

            <div className="bg-white rounded-2xl border border-gray-100 p-5 md:p-6">
              <div className="flex justify-between items-center mb-4">
                <p className="text-xs font-black text-gray-400 uppercase tracking-wide">
                  {t("albanianLabel")}
                </p>
                <button
                  onClick={() => setShowTrans(!showTrans)}
                  className="text-xs font-bold text-teal-600"
                >
                  {showTrans ? t("hideTranslation") : t("showTranslation")}
                </button>
              </div>
              <p className="text-gray-800 leading-relaxed text-sm md:text-base">
                {story.bodyAlbanian}
              </p>
              {showTrans && (
                <div className="mt-4 pt-4 border-t border-gray-100">
                  <p className="text-xs font-black text-gray-400 uppercase tracking-wide mb-2">
                    {langLabel[lang] ?? t("translationLabel")}
                  </p>
                  <p className="text-gray-600 leading-relaxed text-sm italic">
                    {story.bodyTranslated}
                  </p>
                </div>
              )}
            </div>

            {story.newWords?.length > 0 && (
              <div>
                <p className="font-black text-gray-800 mb-3 text-sm">
                  {t("newWords")}
                </p>
                <div className="grid grid-cols-2 gap-3">
                  {story.newWords.map((w, i) => (
                    <div
                      key={i}
                      className="bg-amber-50 border border-amber-200 rounded-2xl p-3 md:p-4"
                    >
                      <p className="font-black text-amber-800 text-sm md:text-base">
                        {w.albanian}
                      </p>
                      <p className="text-amber-600 text-xs md:text-sm mt-1">
                        {w.translated}
                      </p>
                      <p className="text-amber-400 text-xs italic mt-1">
                        [{w.phonetic}]
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            )}

            <div className="flex gap-3 pb-8">
              <button
                onClick={() => navigate("/dashboard")}
                className="flex-1 py-3 bg-red-600 text-white font-black rounded-2xl hover:bg-red-700 text-sm"
              >
                {t("goBack")}
              </button>
              <button
                onClick={() => {
                  setStory(null);
                  setShowTrans(false);
                }}
                className="px-4 py-3 border-2 border-gray-200 text-gray-600 font-bold rounded-2xl text-sm"
              >
                {t("generateNew")}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
