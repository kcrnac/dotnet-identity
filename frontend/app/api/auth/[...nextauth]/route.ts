import { login } from "@/app/actions/actions";
import NextAuth, { NextAuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: "jwt",
  },
  debug: true,
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { type: "email" },
        password: { type: "password" },
      },
      async authorize(credentials) {
        try {
          const response = await login({
            email: credentials?.email,
            password: credentials?.password,
          });

          if (response?.status === 200) {
            return response;
          }

          return Promise.reject(new Error(response?.message));
        } catch (err) {
          throw err;
        }
      },
    }),
  ],
  callbacks: {
    async jwt({ token, user }) {
      if (user) {
        token.access_token = user.token;
        token.name = user.fullName;
      }

      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.access_token = token.access_token;
      }
      return session;
    },
  },
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
