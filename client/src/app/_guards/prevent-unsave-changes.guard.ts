import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { inject } from '@angular/core';
import { ConfirmService } from '../_service/confirm.service';

export const preventUnsaveChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {

  const confirmService = inject(ConfirmService);
  if(component.editForm?.dirty){
    return confirmService.confirm() ?? false;
  }
  return true
  
};
