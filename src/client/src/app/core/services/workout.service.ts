import {Injectable} from '@angular/core';
import {Workout} from '../../models/workout.model';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {PaginatedResponse} from '../../models/paginatedResponse.model';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private apiUrl = 'http://localhost:5155/api/workouts';
  constructor(private http: HttpClient) { }

  getWorkouts(pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedResponse<Workout>> {
    return this.http.get<PaginatedResponse<Workout>>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getWorkoutById(id: number): Observable<Workout> {
    return this.http.get<Workout>(`${this.apiUrl}/{id}`);
  }

  createWorkout(workout: Workout): Observable<Workout> {
    return this.http.post<Workout>(this.apiUrl, workout);
  }

  // modify the endpoint to ass the id along in the route
  updateWorkout(workout: Workout): Observable<Workout> {
    return this.http.put<Workout>(`${this.apiUrl}/${workout.workoutId}`, workout)
  }

  deleteWorkout(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
