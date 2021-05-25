import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AtmListComponent } from './atm-list/atm-list.component';

const routes: Routes = [
  {
    path: 'atmlist',
    component: AtmListComponent,
    data: { title: 'Lista de caixas eletrônicos ativos' }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }

