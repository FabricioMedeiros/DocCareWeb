import { DoctorService } from './../../../../shared/services/doctor.service';
import { Dashboard } from './../../models/dashboard';
import { Component, OnInit } from '@angular/core';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Chart, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js';

import { DashBoardService } from '../../services/dasboard.service';
import { Doctor } from '../../models/doctor';

@Component({
  selector: 'app-dashboard-data',
  templateUrl: './dashboard-data.component.html',
  styleUrls: ['./dashboard-data.component.css']
})
export class DashboardDataComponent implements OnInit {
  dashboardData: Dashboard = this.getEmptyDashboardData();

  doctors: Doctor[] = [];
  months = [
    { value: '1', name: 'Janeiro' },
    { value: '2', name: 'Fevereiro' },
    { value: '3', name: 'Março' },
    { value: '4', name: 'Abril' },
    { value: '5', name: 'Maio' },
    { value: '6', name: 'Junho' },
    { value: '7', name: 'Julho' },
    { value: '8', name: 'Agosto' },
    { value: '9', name: 'Setembro' },
    { value: '10', name: 'Outubro' },
    { value: '11', name: 'Novembro' },
    { value: '12', name: 'Dezembro' }];

  selectedDoctorId: string = '';
  selectedMonth: string = '';
  errorMessage: string = '';

  financialChart: Chart | undefined;

  // Variáveis para controlar os textos
  financialSummaryLabel: string = 'Financeiro do Mês';
  receivedLabel: string = 'Recebido';
  pendingLabel: string = 'Pendente';

  constructor(
    private dashboardService: DashBoardService,
    private doctorService: DoctorService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    const currentMonth = new Date().getMonth() + 1;
    this.selectedMonth = currentMonth.toString();
    this.updateLabels();
    this.loadDashboardData();
    this.loadDoctors();
  }

  loadDashboardData(): void {
    this.spinner.show();
    const { startDate, endDate } = this.getDateRange(this.selectedMonth);

    this.dashboardService.getDashboardData(this.selectedDoctorId, startDate, endDate).subscribe({
      next: (response) => this.processSuccess(response),
      error: (err) => this.processError(err)
    });
  }

  private processSuccess(response: any) {
    if (response && response.data) {
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
      dailyFinancialSummary: {
        totalPending: 0, 
        totalReceived: 0
      },
      monthlyFinancialSummary: {
        totalPending: 0, 
        totalReceived: 0
      }
    }
  }

  private getDateRange(month: string): { startDate: string; endDate: string } {
    const year = new Date().getFullYear();
    if (!month || month === '') {
      return {
        startDate: `${year}-01-01`,
        endDate: `${year}-12-31`
      };
    } else {
      const monthIndex = parseInt(month, 10) - 1;
      const startDate = new Date(year, monthIndex, 1);
      const endDate = new Date(year, monthIndex + 1, 0);
      return {
        startDate: startDate.toISOString().split('T')[0],
        endDate: endDate.toISOString().split('T')[0]
      };
    }
  }

  loadDoctors(): void {
    this.doctorService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.doctors = response.data.items;
        } else {
          this.doctors = [];
        }
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar os médicos';
        console.error('Erro ao carregar os médicos', err);
      }
    });
  }

  onDoctorChange(event: any): void {
    this.selectedDoctorId = event.target.value;
    this.loadDashboardData();
  }

  onMonthChange(event: any): void {
    this.selectedMonth = event.target.value;
    this.updateLabels();
    this.loadDashboardData();
  }

  updateLabels(): void {
    if (this.selectedMonth === '') {
      this.financialSummaryLabel = 'Financeiro Anual';
      this.receivedLabel = 'Recebido';
      this.pendingLabel = 'Pendente';
    } else {
      this.financialSummaryLabel = 'Financeiro do Mês';
      this.receivedLabel = 'Recebido';
      this.pendingLabel = 'Pendente';
    }
  }

  getAppointmentsStatusKeys(): string[] {
    return Object.keys(this.dashboardData.appointmentsByStatus);
  }

  createFinancialChart(): void {
    Chart.register(BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend);

    if (this.financialChart) { this.financialChart.destroy(); }

    this.financialChart = new Chart('financialChart', {
      type: 'bar',
      data: {
        labels: [this.pendingLabel + ' (Diário)', this.receivedLabel + ' (Diário)', this.pendingLabel, this.receivedLabel],
        datasets: [
          {
            label: this.financialSummaryLabel,
            data: [
              this.dashboardData.dailyFinancialSummary.totalPending,
              this.dashboardData.dailyFinancialSummary.totalReceived,
              this.dashboardData.monthlyFinancialSummary.totalPending,
              this.dashboardData.monthlyFinancialSummary.totalReceived,
            ],
            backgroundColor: ['#ff7043', '#42a5f5', '#ffca28', '#66bb6a'],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
        },
      },
    });
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Scheduled':
        return 'Agendado(s)';
      case 'Confirmed':
        return 'Confirmado(s)';
      case 'Canceled':
        return 'Cancelado(s)';
      case 'Completed':
        return 'Concluído(s)';
      default:
        return 'Desconhecido';
    }
  }
}


// import { DoctorService } from './../../../../shared/services/doctor.service';
// import { Dashboard } from './../../models/dashboard';
// import { Component, OnInit } from '@angular/core';

// import { NgxSpinnerService } from 'ngx-spinner';
// import { ToastrService } from 'ngx-toastr';
// import { Chart, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js';

// import { DashBoardService } from '../../services/dasboard.service';
// import { Doctor } from '../../models/doctor';

