"use client";

import { useSession } from "next-auth/react";
import RequestCard from "@/components/RequestCard";
import { requestTypes } from "@/lib/requestTypes";

export default function Home() {
  const { data: session } = useSession();

  return (
    <div>
      <div className="grid grid-cols-2 max-md:grid-cols-1 max-lg:grid-cols-2 gap-4">
        {requestTypes.map((rt) => (
          <RequestCard request={rt} key={rt.title} />
        ))}
      </div>
    </div>
  );
}
