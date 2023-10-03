"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";

export async function login(data: {}) {
  return await fetchWrapper.post("/identity/login", data);
}

export async function authenticated() {
  return await fetchWrapper.get("/identity/authenticated");
}
