import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoadingService } from '../services/loading.service';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  let loadingService = inject(LoadingService);
  loadingService.loading();
  return next(req).pipe(
    delay(1000),
    finalize(()=>{
      loadingService.idle();
    })
  );
};
