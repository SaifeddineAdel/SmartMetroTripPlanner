import { Routes } from '@angular/router';
import { adminGuard } from './core/auth/admin.guard';

export const routes: Routes = [
  {
    path: 'stations',
    loadComponent: () =>
      import('./features/stations/pages/stations-page/stations-page')
        .then(m => m.StationsPage)
  },
  {
    path: 'line-stations',
    loadComponent: () =>
      import('./features/line-stations/pages/line-stations-page/line-stations-page')
        .then(m => m.LineStationsPage)
  },
  {
    path: 'plan-trip',
    loadComponent: () =>
      import('./features/trip-planner/pages/trip-planner-page/trip-planner-page')
        .then(m => m.TripPlannerPage)
  },

  {
    path: 'admin/login',
    loadComponent: () =>
      import('./features/admin/pages/admin-login/admin-login')
        .then(m => m.AdminLogin)
  },

  {
    path: 'admin',
    canActivate: [adminGuard],
    loadComponent: () =>
      import('./features/admin/pages/admin-dashboard/admin-dashboard')
        .then(m => m.AdminDashboard),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/admin/pages/admin-dashboard/admin-home')
            .then(m => m.AdminHome)
      },
      {
        path: 'stations',
        loadComponent: () =>
          import('./features/admin/pages/stations-management/stations-management')
            .then(m => m.StationsManagement)
      },
      {
        path: 'metro-lines',
        loadComponent: () =>
          import('./features/admin/pages/metro-lines-management/metro-lines-management')
            .then(m => m.MetroLinesManagement)
      },
      {
        path: 'line-stations',
        loadComponent: () =>
          import('./features/admin/pages/line-stations-management/line-stations-management')
            .then(m => m.LineStationsManagement)
      }
    ]
  },

  {
    path: '',
    redirectTo: 'plan-trip',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'plan-trip'
  }
];