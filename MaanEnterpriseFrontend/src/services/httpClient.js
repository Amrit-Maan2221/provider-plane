import axios from "axios";

export const createHttpClient = (baseURL) => {
  const client = axios.create({
    baseURL,
    timeout: 15000,
    headers: {
      "Content-Type": "application/json",
    },
  });

  return client;
};
