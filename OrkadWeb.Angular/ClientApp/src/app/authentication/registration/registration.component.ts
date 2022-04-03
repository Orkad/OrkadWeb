import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IFormBuilder, IFormGroup } from '@rxweb/types';
import { GlobalConfigurationResult } from 'src/api/results/GlobalConfigurationResult';
import { ConfigurationService } from 'src/services/configuration.service';
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
    formBuilder: FormBuilder,
    private configurationService: ConfigurationService,
    private route: ActivatedRoute
  ) {
    this.formBuilder = formBuilder;
  }

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
      this.route.snapshot.data['config'] as GlobalConfigurationResult
    );
  }

  initForm(config: GlobalConfigurationResult): void {
    this.formGroup = this.formBuilder.group<RegistrationForm>({
      email: ['', [Validators.required, Validators.email]],
      username: [
        '',
        [
          Validators.required,
          Validators.minLength(config.usernameMinLength),
          Validators.maxLength(config.passwordMaxLength),
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
        case this.username:
          return 'alphanumérique uniquement';
        case this.password:
          return 'au moins une majuscule, une minuscule et un caractère spécial';
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

  register() {}
}
