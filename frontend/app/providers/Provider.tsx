"use client";

import { SessionProvider } from "next-auth/react";
import React from "react";

type Params = {
  children: any;
  session: any;
};

const Provider = ({ children, session }: Params) => {
  return <SessionProvider session={session}>{children}</SessionProvider>;
};

export default Provider;
