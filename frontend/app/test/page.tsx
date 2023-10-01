import React from "react";
import { getCurrentUser, getToken } from "../actions/authActions";

const page = () => {
  var test = getToken();

  return <div>{test}</div>;
};

export default page;
