"use client";

import { useSession } from "next-auth/react";
import RequestCard from "@/components/RequestCard";

export default function Home() {
  const { data: session } = useSession();

  return (
    <div>
      <div className="grid grid-cols-3 max-md:grid-cols-1">
        <RequestCard />
        <RequestCard />
      </div>
    </div>
  );
}