// @Component({
//   selector: 'app-dashboard-data',
//   templateUrl: './dashboard-data.component.html',
//   styleUrls: ['./dashboard-data.component.css']
// })
// export class DashboardDataComponent implements OnInit {
//   dashboardData: Dashboard = this.getEmptyDashboardData();

//   doctors: Doctor[] = [];
//   months = [
//     { value: '1', name: 'Janeiro' },
//     { value: '2', name: 'Fevereiro' },
//     { value: '3', name: 'Março' },
//     { value: '4', name: 'Abril' },
//     { value: '5', name: 'Maio' },
//     { value: '6', name: 'Junho' },
//     { value: '7', name: 'Julho' },
//     { value: '8', name: 'Agosto' },
//     { value: '9', name: 'Setembro' },
//     { value: '10', name: 'Outubro' },
//     { value: '11', name: 'Novembro' },
//     { value: '12', name: 'Dezembro' }];

//   selectedDoctorId: string = '';
//   selectedMonth: string = '';
//   errorMessage: string = '';

//   financialChart: Chart | undefined;

//   constructor(
//     private dashboardService: DashBoardService,
//     private doctorService: DoctorService,
//     private toastr: ToastrService,
//     private spinner: NgxSpinnerService
//   ) { }

//   ngOnInit(): void {
//     const currentMonth = new Date().getMonth() + 1; 
//     this.selectedMonth = currentMonth.toString();
//     this.loadDashboardData();
//     this.loadDoctors();
//   }

//   loadDashboardData(): void {
//     this.spinner.show();
//     const { startDate, endDate } = this.getDateRange(this.selectedMonth);

//     this.dashboardService.getDashboardData(this.selectedDoctorId, startDate, endDate).subscribe({
//       next: (response) => this.processSuccess(response),
//       error: (err) => this.processError(err)
//     });
//   }

//   private processSuccess(response: any) {
//     if (response && response.data) {
//       this.dashboardData = response.data;
//       this.createFinancialChart();
//     } else {
//       this.dashboardData = this.getEmptyDashboardData();
//     }
//     this.spinner.hide();
//   }

//   private processError(err: any) {
//     this.spinner.hide();
//     this.errorMessage = 'Erro ao carregar dados';
//     console.error('Erro ao carregar dados', err);
//     this.toastr.error('Erro ao carregar dados');
//   }

//   getEmptyDashboardData(): Dashboard {
//     return {
//       totalPatients: 0,
//       appointmentsByStatus: {},
//       dailyFinancialSummary: {
//         totalPending: 0, 
//         totalReceived: 0
//       },
//       monthlyFinancialSummary: {
//         totalPending: 0, 
//         totalReceived: 0
//       }
//     }
//   }

//   private getDateRange(month: string): { startDate: string; endDate: string } {
//     const year = new Date().getFullYear();
//     if (!month) {
//       return {
//         startDate: `${year}-01-01`,
//         endDate: `${year}-12-31`
//       };
//     } else {
//       const monthIndex = parseInt(month, 10) - 1;
//       const startDate = new Date(year, monthIndex, 1);
//       const endDate = new Date(year, monthIndex + 1, 0);
//       return {
//         startDate: startDate.toISOString().split('T')[0],
//         endDate: endDate.toISOString().split('T')[0]
//       };
//     }
//   }

//   loadDoctors(): void {
//     this.doctorService.getAll().subscribe({
//       next: (response) => {
//         if (response && response.data) {
//           this.doctors = response.data.items;
//         } else {
//           this.doctors = [];
//         }
//       },
//       error: (err) => {
//         this.errorMessage = 'Erro ao carregar os médicos';
//         console.error('Erro ao carregar os médicos', err);
//       }
//     });
//   }

//   onDoctorChange(event: any): void {
//     this.selectedDoctorId = event.target.value;
//     this.loadDashboardData();
//   }

//   onMonthChange(event: any): void {
//     this.selectedMonth = event.target.value;
//     this.loadDashboardData();
//   }

//   getAppointmentsStatusKeys(): string[] {
//     return Object.keys(this.dashboardData.appointmentsByStatus);
//   }

//   createFinancialChart(): void {
//     Chart.register(BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend);

//     if (this.financialChart) { this.financialChart.destroy(); }

//     this.financialChart = new Chart('financialChart', {
//       type: 'bar',
//       data: {
//         labels: ['Pendente (Diário)', 'Recebido (Diário)', 'Pendente (Mensal)', 'Recebido (Mensal)'],
//         datasets: [
//           {
//             label: 'Resumo Financeiro',
//             data: [
//               this.dashboardData.dailyFinancialSummary.totalPending,
//               this.dashboardData.dailyFinancialSummary.totalReceived,
//               this.dashboardData.monthlyFinancialSummary.totalPending,
//               this.dashboardData.monthlyFinancialSummary.totalReceived,
//             ],
//             backgroundColor: ['#ff7043', '#42a5f5', '#ffca28', '#66bb6a'],
//           },
//         ],
//       },
//       options: {
//         responsive: true,
//         plugins: {
//           legend: { display: false },
//         },
//       },
//     });
//   }

//   getStatusText(status: string): string {
//     switch (status) {
//       case 'Scheduled':
//         return 'Agendado(s)';
//       case 'Confirmed':
//         return 'Confirmado(s)';
//       case 'Canceled':
//         return 'Cancelado(s)';
//       case 'Completed':
//         return 'Concluído(s)';
//       default:
//         return 'Desconhecido';
//     }
//   }
// }
