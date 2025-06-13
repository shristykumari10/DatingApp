import { Component, Inject, inject, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AdminService } from '../../_service/admin.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  private adminService = inject(AdminService)
  users: User[] = []
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  private modalService = inject(BsModalService)
 


  ngOnInit(): void {
    this.getUsersWithRoles();
  }


  openRolesModal(user: User) {
    const initialState: ModalOptions = {
      class: "modal-lg",
      initialState: {
        title: 'User roles',
        username: user.username,
        selectedRoles: [...user.roles],
        users: this.users,
        rolesUpdate: false,
        avilableRoles: ['Admin', 'Moderator', 'Member']
      }
    }

    this.bsModalRef = this.modalService.show(RolesModalComponent, initialState);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        if (this.bsModalRef.content && this.bsModalRef.content.rolesUpdated){
          const selectedRoles = this.bsModalRef.content.selectedRoles;
          this.adminService.updateUserRole(user.username, selectedRoles).subscribe({
            next: role => user.roles = role
          })
        }
      }
    })

  }


  getUsersWithRoles() {
    this.adminService.getUserWithRoles().subscribe({
      next: users => this.users = users
    })
  }
}
