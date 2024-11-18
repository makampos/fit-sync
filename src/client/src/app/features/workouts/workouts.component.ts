import {Component, OnInit} from '@angular/core';
import {Workout} from '../../models/workout.model';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {WorkoutService} from '../../core/services/workout.service';
import {NgForOf} from '@angular/common';
import {CeilPipe} from '../../shared/pipes/ceil.pipe';


import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import {FloatLabelModule} from 'primeng/floatlabel';
import { FocusTrapModule } from 'primeng/focustrap';
import { TableModule } from 'primeng/table';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-workouts',
  standalone: true,
  imports: [
    ButtonModule,
    InputTextModule,
    FocusTrapModule,
    ReactiveFormsModule,
    TableModule,
    CardModule,
    NgForOf,
    CeilPipe,
    FloatLabelModule,
  ],
  templateUrl: './workouts.component.html',
  styleUrl: './workouts.component.css'
})
export class WorkoutsComponent implements OnInit {
  workouts: Workout[] = []; // Property 'workouts' has no initializer and is not definitely assigned in the constructor.
  workoutForm!: FormGroup;
  editMode = false;
  editIndex!: number | undefined; // Add index to track which workout is being edited
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;

  constructor(private workoutService: WorkoutService, private fb: FormBuilder) {

  }

  ngOnInit(): void {
    this.initForm();
    this.loadWorkouts();
  }

  loadWorkouts() {
    this.workoutService.getWorkouts(this.currentPage, this.pageSize).subscribe((response) => {
      this.workouts = response.items; // Get workouts from response
      this.totalCount = response.totalCount; // Update total count for pagination
    });
  }

  initForm() {
    this.workoutForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      type: [0, Validators.required], // Adjust for enum
      bodyPart: ['', Validators.required],
      equipment: ['', Validators.required],
      level: [0, Validators.required], // Adjust for enum
    });
  }

  onSubmit() {
    if (this.editMode && this.editIndex !== undefined) {
      const updatedWorkout = {
        ...this.workoutForm.value,
        workoutId: this.workouts[this.editIndex].workoutId
      };
      this.workoutService.updateWorkout(updatedWorkout).subscribe(() => {
        this.loadWorkouts();
        this.resetForm()
      })
    } else {
      this.workoutService.createWorkout(this.workoutForm.value).subscribe(() => {
        this.loadWorkouts();
        this.resetForm();
      })
    }
  }

  onEdit(workoutId: number) {
    const workout = this.workouts.find(w => w.workoutId === workoutId);
    if (workout){
      this.workoutForm.patchValue(workout);
      this.editMode = true;
      this.editIndex = this.workouts.indexOf(workout) // Set the edit index
    }
  }

  onDelete(workoutId: number) {
    this.workoutService.deleteWorkout(workoutId).subscribe(() => {
      this.loadWorkouts();
    });
  }

  resetForm() {
    this.workoutForm.reset();
    this.editMode = false;
    this.editIndex = undefined;
  }

  // Pagination Methods
  nextPage() {
    if (this.currentPage < Math.ceil(this.totalCount / this.pageSize)) {
      this.currentPage++;
      this.loadWorkouts(); // Load workouts for the new page
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadWorkouts(); // Load workouts for the new page
    }
  }
}
