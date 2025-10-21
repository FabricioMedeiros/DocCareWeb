import { Component, OnInit } from '@angular/core';
import { forkJoin, map, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Chart, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js';

import { DashBoardService } from '../../services/dasboard.service';
import { DoctorService } from './../../../../shared/services/doctor.service';
import { Dashboard } from './../../models/dashboard';
import { Doctor } from '../../models/doctor';

@Component({
  selector: 'app-dashboard-data',
  templateUrl: './dashboard-data.component.html',
  styleUrls: ['./dashboard-data.component.css']
})
export class DashboardDataComponent implements OnInit {
  dashboardData: Dashboard = this.getEmptyDashboardData();

  doctors: Doctor[] = [];

  selectedDoctorId: string = '';
  startDate: string = '';
  endDate: string = '';
  errorMessage: string = '';

  financialChart: Chart | undefined;

  financialSummaryLabel: string = 'Financeiro';
  receivedLabel: string = 'Recebido';
  pendingLabel: string = 'Pendente';

  schedulePeriod: string = 'Agendamento(s) do Dia';

  constructor(
    private dashboardService: DashBoardService,
    private doctorService: DoctorService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    const today = new Date().toISOString().split('T')[0];
    this.startDate = today;
    this.endDate = today;

    this.updateLabels();
    this.loadDoctorsAndDashboard();
  }

  private loadDoctorsAndDashboard(): void {
    this.spinner.show();
    forkJoin([this.loadDoctors(), this.loadInitialDashboard()])
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe({
        next: () => { },
        error: err => {
          this.spinner.hide();
          this.errorMessage = 'Erro ao inicializar a dashboard';
          console.error('Erro init dashboard', err);
        }
      });
  }

  private loadInitialDashboard(): Observable<void> {
    return new Observable<void>(observer => {
      this.loadDashboardData();

      observer.next();
      observer.complete();
    });
  }

  loadDashboardData(): void {
    this.errorMessage = '';

    if (!this.startDate || !this.endDate) {
      this.errorMessage = 'Informe data inicial e final.';
      return;
    }

    let start = new Date(this.startDate);
    let end = new Date(this.endDate);

    if (start > end) {
      const tmp = start; start = end; end = tmp;
      this.startDate = start.toISOString().split('T')[0];
      this.endDate = end.toISOString().split('T')[0];
    }

    this.spinner.show();
    const startDateStr = start.toISOString().split('T')[0];
    const endDateStr = end.toISOString().split('T')[0];

    this.dashboardService.getDashboardData(this.selectedDoctorId, startDateStr, endDateStr).subscribe({
      next: (response) => this.processSuccess(response),
      error: (err) => this.processError(err)
    });
  }

  private processSuccess(response: any) {
    if (response && response.data) {
      console.log('dados:' + JSON.stringify(response.data));
      this.dashboardData = response.data;
      this.createFinancialChart();
    } else {
      this.dashboardData = this.getEmptyDashboardData();
    }
    this.spinner.hide();
  }

  private processError(err: any) {
    this.spinner.hide();
    this.errorMessage = 'Erro ao carregar dados';
    console.error('Erro ao carregar dados', err);
    this.toastr.error('Erro ao carregar dados');
  }

  getEmptyDashboardData(): Dashboard {
    return {
      totalPatients: 0,
      appointmentsByStatus: {},
      financialSummary: {
        totalPending: 0,
        totalReceived: 0
      }
    };
  }

  loadDoctors(): Observable<void> {
    return this.doctorService.getAll().pipe(map(response => {
      if (response && response.data) {
        this.doctors = response.data.items;
      } else {
        this.doctors = [];
      }
    }));
  }

  onDoctorChange(event: any): void {
    this.selectedDoctorId = event.target.value;
    this.loadDashboardData();
  }

  onSearch(): void {
    if (!this.startDate || !this.endDate) {
      this.errorMessage = 'Informe data inicial e final.';
      return;
    }

    const start = new Date(this.startDate);
    const end = new Date(this.endDate);

    if (start > end) {
      this.errorMessage = 'A data inicial não pode ser maior que a final.';
      return;
    }

    this.updateLabels();
    this.loadDashboardData();
  }

  updateLabels(): void {
    if (!this.startDate || !this.endDate) {
      this.financialSummaryLabel = 'Financeiro';
      this.receivedLabel = 'Recebido';
      this.pendingLabel = 'Pendente';
      return;
    }

    if (this.startDate === this.endDate) {
      this.schedulePeriod = 'Agendamento(s) do Dia';
      this.financialSummaryLabel = 'Financeiro do Dia';
    } else {
       this.schedulePeriod = 'Agendamento(s) do Período';
      this.financialSummaryLabel = 'Financeiro por Período';
    }
    this.receivedLabel = 'Recebido';
    this.pendingLabel = 'Pendente';
  }

  createFinancialChart(): void {
    Chart.register(BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend);

    if (this.financialChart) {
      this.financialChart.destroy();
    }

    this.financialChart = new Chart('financialChart', {
      type: 'bar',
      data: {
        labels: [this.pendingLabel, this.receivedLabel],
        datasets: [
          {
            label: this.financialSummaryLabel,
            data: [
              this.dashboardData.financialSummary.totalPending,
              this.dashboardData.financialSummary.totalReceived,
            ],
            backgroundColor: ['#ff7043', '#42a5f5'],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
          tooltip: {
            callbacks: {
              label: function (context) {
                const value = context.raw as number;
                return `R$ ${value.toLocaleString('pt-BR', {
                  minimumFractionDigits: 2,
                  maximumFractionDigits: 2,
                })}`;
              }
            }
          }
        },
        scales: {
          y: {
            ticks: {
              callback: function (tickValue: string | number): string {
                if (typeof tickValue === 'number') {
                  return `R$ ${tickValue.toLocaleString('pt-BR', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2,
                  })}`;
                }
                return tickValue.toString();
              }
            }
          }
        }
      },
    });
  }

  getAppointmentsStatusKeys(): string[] {
    return Object.keys(this.dashboardData.appointmentsByStatus);
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Scheduled': return 'Agendado(s)';
      case 'Confirmed': return 'Confirmado(s)';
      case 'Canceled': return 'Cancelado(s)';
      case 'Completed': return 'Concluído(s)';
      default: return 'Desconhecido';
    }
  }
}
