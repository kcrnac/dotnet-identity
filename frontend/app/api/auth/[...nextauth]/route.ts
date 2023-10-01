import { login } from "@/app/actions/actions";
import NextAuth, { NextAuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: "jwt",
  },
  secret: "somesecret",
  debug: true,
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { type: "email" },
        password: { type: "password" },
      },
      async authorize(credentials, req) {
        console.log("==> authorized", JSON.stringify(credentials));
        try {
          const response = await login({
            email: credentials?.email,
            password: credentials?.password,
          });

          return response?.data;
        } catch (err) {}
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
