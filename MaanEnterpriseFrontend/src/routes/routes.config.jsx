import DashboardPage from "@/features/dashboard/pages/DashboardPage";
import CreateTenantPage from "@/features/tenants/pages/CreateTenant";
import TenantPage from "@/features/tenants/pages/TenantPage";

export const appRoutes = [
  {
    path: '/dashboard',
    element: <DashboardPage />,
    title: 'Dashboard'
  },
  {
    path: '/tenants',
    element: <TenantPage />,
    title: 'Tenants'
  },
  {
    path: '/tenants/create',
    element: <CreateTenantPage />,
    title: 'Create Tenant',
  },
];


export const defaultRoute = '/dashboard';