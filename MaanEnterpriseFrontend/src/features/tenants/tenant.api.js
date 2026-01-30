import {  tenantClient } from "@/services/clients";

export const getTenantsPaged = (pageNumber = 1, pageSize = 10) =>
  tenantClient.get("/api/Tenants/paged", {
    params: { pageNumber, pageSize },
  });

export const getTenantById = (tenantId) =>
  tenantClient.get(`/api/Tenants/${tenantId}`);

export const createTenant = (payload) =>
  tenantClient.post("/api/Tenants", payload);