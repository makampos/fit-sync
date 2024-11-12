import { Routes } from '@angular/router';
import { WorkoutsComponent } from './features/workouts/workouts.component';

export const routes: Routes = [
  { path: '', redirectTo: '/workouts', pathMatch: 'full' }, // Redirect to workouts on load
  { path: 'workouts', component: WorkoutsComponent }, // Define route for workouts
  { path: '**', redirectTo: '/workouts' } // Wildcard route for a 404 page or redirect
];
