"use client";

import * as React from "react";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import Logo from "./Logo";
import { Session } from "next-auth";

import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
} from "@/components/ui/navigation-menu";

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { signOut } from "next-auth/react";
const MainNav = ({ session }: { session: Session | null }) => {
  return (
    <header className="inset-x-0 h-fit bg-background/95 mb- invert-0">
      <div className="container flex h-14 items-center justify-between">
        <Logo />
        <div className="flex-1 flex justify-center">
          <NavigationMenu>
            <NavigationMenuList>
              <NavigationMenuItem>
                <NavigationMenuLink href="/">Home</NavigationMenuLink>
              </NavigationMenuItem>
            </NavigationMenuList>
          </NavigationMenu>
        </div>
        <div className="flex flex-1">
          <div className="ml-auto">
            {session?.user?.name ? (
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="default">{session?.user?.name}</Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent className="w-56">
                  <DropdownMenuItem onClick={() => signOut()}>
                    Log out
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            ) : (
              <Link href={"/login"}>Login</Link>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};

export default MainNav;
