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
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return await handleResponse(response);
}

async function getHeaders() {
  const token = await getToken();
  const headers = { "Content-Type": "application/json" } as any;
  if (token) {
    headers.Authorization = "Bearer " + token;
  }

  return headers;
}

async function handleResponse(response: any) {
  let data = await response.text();
  const statusCode = response.status;

  try {
    data = JSON.parse(data);
  } catch (error) {}

  if (response.ok) {
    return { ...data, status: statusCode };
  } else {
    const message =
      typeof data === "string" && data.length > 0 ? data : response.statusText;

    return { message, status: statusCode };
  }
}

export const fetchWrapper = {
  post,
  get,
};
