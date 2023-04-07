import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
  FormBuilder,
  FormControl,
} from '@angular/forms';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { GlobalConfigurationResult } from 'src/api/results/GlobalConfigurationResult';
import { NotificationService } from 'src/services/notification.service';
import { AuthenticationService } from '../authentication.service';
import { RegistrationForm } from './registration.form';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
})
export class RegistrationComponent implements OnInit {
  formGroup: FormGroup<RegistrationForm>;
  constructor(
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private routeSnapshot: ActivatedRouteSnapshot,
    private router: Router
  ) {}

  get username(): FormControl {
    return this.formGroup.controls.username as FormControl;
  }
  get email(): FormControl {
    return this.formGroup.controls.email as FormControl;
  }
  get password(): FormControl {
    return this.formGroup.controls.password as FormControl;
  }
  get passwordConfirm(): FormControl {
    return this.formGroup.controls.passwordConfirm as FormControl;
  }

  ngOnInit(): void {
    this.initForm(
      this.routeSnapshot.data['config'] as GlobalConfigurationResult
    );
  }

  initForm(config: GlobalConfigurationResult): void {
    const formBuilder = new FormBuilder();
    this.formGroup = formBuilder.nonNullable.group({
      email: ['', [Validators.required, Validators.pattern(config.emailRegex)]],
      username: [
        '',
        [
          Validators.required,
          Validators.minLength(config.usernameMinLength),
          Validators.maxLength(config.usernameMaxLength),
          Validators.pattern(config.usernameRegex),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(config.passwordMinLength),
          Validators.maxLength(config.passwordMaxLength),
          Validators.pattern(config.passwordRegex),
        ],
      ],
      passwordConfirm: ['', [Validators.required, this.matchPassword]],
    });
  }

  displayErrorMessage(control: FormControl): string {
    if (control.hasError('required')) {
      return 'obligatoire';
    }
    if (control.hasError('email')) {
      return 'email invalide';
    }
    if (control.hasError('minlength')) {
      return 'trop court';
    }
    if (control.hasError('maxlength')) {
      return 'trop grand';
    }
    if (control.hasError('unmatch')) {
      return 'les mots de passes doivent correspondre';
    }
    if (control.hasError('pattern')) {
      switch (control) {
        case this.email:
          return 'email invalide';
        case this.username:
          return 'alphanumérique uniquement';
        case this.password:
          return 'au moins une majuscule, une minuscule, un chiffre et un caractère spécial';
      }
    }
    return '';
  }

  matchPassword: ValidatorFn = (
    control: AbstractControl
  ): ValidationErrors | null => {
    if (control.value === this.formGroup?.controls?.password.value) {
      return null;
    }
    return { unmatch: true };
  };

  register() {
    this.authenticationService
      .register(this.username.value, this.email.value, this.password.value)
      .subscribe({
        next: () => {
          this.router.navigate(['/auth']);
          this.notificationService.success('enregistrement effectué');
        },
        error: (err) => {
          console.error(err);
          this.notificationService.error(err);
        },
      });
  }
}
