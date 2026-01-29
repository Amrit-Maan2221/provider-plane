import React from 'react';

const kpis = [
  {
    label: 'Total Tenants',
    value: 42,
    trend: '+12%',
    trendType: 'up'
  },
  {
    label: 'Active Tenants',
    value: 36,
    trend: '+8%',
    trendType: 'up'
  },
  {
    label: 'Pending Activations',
    value: 6,
    trend: '-3%',
    trendType: 'down'
  },
  {
    label: 'Countries Covered',
    value: 9,
    trend: '+1',
    trendType: 'up'
  }
];

function DashboardPage() {
  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-semibold text-gray-800">
          Dashboard
        </h1>
        <p className="text-sm text-gray-500">
          Overview of your platform
        </p>
      </div>

      {/* KPI Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {kpis.map((kpi) => (
          <div
            key={kpi.label}
            className="bg-white rounded-lg shadow-sm border p-5"
          >
            <p className="text-sm text-gray-500">
              {kpi.label}
            </p>

            <div className="flex items-center justify-between mt-2">
              <span className="text-3xl font-bold text-gray-800">
                {kpi.value}
              </span>

              <span
                className={`text-sm font-medium ${
                  kpi.trendType === 'up'
                    ? 'text-green-600'
                    : 'text-red-600'
                }`}
              >
                {kpi.trend}
              </span>
            </div>
          </div>
        ))}
      </div>

      {/* Placeholder sections */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white border rounded-lg p-5 h-64 flex items-center justify-center text-gray-400">
          Tenant Growth (Chart)
        </div>
        <div className="bg-white border rounded-lg p-5 h-64 flex items-center justify-center text-gray-400">
          Recent Activity
        </div>
      </div>
    </div>
  );
}

export default DashboardPage;
