import {FormGroup} from '@angular/forms';

export function MustBeTheSame(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup) => {
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];

    if (matchingControl.errors && !matchingControl.errors.mustBeTheSame) {
      return;
    }

    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ MustBeTheSame: true });
    } else {
      matchingControl.setErrors(null);
    }
  };
}
