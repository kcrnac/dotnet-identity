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
    <header className="inset-x-0 h-fit bg-background/95 mb-3">
      <div className="container flex h-14 items-center justify-between">
        <Logo />
        <div className="flex content-center">
          <NavigationMenu>
            <NavigationMenuList>
              <NavigationMenuItem>
                <NavigationMenuLink href="/">Home</NavigationMenuLink>
              </NavigationMenuItem>
            </NavigationMenuList>
          </NavigationMenu>
        </div>
        <div>
          <Button asChild>
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
          </Button>
        </div>
      </div>
    </header>
  );
};

export default MainNav;
