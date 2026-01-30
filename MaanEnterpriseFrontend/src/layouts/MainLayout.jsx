import { Outlet } from "react-router-dom";
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import AppSidebar from "@/layouts/AppSidebar";
import Topbar from "./Topbar";
import { TopbarProvider } from "./TopbarContext";

export default function MainLayout() {
  return (
    <SidebarProvider>
      <TopbarProvider>
      <div className="flex h-screen w-full">
        <AppSidebar />

        <div className="flex flex-1 flex-col">
          <Topbar />

          <main className="flex-1 overflow-y-auto p-2 md:p-4">
            <div className="max-w-7xl mx-auto">
              <Outlet />
            </div>
          </main>
        </div>
      </div>
      </TopbarProvider>
    </SidebarProvider>
  );
}
