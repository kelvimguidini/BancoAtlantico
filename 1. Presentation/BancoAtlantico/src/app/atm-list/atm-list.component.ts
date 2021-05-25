import { Component, OnInit } from '@angular/core';

import { ATM } from '../_interfaces/atm';
import { Message } from '../_interfaces/message';
import { AtmService } from '../_services/atm.service';

@Component({
  selector: 'app-atm-list',
  templateUrl: './atm-list.component.html',
  styleUrls: ['./atm-list.component.css']
})
export class AtmListComponent implements OnInit {

  public message!: Message ;

  constructor(public atmService: AtmService) { }

  ngOnInit() {
    this.getListATMs();
    console.log(this.atmService.atms);

    this.atmService.startConnection();
    this.atmService.addTransferATMDataListener();
    //this.atmService.addNotesAlertDataListener();
  }

  getListATMs() {

    this.atmService.getATMs()
      .subscribe((atms: ATM[]) => {
        console.log(atms);
        this.atmService.atms = atms;
      }, () => {
        this.message = { description: 'Falha ao buscar caixas eletrônicos.', class: "bg-danger" }
      });
  }


  tornOff(atm: ATM) {

    if (confirm('Deseja desligar o caixa - "' + atm.name + '" ?')) {
      this.atmService.turnOff(atm.id)
        .subscribe((sucess: boolean) => {
          if (sucess) {
            
            this.message = { description: 'Desligado com sucesso.', class: "bg-success" }
          } else {
            this.message = { description: 'Falha ao desligar caixa eletrônico solicitação.', class: "bg-danger" }
          }
          console.log(this.message);
        }, () => {
          this.message = { description: 'Falha ao desligar caixa eletrônico solicitação.', class: "bg-danger" }
        });
    }

  }
}

