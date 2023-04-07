import { FormControl } from '@angular/forms';

export interface RegistrationForm {
  /** (required) username 5 to 32 characters */
  username: FormControl<string>;
  /** (required) valid email adress */
  email: FormControl<string>;
  /** (required) password with at least 8 characters, one lower, one upper, and one special character */
  password: FormControl<string>;
  /** (required) password confirmation that should match with password */
  passwordConfirm: FormControl<string>;
}
