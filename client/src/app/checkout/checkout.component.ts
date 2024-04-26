import { Component } from '@angular/core';
import { BasketService } from '../basket/basket.service';
import { AccountService } from '../account/account.service';
import { IBasket, IBasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent {
  constructor(
    public basketService: BasketService,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe({
      next: (res) => {
        this.isUserAuthenticated = res;
      },
      error: (err) => {
        console.log(
          `An error occurred while setting isUserAuthenticated flag.`
        );
      },
    });
  }
  public isUserAuthenticated: boolean = false;

  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  orderNow(item: IBasket) {
    this.basketService.checkoutBasket(item);
  }
}
