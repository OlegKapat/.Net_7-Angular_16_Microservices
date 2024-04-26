import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'client';
    constructor(private basketService: BasketService, private accountService: AccountService) {}

    ngOnInit(): void {

      const basket_username = localStorage.getItem('basket_username');
      if(basket_username){
        this.basketService.getBasket(basket_username);
      }
    }
}
