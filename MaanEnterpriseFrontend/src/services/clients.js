import { createHttpClient } from "./httpClient";

export const tenantClient = createHttpClient(
  import.meta.env.VITE_TENANT_API_URL
);