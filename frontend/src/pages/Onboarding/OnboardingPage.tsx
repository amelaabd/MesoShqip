import { useState } from 'react'
import { useCompleteOnboarding } from '../../hooks/useAuth'

const LANGUAGES = [
  { code: 'en', flag: '🇬🇧', name: 'English' },
  { code: 'de', flag: '🇩🇪', name: 'Deutsch' },
  { code: 'it', flag: '🇮🇹', name: 'Italiano' },
  { code: 'fr', flag: '🇫🇷', name: 'Français' },
  { code: 'sv', flag: '🇸🇪', name: 'Svenska' },
  { code: 'tr', flag: '🇹🇷', name: 'Türkçe' },
]

const LEVELS = [
  { value: 1, emoji: '🌱', label: 'Fillestor', desc: 'Mezi di disa fjalë shqipe' },
  { value: 2, emoji: '📖', label: 'Mesatar',   desc: 'Kuptoj por dua të përmirësohem' },
  { value: 3, emoji: '🚀', label: 'Avancuar',  desc: 'Flas mirë, dua të perfeksionoj' },
]

export default function OnboardingPage() {
  const [step, setStep]         = useState(1)
  const [language, setLanguage] = useState('')
  const [level, setLevel]       = useState(0)
  const mutation                = useCompleteOnboarding()

  const handleFinish = () => {
    mutation.mutate({ nativeLanguage: language, level })
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-red-50 to-white flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        {/* Logo */}
        <div className="text-center mb-8">
          <h1 className="text-4xl font-black text-red-600">
            Mëso<span className="text-teal-500">Shqip</span>
          </h1>
          <p className="text-gray-400 text-sm mt-1">Hapi {step} nga 2</p>
          <div className="flex gap-2 justify-center mt-3">
            <div className={`h-2 w-16 rounded-full transition-all ${step >= 1 ? 'bg-red-500' : 'bg-gray-200'}`} />
            <div className={`h-2 w-16 rounded-full transition-all ${step >= 2 ? 'bg-red-500' : 'bg-gray-200'}`} />
          </div>
        </div>

        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
          {step === 1 && (
            <>
              <h2 className="text-xl font-black text-gray-800 mb-2">
                Çfarë gjuhe flet? 🌍
              </h2>
              <p className="text-sm text-gray-500 mb-6">
                Do të përdorim këtë gjuhë për përkthimet dhe shpjegimet.
              </p>
              <div className="grid grid-cols-2 gap-3 mb-6">
                {LANGUAGES.map((lang) => (
                  <button
                    key={lang.code}
                    onClick={() => setLanguage(lang.code)}
                    className={`p-4 rounded-2xl border-2 text-center transition-all ${
                      language === lang.code
                        ? 'border-red-500 bg-red-50'
                        : 'border-gray-100 hover:border-gray-300'
                    }`}
                  >
                    <div className="text-2xl mb-1">{lang.flag}</div>
                    <div className="text-sm font-bold text-gray-700">{lang.name}</div>
                  </button>
                ))}
              </div>
              <button
                onClick={() => setStep(2)}
                disabled={!language}
                className="w-full py-3 bg-red-600 text-white font-black rounded-xl hover:bg-red-700 transition-all disabled:opacity-40"
              >
                Vazhdo →
              </button>
            </>
          )}

          {step === 2 && (
            <>
              <h2 className="text-xl font-black text-gray-800 mb-2">
                Çfarë niveli ke? 📚
              </h2>
              <p className="text-sm text-gray-500 mb-6">
                Do të personalizojmë mësimet sipas nivelit tënd.
              </p>
              <div className="flex flex-col gap-3 mb-6">
                {LEVELS.map((lv) => (
                  <button
                    key={lv.value}
                    onClick={() => setLevel(lv.value)}
                    className={`p-4 rounded-2xl border-2 text-left transition-all ${
                      level === lv.value
                        ? 'border-red-500 bg-red-50'
                        : 'border-gray-100 hover:border-gray-300'
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
                  className="flex-1 py-3 border-2 border-gray-200 text-gray-600 font-bold rounded-xl hover:bg-gray-50"
                >
                  ← Prapa
                </button>
                <button
                  onClick={handleFinish}
                  disabled={!level || mutation.isPending}
                  className="flex-1 py-3 bg-red-600 text-white font-black rounded-xl hover:bg-red-700 transition-all disabled:opacity-40"
                >
                  {mutation.isPending ? 'Duke ruajtur...' : 'Fillo! 🦅'}
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  )
}