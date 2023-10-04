import { anonymous, authenticated, authorized } from "@/app/actions/actions";
import { RuquestCardType } from "@/types";

export const requestTypes: RuquestCardType[] = [
  {
    title: "Authenticated",
    description: "Check whether current user is authenticated",
    buttonText: "Test",
    requestType: "GET",
    request: authenticated,
  },
  {
    title: "Authorized",
    description: "Check whether current user is authorized",
    buttonText: "Test",
    requestType: "GET",
    request: authorized,
  },
  {
    title: "Anonymous",
    description: "Check endpoint which allows anonymous access",
    buttonText: "Test",
    requestType: "GET",
    request: anonymous,
  },
];
