export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  username: string;
  role: string;
}

export interface UserProfile {
  username: string;
  email: string;
  role: string;
  nativeLanguage: string;
  currentLevel: string;
  totalPoints: number;
  currentStreak: number;
  onboardingCompleted: boolean;
}

export interface Lesson {
  id: string;
  titleAlbanian: string;
  titleEnglish: string;
  level: string;
  lessonType: string;
  orderIndex: number;
  vocabularyCount: number;
}

export interface VocabularyItem {
  id: string;
  wordAlbanian: string;
  wordEnglish: string;
  phonetic?: string;
  exampleSentence?: string;
  audioFileUrl?: string;
  imageUrl?: string;
  difficultyScore: number;
}

export interface LessonDetail extends Lesson {
  vocabularyItems: VocabularyItem[];
}

export interface ProgressSummary {
  username: string;
  nativeLanguage: string;
  currentLevel: string;
  totalPoints: number;
  currentStreak: number;
  completedLessons: number;
  totalLessons: number;
}

export interface StoryResponse {
  storyId: string;
  titleAlbanian: string;
  bodyAlbanian: string;
  bodyTranslated: string;
  newWords: { albanian: string; translated: string; phonetic: string }[];
}

export interface QuizQuestion {
  questionText: string;
  correctAnswer: string;
  options: string[];
  explanation: string;
}
