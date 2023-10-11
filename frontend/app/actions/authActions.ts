import { getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]/route";

export async function getSession() {
  return await getServerSession(authOptions);
}

export async function getCurrentUser() {
  try {
    const session = await getSession();

    if (!session) return null;

    return session.user;
  } catch (error) {
    return null;
  }
}

export async function getToken() {
  try {
    const session = await getSession();

    if (!session) return null;

    return session.access_token;
  } catch (error) {
    return null;
  }
}
