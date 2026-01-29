import { Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from '@/layouts/MainLayout';
import { appRoutes } from './routes.config';

export default function AppRoutes() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route path="/" element={<Navigate to="/dashboard" />} />
        {appRoutes.map(({ path, element }) => (
            <Route key={path} path={path} element={element} />
        ))}
      </Route>
    </Routes>
  );
}
