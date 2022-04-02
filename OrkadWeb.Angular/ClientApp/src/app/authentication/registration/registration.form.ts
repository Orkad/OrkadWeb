export interface RegistrationForm {
  /** (required) username 5 to 32 characters */
  username: string;
  /** (required) valid email adress */
  email: string;
  /** (required) password with at least 8 characters, one lower, one upper, and one special character */
  password: string;
  /** (required) password confirmation that should match with password */
  passwordConfirm: string;
}
