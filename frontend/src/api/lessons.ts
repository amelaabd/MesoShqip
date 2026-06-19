import { apiClient } from "./client";
import type { Lesson, LessonDetail, LessonWithProgress } from "../types";

export const getLessons = async (level?: string): Promise<Lesson[]> => {
  const params = level ? { level } : {};
  const res = await apiClient.get<Lesson[]>("/api/v1/lessons", { params });
  return res.data;
};

export const getLessonById = async (id: string): Promise<LessonDetail> => {
  const res = await apiClient.get<LessonDetail>(`/api/v1/lessons/${id}`);
  return res.data;
};

export const getLessonsWithProgress = async (): Promise<
  LessonWithProgress[]
> => {
  const res = await apiClient.get<LessonWithProgress[]>(
    "/api/v1/lessons/with-progress",
  );
  return res.data;
};
