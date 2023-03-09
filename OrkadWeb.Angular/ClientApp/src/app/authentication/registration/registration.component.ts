import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormBuilder,
  UntypedFormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IFormBuilder, IFormGroup } from '@rxweb/types';
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
  formGroup: IFormGroup<RegistrationForm>;
  formBuilder: IFormBuilder;
  constructor(
    formBuilder: UntypedFormBuilder,
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.formBuilder = formBuilder;
  }

  get username(): UntypedFormControl {
    return this.formGroup.controls.username as UntypedFormControl;
  }
  get email(): UntypedFormControl {
    return this.formGroup.controls.email as UntypedFormControl;
  }
  get password(): UntypedFormControl {
    return this.formGroup.controls.password as UntypedFormControl;
  }
  get passwordConfirm(): UntypedFormControl {
    return this.formGroup.controls.passwordConfirm as UntypedFormControl;
  }

  ngOnInit(): void {
    this.initForm(
      this.route.snapshot.data['config'] as GlobalConfigurationResult
    );
  }

  initForm(config: GlobalConfigurationResult): void {
    this.formGroup = this.formBuilder.group<RegistrationForm>({
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

  displayErrorMessage(control: UntypedFormControl): string {
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
