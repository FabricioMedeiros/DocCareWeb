import { AfterViewInit, ElementRef, ViewChildren, Directive } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { fromEvent, merge, Observable } from 'rxjs';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/core/validators/generic-form-validation';
import { FormCanDeactivate } from 'src/app/core/guards/form-can-deactivate.interface';

@Directive()
export abstract class BaseFormComponent<T> implements AfterViewInit, FormCanDeactivate {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

    form!: FormGroup;
    entity!: T;
    errors: any[] = [];
    validationMessages!: ValidationMessages;
    genericValidator!: GenericValidator;
    displayMessage: DisplayMessage = {};
    isEditMode: boolean = false;
    changesSaved: boolean = true;

    constructor(
        protected fb: FormBuilder,
        protected router: Router,
        protected route: ActivatedRoute,
        protected toastr: ToastrService,
        protected spinner: NgxSpinnerService
    ) { }

    abstract buildForm(): void;

    ngAfterViewInit(): void {
        const controlBlurs: Observable<any>[] = this.formInputElements
            .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

        merge(...controlBlurs).subscribe(() => {
            this.displayMessage = this.genericValidator.processMessages(this.form);
            this.changesSaved = false;
        });
    }

    initializeForm(resolvedData: any): void {
        if (resolvedData) {
            this.spinner.show();
            this.isEditMode = true;
            this.form.patchValue(resolvedData.data);
            this.spinner.hide();
        }
    }

    processSuccess(message: string, navigateTo: string): void {
        this.form.reset();
        this.errors = [];
        const toast = this.toastr.success(message, 'Atenção!');
        if (toast) {
            toast.onHidden.subscribe(() => this.router.navigate([navigateTo]));
        }
    }

    processFail(error: any): void {
        if (error?.status === 401) {
            this.spinner.hide();
            return;
        }

        this.errors = error.error.errors;
        this.toastr.error('Ocorreu um erro.', 'Atenção');
    }

    cancel(navigateTo: string): void {
        this.router.navigate([navigateTo]);
    }
}
