import DashboardPage from "@/features/dashboard/pages/DashboardPage";

export const appRoutes = [
  {
    path: '/dashboard',
    element: <DashboardPage />,
    title: 'Dashboard'
  }
];


export const defaultRoute = '/dashboard';