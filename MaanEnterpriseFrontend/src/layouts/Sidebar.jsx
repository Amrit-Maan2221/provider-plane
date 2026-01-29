import { NavLink } from 'react-router-dom';
import { LayoutDashboard, Building2 } from 'lucide-react';

const navItems = [
  {
    label: 'Dashboard',
    to: '/dashboard',
    icon: LayoutDashboard,
  },
  {
    label: 'Tenants',
    to: '/tenants',
    icon: Building2,
  },
];

export default function Sidebar({ isOpen, setIsOpen }) {
  return (
    <aside
      className={`
        fixed inset-y-0 left-0 z-30 w-64
        bg-gray-100
        border-r border-gray-300
        transform transition-transform duration-300 ease-in-out
        flex flex-col
        lg:relative lg:translate-x-0
        ${isOpen ? 'translate-x-0' : '-translate-x-full'}
      `}
    >
      {/* Brand Header */}
      <div className="h-16 flex items-center px-3 border-b border-gray-300">
        <span className="bg-indigo-600 text-white p-1.5 rounded-lg mr-3 text-xs font-semibold">
          ME
        </span>
        <span className="text-gray-900 font-semibold text-lg">
          Maan Enterprise
        </span>
      </div>

      {/* Navigation */}
      <nav className="flex-1 mt-4 px-2 space-y-1">
        {navItems.map(item => {
          const Icon = item.icon;

          return (
            <NavLink
              key={item.to}
              to={item.to}
              onClick={() => setIsOpen(false)}
              className={({ isActive }) => `
                group flex items-center gap-3 px-4 py-2.5 rounded-md
                text-sm font-medium transition-all
                ${
                  isActive
                    ? 'bg-indigo-100 text-indigo-700 shadow-sm'
                    : 'text-gray-700 hover:bg-gray-200 hover:text-gray-900'
                }
              `}
            >
              <Icon
                className={`
                  h-5 w-5
                  ${
                    item.to
                      ? 'group-hover:scale-105 transition-transform'
                      : ''
                  }
                `}
              />
              {item.label}
            </NavLink>
          );
        })}
      </nav>

      {/* User Footer */}
      <div className="px-2 py-4 border-t border-gray-300 bg-gray-100">
        <div className="flex items-center gap-3">
          <div className="h-9 w-9 rounded-full bg-indigo-200 text-indigo-700 flex items-center justify-center font-semibold">
            JD
          </div>
          <div className="flex-1 overflow-hidden">
            <p className="text-sm font-medium text-gray-900 truncate">
              John Doe
            </p>
            <p className="text-xs text-gray-600 truncate">
              john@maan.com
            </p>
          </div>
        </div>
      </div>
    </aside>
  );
}
