import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../../_service/account.service';
import { MembersService } from '../../_service/members.service';
import { Member } from '../../_models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryModule } from 'ng-gallery';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event:any)
  {
    if(this.editForm?.dirty)
    {
      $event.returnValue = true;
    }
  }
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  member?: Member
  private toastr = inject(ToastrService)

  ngOnInit(): void {
     this.loadMember() 
  }


 loadMember()
 {
  const user = this.accountService.currentUser();
  if(!user) return

  this.memberService.getMember(user.username).subscribe({
    next: member => this.member = member
  })
 }

 updateMember()
{
  this.memberService.updateMember(this.editForm?.value).subscribe({
    next: _=>{
 this.toastr.success("Profile update successfully")
  this.editForm?.reset(this.member);

    }
  })
 
}
  
}
