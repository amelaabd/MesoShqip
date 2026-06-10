import { useState, useMemo } from "react";
import { useParams, useSearchParams, useNavigate } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { getLessonById } from "../../api/lessons";
import { updateProgress } from "../../api/progress";

interface Question {
  question: string;
  correct: number;
  options: string[];
}

function buildQuestions(words: any[], nativeLanguage: string): Question[] {
  return words.slice(0, 5).map((word, i) => {
    const correctWord =
      nativeLanguage === "en" ? word.wordEnglish : word.wordEnglish;

    const wrongWords = words
      .filter((_, idx) => idx !== i)
      .sort(() => Math.random() - 0.5)
      .slice(0, 3)
      .map((w) => w.wordEnglish);

    const shuffled = [...wrongWords, correctWord].sort(
      () => Math.random() - 0.5,
    );
    const correctIndex = shuffled.indexOf(correctWord);

    return {
      question: `Çfarë do të thotë "${word.wordAlbanian}"?`,
      correct: correctIndex,
      options: shuffled,
    };
  });
}

export default function QuizPage() {
  const { lessonId } = useParams<{ lessonId: string }>();
  const [searchParams] = useSearchParams();
  const childId = searchParams.get("childId") ?? "";
  const navigate = useNavigate();

  const [qIndex, setQIndex] = useState(0);
  const [chosen, setChosen] = useState<number | null>(null);
  const [answered, setAnswered] = useState(false);
  const [correct, setCorrect] = useState(0);
  const [finished, setFinished] = useState(false);

  const { data: lesson, isLoading } = useQuery({
    queryKey: ["lesson", lessonId],
    queryFn: () => getLessonById(lessonId!),
    enabled: !!lessonId,
  });

  const questions = useMemo(() => {
    if (!lesson?.vocabularyItems?.length) return [];
    return buildQuestions(lesson.vocabularyItems, "en");
  }, [lesson]);

  if (isLoading)
    return (
      <div className="min-h-screen flex items-center justify-center">
        <p className="text-gray-400 font-bold">Duke ngarkuar...</p>
      </div>
    );

  if (!questions.length)
    return (
      <div className="min-h-screen flex items-center justify-center">
        <p className="text-gray-400 font-bold">Nuk ka fjalë për kuiz.</p>
      </div>
    );

  const current = questions[qIndex];
  const progressPct = Math.round((qIndex / questions.length) * 100);

  const handleAnswer = (idx: number) => {
    if (answered) return;
    setChosen(idx);
    setAnswered(true);
    if (idx === current.correct) setCorrect((prev) => prev + 1);
  };

  const handleNext = () => {
    const isLast = qIndex + 1 >= questions.length;
    if (isLast) {
      const finalCorrect = correct + (chosen === current.correct ? 1 : 0);
      const scorePercent = Math.round((finalCorrect / questions.length) * 100);
      updateProgress({
        childProfileId: childId,
        lessonId: lessonId!,
        scorePercent,
      });
      setFinished(true);
    } else {
      setQIndex((prev) => prev + 1);
      setChosen(null);
      setAnswered(false);
    }
  };

  const handleRestart = () => {
    setFinished(false);
    setQIndex(0);
    setCorrect(0);
    setChosen(null);
    setAnswered(false);
  };

  if (finished) {
    const finalCorrect = correct;
    const pct = Math.round((finalCorrect / questions.length) * 100);
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
        <div className="bg-white rounded-3xl border border-gray-100 p-10 w-full max-w-md text-center">
          <p className="text-6xl mb-4">
            {pct >= 80 ? "🎉" : pct >= 60 ? "👍" : "💪"}
          </p>
          <h2 className="text-4xl font-black text-red-600 mb-1">{pct}%</h2>
          <p className="text-gray-500 mb-1">
            {finalCorrect} / {questions.length} saktë
          </p>
          <p className="font-bold text-lg text-gray-700 mb-8">
            {pct >= 80
              ? "Fantastike! Bravo!"
              : pct >= 60
                ? "Mirë, vazhdo!"
                : "Mos u dorëzo!"}
          </p>
          <div className="flex gap-3">
            <button
              onClick={handleRestart}
              className="flex-1 py-3 border-2 border-gray-200 text-gray-600 font-bold rounded-xl hover:bg-gray-50"
            >
              Provo sërish
            </button>
            <button
              onClick={() => navigate("/dashboard")}
              className="flex-1 py-3 bg-red-600 text-white font-bold rounded-xl hover:bg-red-700"
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
            Kuiz — {lesson?.titleAlbanian}
          </p>
        </div>
        <span className="text-xs font-bold bg-purple-100 text-purple-700 px-3 py-1 rounded-full">
          {qIndex + 1} / {questions.length}
        </span>
      </div>

      <div className="h-2 bg-gray-100">
        <div
          className="h-2 bg-red-500 transition-all duration-500"
          style={{ width: `${progressPct}%` }}
        />
      </div>

      <div className="max-w-md mx-auto px-4 py-8">
        <div className="bg-white rounded-3xl border border-gray-100 p-6 mb-6">
          <p className="text-xs font-black text-gray-400 uppercase tracking-wide mb-3">
            Pyetja {qIndex + 1}
          </p>
          <p className="text-xl font-black text-gray-800 leading-snug">
            {current.question}
          </p>
        </div>

        <div className="grid grid-cols-2 gap-3 mb-6">
          {current.options.map((opt, idx) => {
            let style =
              "bg-white border-2 border-gray-200 text-gray-700 hover:border-red-300 hover:bg-red-50";
            if (answered) {
              if (idx === current.correct)
                style = "bg-teal-500 border-2 border-teal-500 text-white";
              else if (idx === chosen)
                style = "bg-red-100 border-2 border-red-400 text-red-700";
              else style = "bg-gray-50 border-2 border-gray-100 text-gray-400";
            }
            return (
              <button
                key={idx}
                onClick={() => handleAnswer(idx)}
                className={`py-4 px-3 rounded-2xl font-bold text-sm text-center transition-all ${style}`}
              >
                {opt}
              </button>
            );
          })}
        </div>

        {answered && (
          <div
            className={`rounded-2xl p-4 mb-4 text-center font-bold ${
              chosen === current.correct
                ? "bg-teal-50 text-teal-700"
                : "bg-red-50 text-red-700"
            }`}
          >
            {chosen === current.correct
              ? "🎉 Saktë! Bravo!"
              : `❌ Gabim! Përgjigja: "${current.options[current.correct]}"`}
          </div>
        )}

        {answered && (
          <button
            onClick={handleNext}
            className="w-full py-4 bg-red-600 text-white font-black rounded-2xl hover:bg-red-700 transition-all"
          >
            {qIndex + 1 >= questions.length ? "Shiko rezultatin →" : "Vazhdo →"}
          </button>
        )}
      </div>
    </div>
  );
}
