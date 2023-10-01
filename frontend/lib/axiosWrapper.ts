import { getToken } from "@/app/actions/authActions";

const baseUrl = process.env.API_URL;

async function post(url: string, body: {}) {
  const requestOptions = {
    method: "POST",
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function get(url: string) {
  const requestOptions = {
    method: "GET",
    header: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function getHeaders() {
  const token = await getToken();
  const headers = { "Content-type": "application/json" } as any;
  if (token) {
    headers.Authorization = "Bearer " + token;
  }

  console.log("HEADERS" + JSON.stringify(headers));

  return headers;
}

async function handleResponse(response: Response) {
  const text = await response.text();
  // const data = text && JSON.parse(text);

  console.log("TEXT" + response.status);
  let data;
  try {
    data = JSON.parse(text);
  } catch (error) {
    data = text;
  }

  if (response.ok) {
    return data || response.statusText;
  } else {
    const error = {
      status: response.status,
      message:
        typeof data === "string" && data.length > 0
          ? data
          : response.statusText,
    };
    return { error };
  }
}

export const axiosWrapper = {
  post,
  get,
};
