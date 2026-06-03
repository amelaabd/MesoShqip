import { apiClient } from "./client";
import type { ChildProfile } from "../types";

export const getChildren = async (): Promise<ChildProfile[]> => {
  const res = await apiClient.get<ChildProfile[]>("/api/v1/children");
  return res.data;
};

export const createChild = async (data: {
  displayName: string;
  avatarCode: string;
  nativeLanguage: string;
  startingLevel: number;
}): Promise<ChildProfile> => {
  const res = await apiClient.post<ChildProfile>("/api/v1/children", data);
  return res.data;
};
