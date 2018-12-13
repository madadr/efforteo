import {FormControl} from '@angular/forms';

export function CategoryValidator(categories: string[]) {
  let categoryControl: FormControl;

  return (control: FormControl) => {
    if (!control.parent) {
      return null;
    }

    categoryControl = control;

    if (categories.indexOf(categoryControl.value.toLowerCase()) === -1) {
      return {
        mustBeValid: true
      };
    }

    return null;
  };
}
