"use client";

import { useSession } from "next-auth/react";
import { useEffect } from "react";
import { authorized } from "./actions/actions";

export default function Home() {
  const { data: session } = useSession();

  useEffect(() => {
    authorized().then((res: any) => {
      //console.log(JSON.stringify(res));
    });
  }, []);

  console.log(session);

  return (
    <div>
      <h1>Home</h1>
      <h2>{session?.access_token}</h2>
    </div>
  );
}
