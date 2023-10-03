"use client";

import { authenticated } from "@/app/actions/actions";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { XIcon } from "lucide-react";
import { useState } from "react";

const RequestCard = () => {
  const [response, setResponse] = useState<any>();

  function onAuthenticatedClick() {
    authenticated()
      .then((res) => {
        setResponse(res);
      })
      .catch((res) => {
        console.log(res);
      });
  }

  return (
    <Card className="w-full mb-4">
      <CardHeader className="gap-1">
        <Badge className="self-start">GET</Badge>
        <CardTitle>Authenticated</CardTitle>
        <CardDescription>
          Check whether current user is authenticated
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Button onClick={() => onAuthenticatedClick()}>Test</Button>
      </CardContent>

      {response && (
        <>
          <hr className="mb-4" />
          <CardFooter className="flex flex-col items-start">
            <div className="flex justify-between w-full">
              {response.status === 200 ? (
                <Badge className="self-start bg-green-600 hover:bg-green-700">
                  {response.status}
                </Badge>
              ) : (
                <Badge className="self-start" variant="destructive">
                  {response.status}
                </Badge>
              )}
              <XIcon onClick={() => setResponse(null)} />
            </div>

            <pre className="mt-4 bg-slate-100 p-2 w-full">
              {JSON.stringify(response, null, 2)}
            </pre>
          </CardFooter>
        </>
      )}
    </Card>
  );
};

export default RequestCard;
