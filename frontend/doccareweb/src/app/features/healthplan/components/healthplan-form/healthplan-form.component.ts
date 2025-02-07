import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { HealthPlan } from '../../models/healthplan';
import { HealthPlanService } from 'src/app/shared/services/healthplan.service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';

@Component({
  selector: 'app-health-plan-form',
  templateUrl: './healthplan-form.component.html',
  styleUrls: ['./healthplan-form.component.css']
})
export class HealthPlanFormComponent extends BaseFormComponent<HealthPlan> implements OnInit {

  constructor(
    protected override fb: FormBuilder,
    private healthPlanService: HealthPlanService, 
    protected override router: Router,
    protected override route: ActivatedRoute,
    protected override toastr: ToastrService,
    protected override spinner: NgxSpinnerService
  ) {
    super(fb, router, route, toastr, spinner);
    this.validationMessages = {
      description: {
        required: 'Informe o nome do plano'
      },
      cost: {
        required: 'Informe o preço do plano'
      }
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['healthPlan']; 

    if (resolvedData) {
      this.initializeForm(resolvedData);  
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      description: ['', [Validators.required]],  
      cost: ['', [Validators.required, Validators.pattern('^[0-9]*\.?[0-9]+$')]],  
    });
  }

  saveHealthPlan() {
    if (this.form.dirty && this.form.valid) {
      this.entity = Object.assign({}, this.entity, this.form.value);

      if (this.isEditMode) {
        this.healthPlanService.updateHealthPlan(this.entity).subscribe({
          next: () => {
            this.processSuccess('Plano de saúde alterado com sucesso!', '/healthplan/list');
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      } else {
        this.healthPlanService.registerHealthPlan(this.entity).subscribe({
          next: () => {
            this.processSuccess('Plano de saúde cadastrado com sucesso!', '/healthplan/list');
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }

    this.changesSaved = true;
  }
}


// import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
// import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';
// import { Router, ActivatedRoute } from '@angular/router';
// import { fromEvent, merge, Observable } from 'rxjs';

// import { NgxSpinnerService } from 'ngx-spinner';
// import { ToastrService } from 'ngx-toastr';

// import { ValidationMessages, GenericValidator, DisplayMessage } from 'src/app/core/validators/generic-form-validation';
// import { FormCanDeactivate } from 'src/app/core/guards/form-can-deactivate.interface';
// import { HealthPlan } from './../../models/healthplan';
// import { HealthPlanService } from '../../../../shared/services/healthplan.service';

// @Component({
//   selector: 'app-healthplan-form',
//   templateUrl: './healthplan-form.component.html',
//   styleUrls: ['./healthplan-form.component.css']
// })
// export class HealthplanFormComponent implements OnInit, AfterViewInit, FormCanDeactivate {
//   @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

//   errors: any[] = [];
//   healthPlanForm!: FormGroup;
//   healthPlan!: HealthPlan;

//   validationMessages!: ValidationMessages;
//   genericValidator!: GenericValidator;
//   displayMessage: DisplayMessage = {};

//   changesSaved: boolean = true;
//   isEditMode: boolean = false;

//   constructor(private fb: FormBuilder,
//     private healthPlanService: HealthPlanService,
//     private router: Router,
//     private route: ActivatedRoute,
//     private toastr: ToastrService,
//     private spinner: NgxSpinnerService) {
//     this.validationMessages = {
//       description: {
//         required: 'Informe a descrição',
//       },
//       cost: {
//         required: 'Informe o valor da consulta',
//       }
//     };

//     this.genericValidator = new GenericValidator(this.validationMessages);
//   }

//   ngOnInit(): void {
//     this.healthPlanForm = this.fb.group({
//       id: ['', []],
//       description: ['', [Validators.required]],
//       cost: ['0', [Validators.required]],
//     });
  
//     const resolvedData = this.route.snapshot.data['healthPlan'];
  
//     if (resolvedData) {
//       this.spinner.show();
//       this.isEditMode = true;
      
//       this.healthPlanForm.patchValue(resolvedData.data);
  
//       this.spinner.hide();
//     }
  
//     this.healthPlanForm.markAsPristine();
//   }  
  
//   ngAfterViewInit(): void {
//     let controlBlurs: Observable<any>[] = this.formInputElements
//       .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

//     merge(...controlBlurs).subscribe(() => {
//       this.displayMessage = this.genericValidator.processMessages(this.healthPlanForm);    

//       this.changesSaved = false;
//     });
//   }

//   saveHealthPlan() {
//     if (this.healthPlanForm.dirty && this.healthPlanForm.valid) {
//       this.healthPlan = Object.assign({}, this.healthPlan, this.healthPlanForm.value);

//       if (this.isEditMode) {
//         this.healthPlanService.updateHealthPlan(this.healthPlan).subscribe({
//           next: (success) => {
//             this.processSuccess(success);
//           },
//           error: (error) => {
//             this.processFail(error);
//           }
//         });
//       } else {
//         this.healthPlanService.registerHealthPlan(this.healthPlan).subscribe({
//           next: (success) => {
//             this.processSuccess(success);
//           },
//           error: (error) => {
//             this.processFail(error);
//           }
//         });
//       }
//     }

//     this.changesSaved = true;
//   }

//   processSuccess(success: HealthPlan) {
//     this.healthPlanForm.reset();
//     this.errors = [];

//     let toast = this.toastr.success(this.isEditMode ? 'Plano de saúde alterado com sucesso!' : 'Plano de saúde cadastrado com sucesso!', 'Atenção!');

//     if (toast) {
//       toast.onHidden.subscribe(() => {
//         this.router.navigate(['/healthplan/list']);
//       });
//     }
//   }

//   processFail(fail: any) {
//     this.errors = fail.error.errors;
//     this.toastr.error('Ocorreu um erro.', 'Atenção', { toastClass: 'ngx-toastr error-toast' });
//   }

//   cancel() {
//     this.router.navigate(['/healthplan/list']); 
//   }    
// }


