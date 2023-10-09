import { login } from "@/app/actions/actions";
import jwtDecode from "jwt-decode";
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
        token.expired = true;
      }

      if (token && token.access_token) {
        const decodedServerToken: any = jwtDecode(token.access_token);
        if (decodedServerToken) {
          token.exp = decodedServerToken.exp;
        }

        token.expired = token?.exp > 0 && Date.now() >= token.exp * 1000;
      }

      return token;
    },
    async session({ session, token }) {
      if (token) {
        if (token.expired) {
          return Promise.reject(session);
        }

        session.access_token = token.access_token;
        session.expires = token.exp.toString();
      }

      return session;
    },
  },
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
