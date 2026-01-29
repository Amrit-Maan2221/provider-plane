import { Menu, Bell, MessageSquare } from "lucide-react";

export default function Topbar({ onMenuClick }) {
  return (
    <header className="h-16 bg-white border-b border-gray-200 flex items-center justify-between px-2 md:px-4">
      <div className="flex items-center gap-4">
        <button onClick={onMenuClick} className="p-2 rounded-md lg:hidden hover:bg-gray-100">
         <Menu className="w-6 h-6 text-gray-600" />
        </button>
        <h1 className="font-semibold text-gray-800 hidden md:block">Provider Portal</h1>
      </div>

      {/* Utilities: Notifications, Chat, etc. */}
      <div className="flex items-center gap-1 md:gap-3">
        {/* Chat Icon */}
        <button className="p-2 text-gray-500 hover:bg-gray-50 rounded-full relative transition-colors">
            <MessageSquare className="w-6 h-6" />
        
          <span className="absolute top-2 right-2 h-2 w-2 bg-red-500 rounded-full border-2 border-white"></span>
        </button>

        {/* Notification Icon */}
        <button className="p-2 text-gray-500 hover:bg-gray-50 rounded-full relative transition-colors">
          <Bell className="w-6 h-6" />
          <span className="absolute top-2 right-2.5 h-2 w-2 bg-indigo-600 rounded-full border-2 border-white"></span>
        </button>

        <div className="h-6 w-[1px] bg-gray-200 mx-1"></div>

        <button className="text-sm font-medium text-gray-600 hover:text-indigo-600 p-2">
          Settings
        </button>
      </div>
    </header>
  );
}