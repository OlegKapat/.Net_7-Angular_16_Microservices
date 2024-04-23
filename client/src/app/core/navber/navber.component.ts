import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-navbar',
  templateUrl: './navber.component.html',
  styleUrl: './navber.component.scss',
})
export class NavbarComponent implements OnInit {
  constructor(
    public basketService: BasketService,
    private acntService: AcntService
  ) {}

  ngOnInit(): void {
    console.log(`current user:`);
    this.acntService.currentUser$.subscribe({
      next: (res: any) => {
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },
      error: (err: any) => {
        console.log(
          `An error occurred while setting isUserAuthenticated flag.`
        );
      },
    });
  }
  public isUserAuthenticated: boolean = false;
  getBasketCount(items: IBasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }
  public login = () => {
    this.acntService.login();
  };
  public logout = () => {
    this.acntService.signout();
  };
}
