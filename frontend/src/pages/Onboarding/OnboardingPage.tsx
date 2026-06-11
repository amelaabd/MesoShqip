import { useState } from "react";
import { useCompleteOnboarding } from "../../hooks/useAuth";
import { t, type SupportedLanguage } from "../../i18n/translations";

const LANGUAGES = [
  { code: "en", flag: "🇬🇧", name: "English" },
  { code: "de", flag: "🇩🇪", name: "Deutsch" },
  { code: "it", flag: "🇮🇹", name: "Italiano" },
  { code: "fr", flag: "🇫🇷", name: "Français" },
  { code: "sv", flag: "🇸🇪", name: "Svenska" },
  { code: "tr", flag: "🇹🇷", name: "Türkçe" },
];

export default function OnboardingPage() {
  const [step, setStep] = useState(1);
  const [language, setLanguage] = useState<SupportedLanguage>("en");
  const [level, setLevel] = useState(0);
  const mutation = useCompleteOnboarding();

  const T = (key: Parameters<typeof t>[1]) => t(language, key);

  const LEVELS = [
    { value: 1, emoji: "🌱", label: T("level1Label"), desc: T("level1Desc") },
    { value: 2, emoji: "📖", label: T("level2Label"), desc: T("level2Desc") },
    { value: 3, emoji: "🚀", label: T("level3Label"), desc: T("level3Desc") },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-red-50 to-white flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-black text-red-600">
            Mëso<span className="text-teal-500">Shqip</span>
          </h1>
          <p className="text-gray-400 text-sm mt-1">
            {T("onboardingStep")} {step} {T("onboardingOf")} 2
          </p>
          <div className="flex gap-2 justify-center mt-3">
            <div
              className={`h-2 w-16 rounded-full transition-all ${step >= 1 ? "bg-red-500" : "bg-gray-200"}`}
            />
            <div
              className={`h-2 w-16 rounded-full transition-all ${step >= 2 ? "bg-red-500" : "bg-gray-200"}`}
            />
          </div>
        </div>

        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
          {step === 1 && (
            <>
              <h2 className="text-xl font-black text-gray-800 mb-2">
                {T("onboardingLangTitle")}
              </h2>
              <p className="text-sm text-gray-500 mb-6">
                {T("onboardingLangDesc")}
              </p>
              <div className="grid grid-cols-2 gap-3 mb-6">
                {LANGUAGES.map((lang) => (
                  <button
                    key={lang.code}
                    onClick={() => setLanguage(lang.code as SupportedLanguage)}
                    className={`p-4 rounded-2xl border-2 text-center transition-all ${
                      language === lang.code
                        ? "border-red-500 bg-red-50"
                        : "border-gray-100 hover:border-gray-300"
                    }`}
                  >
                    <div className="text-2xl mb-1">{lang.flag}</div>
                    <div className="text-sm font-bold text-gray-700">
                      {lang.name}
                    </div>
                  </button>
                ))}
              </div>
              <button
                onClick={() => setStep(2)}
                className="w-full py-3 bg-red-600 text-white font-black rounded-xl hover:bg-red-700 transition-all"
              >
                {T("continue")}
              </button>
            </>
          )}

          {step === 2 && (
            <>
              <h2 className="text-xl font-black text-gray-800 mb-2">
                {T("onboardingLevelTitle")}
              </h2>
              <p className="text-sm text-gray-500 mb-6">
                {T("onboardingLevelDesc")}
              </p>
              <div className="flex flex-col gap-3 mb-6">
                {LEVELS.map((lv) => (
                  <button
                    key={lv.value}
                    onClick={() => setLevel(lv.value)}
                    className={`p-4 rounded-2xl border-2 text-left transition-all ${
                      level === lv.value
                        ? "border-red-500 bg-red-50"
                        : "border-gray-100 hover:border-gray-300"
                    }`}
                  >
                    <div className="font-black text-gray-800">
                      {lv.emoji} {lv.label}
                    </div>
                    <div className="text-sm text-gray-500 mt-1">{lv.desc}</div>
                  </button>
                ))}
              </div>
              <div className="flex gap-3">
                <button
                  onClick={() => setStep(1)}
                  className="flex-1 py-3 border-2 border-gray-200 text-gray-600 font-bold rounded-xl"
                >
                  {T("back")}
                </button>
                <button
                  onClick={() =>
                    mutation.mutate({ nativeLanguage: language, level })
                  }
                  disabled={!level || mutation.isPending}
                  className="flex-1 py-3 bg-red-600 text-white font-black rounded-xl hover:bg-red-700 disabled:opacity-40"
                >
                  {mutation.isPending ? T("saving") : T("startLearning")}
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
