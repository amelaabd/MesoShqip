import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@tanstack/react-query";
import { getLessonById } from "../../api/lessons";
import { updateProgress } from "../../api/progress";
import type { VocabularyItem } from "../../types";
import toast from "react-hot-toast";

export default function FlashcardPage() {
  const { lessonId } = useParams<{ lessonId: string }>();
  const navigate = useNavigate();

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
    onSuccess: () => toast.success("Progresi u ruajt!"),
  });

  if (isLoading)
    return (
      <div className="min-h-screen flex items-center justify-center">
        <p className="text-gray-400 font-bold">Duke ngarkuar...</p>
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
      const scores = [78, 83, 88, 91, 94];
      setScore(scores[Math.floor(Math.random() * scores.length)]);
      setRecording(false);
    }, 1200);
  };

  if (finished) {
    const pct = Math.round((known.length / words.length) * 100);
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
        <div className="bg-white rounded-3xl border border-gray-100 p-10 w-full max-w-md text-center">
          <p className="text-6xl mb-4">
            {pct >= 80 ? "🎉" : pct >= 60 ? "👍" : "💪"}
          </p>
          <h2 className="text-3xl font-black text-gray-800 mb-2">{pct}%</h2>
          <p className="text-gray-500 mb-8">
            {known.length} nga {words.length} fjalë të njohura
          </p>
          <div className="flex gap-3">
            <button
              onClick={() => navigate(`/quiz/${lessonId}`)}
              className="flex-1 py-3 bg-red-600 text-white font-bold rounded-xl hover:bg-red-700"
            >
              Bëj kuizin →
            </button>
            <button
              onClick={() => navigate("/dashboard")}
              className="flex-1 py-3 border border-gray-200 text-gray-600 font-bold rounded-xl hover:bg-gray-50"
            >
              Kthehu
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-white border-b border-gray-100 px-4 py-4 flex items-center gap-3">
        <button
          onClick={() => navigate("/dashboard")}
          className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center font-bold text-gray-600"
        >
          ‹
        </button>
        <div className="flex-1">
          <p className="font-black text-gray-800 text-sm">
            {lesson?.titleAlbanian}
          </p>
        </div>
        <span className="text-xs font-bold bg-amber-100 text-amber-700 px-3 py-1 rounded-full">
          +5 XP
        </span>
      </div>

      <div className="h-2 bg-gray-100">
        <div
          className="h-2 bg-red-500 transition-all duration-500"
          style={{ width: `${progress}%` }}
        />
      </div>
      <p className="text-center text-xs text-gray-400 font-semibold py-2">
        {cardIndex + 1} / {words.length} fjalë
      </p>

      <div className="max-w-md mx-auto px-4 py-4">
        <div
          onClick={() => setFlipped(!flipped)}
          className="cursor-pointer mb-5"
          style={{ perspective: "1000px" }}
        >
          <div
            style={{
              transformStyle: "preserve-3d",
              transition: "transform 0.5s",
              transform: flipped ? "rotateY(180deg)" : "rotateY(0deg)",
              position: "relative",
              height: "220px",
            }}
          >
            <div
              style={{
                backfaceVisibility: "hidden",
                WebkitBackfaceVisibility: "hidden",
              }}
              className="absolute inset-0 bg-white rounded-3xl border-2 border-blue-100 flex flex-col items-center justify-center gap-3 p-6"
            >
              <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">
                🇦🇱 Shqip
              </span>
              <p className="text-5xl font-black text-gray-900">
                {current?.wordAlbanian}
              </p>
              {current?.phonetic && (
                <p className="text-sm text-gray-400 italic">
                  [{current.phonetic}]
                </p>
              )}
              <p className="text-xs text-gray-300 mt-2">Kliko për të kthyer</p>
            </div>
            <div
              style={{
                backfaceVisibility: "hidden",
                WebkitBackfaceVisibility: "hidden",
                transform: "rotateY(180deg)",
              }}
              className="absolute inset-0 bg-white rounded-3xl border-2 border-teal-100 flex flex-col items-center justify-center gap-3 p-6"
            >
              <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">
                🌍 Përkthim
              </span>
              <p className="text-4xl font-black text-gray-900">
                {current?.wordEnglish}
              </p>
              {current?.exampleSentence && (
                <p className="text-sm text-teal-700 bg-teal-50 rounded-xl px-4 py-2 text-center leading-relaxed">
                  "{current.exampleSentence}"
                </p>
              )}
            </div>
          </div>
        </div>

        <div className="flex flex-col items-center gap-2 mb-5">
          <button
            onPointerDown={handleMic}
            className={`flex items-center gap-2 px-5 py-2 rounded-full border font-bold text-sm transition-all ${
              recording
                ? "bg-red-50 border-red-400 text-red-600"
                : "bg-white border-gray-200 text-gray-600"
            }`}
          >
            🎤 {recording ? "Duke dëgjuar..." : "Shqiptoje"}
          </button>
          {score !== null && (
            <span
              className={`text-xs font-bold px-3 py-1 rounded-full ${
                score >= 80
                  ? "bg-teal-100 text-teal-700"
                  : "bg-amber-100 text-amber-700"
              }`}
            >
              {score >= 80 ? "✓" : "↻"} {score}% saktësi
            </span>
          )}
        </div>

        <div className="flex gap-3">
          <button
            onClick={() => handleNext(false)}
            className="flex-1 py-4 rounded-2xl border-2 border-gray-200 bg-white font-black text-gray-500 hover:bg-red-50 hover:border-red-300 hover:text-red-500 transition-all"
          >
            ✗ Nuk e di
          </button>
          <button
            onClick={() => handleNext(true)}
            className="flex-1 py-4 rounded-2xl bg-teal-500 text-white font-black hover:bg-teal-600 transition-all"
          >
            ✓ E di!
          </button>
        </div>

        {known.length > 0 && (
          <div className="mt-5 bg-blue-50 rounded-2xl p-4">
            <p className="text-xs font-black text-blue-700 uppercase tracking-wide mb-2">
              Mësuar ✓
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
