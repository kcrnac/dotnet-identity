import "./globals.css";
import type { Metadata } from "next";
import { Inter } from "next/font/google";
import Navbar from "@/components/Navbar";
import Provider from "./providers/Provider";
import { Session } from "next-auth";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "dotnet-identity",
  description: "dotnet-identity",
};

export default function RootLayout({
  children,
  session,
}: {
  children: React.ReactNode;
  session: Session;
}) {
  return (
    <html lang="en">
      <Provider session={session}>
        <body className={inter.className}>
          <Navbar />
          <div className="container h-full mx-auto">{children}</div>
        </body>
      </Provider>
    </html>
  );
}
