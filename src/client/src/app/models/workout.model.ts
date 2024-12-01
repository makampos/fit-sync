export interface Workout {
  workoutId: number;
  title: string;
  description: string;
  type: number; // define as enum later on
  bodyPart: string;
  equipment: string;
  level: number // define as enum late on
}
