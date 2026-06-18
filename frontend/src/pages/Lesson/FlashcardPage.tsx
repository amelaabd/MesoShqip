import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@tanstack/react-query";
import { getLessonById } from "../../api/lessons";
import { updateProgress } from "../../api/progress";
import { useTranslation } from "../../hooks/useTranslation";
import { getWordInLang } from "../../utils/wordByLang";
import type { SupportedLanguage } from "../../i18n/translations";
import type { VocabularyItem } from "../../types";
import toast from "react-hot-toast";

const langLabel: Record<string, string> = {
  en: "🇬🇧 English",
  de: "🇩🇪 Deutsch",
  it: "🇮🇹 Italiano",
  fr: "🇫🇷 Français",
  sv: "🇸🇪 Svenska",
  tr: "🇹🇷 Türkçe",
};

export default function FlashcardPage() {
  const { lessonId } = useParams<{ lessonId: string }>();
  const navigate = useNavigate();
  const { t, lang } = useTranslation();

  const [cardIndex, setCardIndex] = useState(0);
  const [flipped, setFlipped] = useState(false);
  const [known, setKnown] = useState<string[]>([]);
  const [recording, setRecording] = useState(false);
  const [score, setScore] = useState<number | null>(null);
  const [finished, setFinished] = useState(false);

  const { data: lesson, isLoading } = useQuery({
    queryKey: ["lesson", lessonId],
    queryFn: () => getLessonById(lessonId!),
    enabled: !!lessonId,
  });

  const progressMutation = useMutation({
    mutationFn: updateProgress,
    onSuccess: () => toast.success("✓"),
  });

  if (isLoading)
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <div className="text-4xl mb-3 animate-pulse">🦅</div>
          <p className="text-gray-400 font-bold text-sm">{t("loading")}</p>
        </div>
      </div>
    );

  const words = lesson?.vocabularyItems ?? [];
  const current: VocabularyItem | undefined = words[cardIndex];
  const progress = Math.round((cardIndex / Math.max(words.length, 1)) * 100);

  const handleNext = (knew: boolean) => {
    if (!current) return;
    if (knew) setKnown((prev) => [...prev, current.id]);
    setFlipped(false);
    setScore(null);

    if (cardIndex + 1 >= words.length) {
      const knownCount = known.length + (knew ? 1 : 0);
      const scorePercent = Math.round((knownCount / words.length) * 100);
      progressMutation.mutate({ lessonId: lessonId!, scorePercent });
      setFinished(true);
    } else {
      setCardIndex((prev) => prev + 1);
    }
  };

  const handleMic = () => {
    setRecording(true);
    setTimeout(() => {
      setScore([78, 83, 88, 91, 94][Math.floor(Math.random() * 5)]);
      setRecording(false);
    }, 1200);
  };

  if (finished) {
    const pct = Math.round((known.length / words.length) * 100);
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
        <div className="bg-white rounded-3xl border border-gray-100 p-8 md:p-10 w-full max-w-md text-center">
          <p className="text-6xl mb-4">
            {pct >= 80 ? "🎉" : pct >= 60 ? "👍" : "💪"}
          </p>
          <h2 className="text-3xl font-black text-gray-800 mb-2">{pct}%</h2>
          <p className="text-gray-500 mb-8">
            {known.length} / {words.length} {t("learned")}
          </p>
          <div className="flex gap-3">
            <button
              onClick={() => navigate(`/quiz/${lessonId}`)}
              className="flex-1 py-3 bg-red-600 text-white font-bold rounded-xl hover:bg-red-700 text-sm"
            >
              {t("startQuiz")}
            </button>
            <button
              onClick={() => navigate("/dashboard")}
              className="flex-1 py-3 border border-gray-200 text-gray-600 font-bold rounded-xl text-sm"
            >
              {t("goBack")}
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <div className="flex-1 min-w-0">
          <p className="font-black text-gray-800 text-sm truncate">
            {lesson?.titleAlbanian}
          </p>
        </div>
        <span className="text-xs font-bold bg-amber-100 text-amber-700 px-3 py-1 rounded-full flex-shrink-0">
          {t("xpEarned")}
        </span>
      </div>

      {/* Progress bar */}
      <div className="h-2 bg-gray-100">
        <div
          className="h-2 bg-red-500 transition-all duration-500"
          style={{ width: `${progress}%` }}
        />
      </div>
      <p className="text-center text-xs text-gray-400 font-semibold py-2">
        {cardIndex + 1} / {words.length}
      </p>

      <div className="max-w-md mx-auto px-4 py-2">
        {/* Flashcard */}
        <div
          onClick={() => setFlipped(!flipped)}
          className="cursor-pointer mb-4"
          style={{ perspective: "1000px" }}
        >
          <div
            style={{
              transformStyle: "preserve-3d",
              transition: "transform 0.5s",
              transform: flipped ? "rotateY(180deg)" : "rotateY(0deg)",
              position: "relative",
              height: "200px",
            }}
          >
            {/* Front — Albanian */}
            <div
              style={{
                backfaceVisibility: "hidden",
                WebkitBackfaceVisibility: "hidden",
              }}
              className="absolute inset-0 bg-white rounded-3xl border-2 border-blue-100 flex flex-col items-center justify-center gap-2 p-6"
            >
              <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">
                🇦🇱 Shqip
              </span>
              <p className="text-4xl md:text-5xl font-black text-gray-900 text-center">
                {current?.wordAlbanian}
              </p>
              {/* phonetic i fshirë me kërkesë */}
              <p className="text-xs text-gray-300">{t("tapToFlip")}</p>
            </div>

            {/* Back — Native language */}
            <div
              style={{
                backfaceVisibility: "hidden",
                WebkitBackfaceVisibility: "hidden",
                transform: "rotateY(180deg)",
              }}
              className="absolute inset-0 bg-white rounded-3xl border-2 border-teal-100 flex flex-col items-center justify-center gap-2 p-6"
            >
              <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">
                {langLabel[lang] ?? "🌍"}
              </span>
              <p className="text-3xl md:text-4xl font-black text-gray-900 text-center">
                {current
                  ? getWordInLang(current, lang as SupportedLanguage)
                  : ""}
              </p>
              {current?.exampleSentence && (
                <p className="text-xs md:text-sm text-teal-700 bg-teal-50 rounded-xl px-3 py-2 text-center leading-relaxed">
                  "{current.exampleSentence}"
                </p>
              )}
            </div>
          </div>
        </div>

        {/* Mic */}
        <div className="flex flex-col items-center gap-2 mb-4">
          <button
            onPointerDown={handleMic}
            className={`flex items-center gap-2 px-5 py-2 rounded-full border font-bold text-sm transition-all ${
              recording
                ? "bg-red-50 border-red-400 text-red-600"
                : "bg-white border-gray-200 text-gray-600"
            }`}
          >
            🎤 {recording ? t("listening") : t("pronounce")}
          </button>
          {score !== null && (
            <span
              className={`text-xs font-bold px-3 py-1 rounded-full ${
                score >= 80
                  ? "bg-teal-100 text-teal-700"
                  : "bg-amber-100 text-amber-700"
              }`}
            >
              {score >= 80 ? "✓" : "↻"} {score}%
            </span>
          )}
        </div>

        {/* Actions */}
        <div className="flex gap-3 mb-4">
          <button
            onClick={() => handleNext(false)}
            className="flex-1 py-3 md:py-4 rounded-2xl border-2 border-gray-200 bg-white font-black text-gray-500 hover:bg-red-50 hover:border-red-300 hover:text-red-500 transition-all text-sm"
          >
            {t("dontKnow")}
          </button>
          <button
            onClick={() => handleNext(true)}
            className="flex-1 py-3 md:py-4 rounded-2xl bg-teal-500 text-white font-black hover:bg-teal-600 transition-all text-sm"
          >
            {t("iKnow")}
          </button>
        </div>

        {/* Learned words */}
        {known.length > 0 && (
          <div className="bg-blue-50 rounded-2xl p-3 md:p-4">
            <p className="text-xs font-black text-blue-700 uppercase tracking-wide mb-2">
              {t("learned")}
            </p>
            <div className="flex flex-wrap gap-2">
              {known.map((id) => {
                const w = words.find((v) => v.id === id);
                return (
                  <span
                    key={id}
                    className="bg-blue-100 text-blue-700 text-xs font-bold px-3 py-1 rounded-full"
                  >
                    {w?.wordAlbanian}
                  </span>
                );
              })}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
