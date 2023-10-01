import type { JWT } from "next-auth/jwt";

declare module "next-auth" {
  interface Session {
    access_token: string;
    user: {
      name: string;
    } & DefaultSession["user"];
  }

  interface User {
    token: string;
    fullName: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    access_token: string;
  }
}
