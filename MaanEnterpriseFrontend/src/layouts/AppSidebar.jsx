import {
  Sidebar,
  SidebarContent,
  SidebarHeader,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarMenu,
  SidebarMenuItem,
  SidebarMenuButton,
} from "@/components/ui/sidebar";

import { NavLink } from "react-router-dom";
import { LayoutDashboard, Building2 } from "lucide-react";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import logo from "@/assets/branding/logo.png";

const navItems = [
  {
    title: "Dashboard",
    to: "/dashboard",
    icon: LayoutDashboard,
  },
  {
    title: "Tenants",
    to: "/tenants",
    icon: Building2,
  },
];

export default function AppSidebar() {
  return (
    <Sidebar>
      {/* Header */}
      <SidebarHeader>
        <div className="h-12 px-4 flex items-center gap-3"> 
          <img
            src={logo}
            alt="Maan Enterprise"
            className="h-8 w-8 object-contain rounded-lg"
          />

          <span className="font-semibold text-lg tracking-tight">
            Maan Enterprise
          </span>
        </div>
      </SidebarHeader>

      {/* Content */}
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu>
              {navItems.map((item) => {
                const Icon = item.icon;

                return (
                  <SidebarMenuItem key={item.to}>
                    <SidebarMenuButton asChild>
                      <NavLink
                        to={item.to}
                        className={({ isActive }) =>
                          isActive
                            ? "bg-indigo-100 text-indigo-700"
                            : ""
                        }
                      >
                        <Icon className="h-5 w-5" />
                        <span>{item.title}</span>
                      </NavLink>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      {/* Footer */}
      <SidebarFooter className="p-4">
        <div className="flex items-center gap-3">
          <Avatar>
            <AvatarFallback className="bg-indigo-200 text-indigo-700">
              JD
            </AvatarFallback>
          </Avatar>

          <div className="overflow-hidden">
            <p className="text-sm font-medium truncate">John Doe</p>
            <p className="text-xs text-muted-foreground truncate">
              john@maan.com
            </p>
          </div>
        </div>
      </SidebarFooter>
    </Sidebar>
  );
}
