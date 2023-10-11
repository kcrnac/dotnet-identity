"use client";

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
import { RuquestCardType } from "@/types/index";

const RequestCard = ({ request }: { request: RuquestCardType }) => {
  const [response, setResponse] = useState<any>();

  function onButtonClick() {
    request
      .request()
      .then((res) => {
        setResponse(res);
      })
      .catch((res) => {
        console.log(res);
      });
  }

  return (
    <Card className="w-full mb-4 h-auto self-start">
      <CardHeader className="gap-1">
        <Badge className="self-start">{request.requestType}</Badge>
        <CardTitle>{request.title}</CardTitle>
        <CardDescription>{request.description}</CardDescription>
      </CardHeader>
      <CardContent>
        <Button onClick={() => onButtonClick()}>{request.buttonText}</Button>
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

            <pre className="mt-4 bg-slate-100 p-2 w-full overflow-y-auto">
              {JSON.stringify(response, null, 2)}
            </pre>
          </CardFooter>
        </>
      )}
    </Card>
  );
};

export default RequestCard;
