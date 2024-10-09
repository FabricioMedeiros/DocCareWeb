import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Specialty } from '../../models/specialty';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/core/validators/generic-form-validation';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SpecialtyService } from '../../services/specialty.service';
import { fromEvent, merge, Observable } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormCanDeactivate } from 'src/app/core/guards/form-can-deactivate.interface';

@Component({
  selector: 'app-specialty-form',
  templateUrl: './specialty-form.component.html',
  styleUrls: ['./specialty-form.component.css']
})
export class SpecialtyFormComponent implements OnInit, AfterViewInit, FormCanDeactivate  {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  errors: any[] = [];
  specialtyForm!: FormGroup;
  specialty!: Specialty;

  validationMessages!: ValidationMessages;
  genericValidator!: GenericValidator;
  displayMessage: DisplayMessage = {};

  changesSaved: boolean = true;
  isEditMode: boolean = false;

  constructor(private fb: FormBuilder,
    private specialtyService: SpecialtyService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) {
    this.validationMessages = {
      description: {
        required: 'Informe a Descrição',
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.specialtyForm = this.fb.group({
      id: ['', []],
      description: ['', [Validators.required]],
    });

    const resolvedData = this.route.snapshot.data['specialty']; 

    if (resolvedData) {
      this.spinner.show();
      this.isEditMode = true;
      this.specialtyForm.patchValue(resolvedData.data);
      this.spinner.hide();
    }         
  }

  ngAfterViewInit(): void {
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.specialtyForm);

      this.changesSaved = false;
    });
  }

  saveSpecialty() {
    if (this.specialtyForm.dirty && this.specialtyForm.valid) {
      this.specialty = Object.assign({}, this.specialty, this.specialtyForm.value);

      if (this.isEditMode) {
        this.specialtyService.updateSpecialty(this.specialty).subscribe({
          next: (success) => {
            this.processSuccess(success);
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      } else {
        this.specialtyService.registerSpecialty(this.specialty).subscribe({
          next: (success) => {
            this.processSuccess(success);
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }

    this.changesSaved = true;
  }

  processSuccess(success: Specialty) {
    this.specialtyForm.reset();
    this.errors = [];

    let toast = this.toastr.success(this.isEditMode ? 'Especialidade alterada com sucesso!' : 'Especialidade cadastrada com sucesso!', 'Atenção!');

    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/specialty/list']);
      });
    }
  }

  processFail(fail: any) {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  cancel() {
    this.router.navigate(['/specialty/list']); 
  }  
}

