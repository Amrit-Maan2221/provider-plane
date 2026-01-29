import { useLocation, matchRoutes } from 'react-router-dom';
import { appRoutes } from '@/routes/routes.config';

export function usePageTitle() {
  const location = useLocation();

  const matches = matchRoutes(
    appRoutes.map(r => ({
      path: r.path,
      element: r.element,
      title: r.title
    })),
    location
  );

  return matches?.[0]?.route?.title ?? 'Provider Portal';
}
