import "./globals.css";
import type { Metadata } from "next";
import { Inter } from "next/font/google";
import Navbar from "@/components/Navbar";
import Provider from "./providers/Provider";
import { authOptions } from "./api/auth/[...nextauth]/route";
import { getServerSession } from "next-auth";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "dotnet-identity",
  description: "dotnet-identity",
};

export default async function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const session = await getServerSession(authOptions);
  return (
    <html lang="en">
      <Provider session={session}>
        <body className={inter.className}>
          <Navbar session={session} />
          <div className="container h-full mx-auto pt-5">{children}</div>
        </body>
      </Provider>
    </html>
  );
}
