"use server";

import { axiosWrapper } from "@/lib/axiosWrapper";

export async function login(data: {}) {
  return await axiosWrapper.post("/identity/login", data);
}

export async function authorized() {
  return await axiosWrapper.get("/identity/authenticated");
}
