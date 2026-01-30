import { SidebarTrigger } from "@/components/ui/sidebar";
import { Bell, MessageSquare } from "lucide-react";
import { usePageTitle } from "@/routes/usePageTitle";
import { useTopbar } from "./TopbarContext";

export default function Topbar() {
  const pageTitle = usePageTitle();
  const { actions } = useTopbar();

  return (
    <header className="h-16 border-b bg-background flex items-center justify-between px-2 md:px-4">
      <div className="flex items-center gap-3">
        <SidebarTrigger />
        <h1 className="font-semibold text-xl hidden md:block">
          {pageTitle}
        </h1>
      </div>

      <div className="flex items-center gap-2">
        <div className="flex gap-2">
          {actions}
        </div>
        <button className="p-2 rounded-full hover:bg-muted">
          <MessageSquare className="h-5 w-5" />
        </button>
        <button className="p-2 rounded-full hover:bg-muted">
          <Bell className="h-5 w-5" />
        </button>
      </div>
    </header>
  );
}
