import { FormControl } from '@angular/forms';

export function ConfirmPasswordValidator(passwordInput: string) {
  let confirmPasswordControl: FormControl;
  let passwordControl: FormControl;

  return (control: FormControl) => {
    if (!control.parent) {
      return null;
    }

    if (!confirmPasswordControl) {
      confirmPasswordControl = control;
      passwordControl = control.parent.get(passwordInput) as FormControl;
      passwordControl.valueChanges.subscribe(() => {
        confirmPasswordControl.updateValueAndValidity();
      });
    }

    if (
      passwordControl.value !==
      confirmPasswordControl.value
    ) {
      return {
        mustMatch: true
      };
    }
    return null;
  };
}
