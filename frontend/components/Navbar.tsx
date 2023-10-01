"use client";

import * as React from "react";
import Link from "next/link";

import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
} from "@/components/ui/navigation-menu";
import Logo from "./Logo";
import { Button } from "./ui/button";

const MainNav = () => {
  return (
    <header className="inset-x-0 h-fit bg-background/95">
      <div className="container flex h-14 items-center justify-between">
        <Logo />
        <div className="flex content-center">
          <NavigationMenu>
            <NavigationMenuList>
              <NavigationMenuItem>
                <NavigationMenuLink href="/">Docs</NavigationMenuLink>
              </NavigationMenuItem>
            </NavigationMenuList>
          </NavigationMenu>
        </div>
        <div>
          <Button asChild>
            <Link href={"/login"}>Login</Link>
          </Button>
        </div>
      </div>
    </header>
  );
};

export default MainNav;
