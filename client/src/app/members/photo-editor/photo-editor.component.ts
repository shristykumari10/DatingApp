import { ChangeDetectorRef, Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import {FileUploader, FileUploadModule} from 'ng2-file-upload'
import { AccountService } from '../../_service/account.service';
import { environment } from '../../../environments/environment';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_service/members.service';

@Component({
  selector: 'app-photo-editor',
  imports: [NgIf, NgFor, NgClass, NgStyle, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
   member = input.required<Member>();
   private accountService = inject(AccountService)
   private memberService = inject(MembersService)
   uploader?: FileUploader;
   hasBaseDropZoneOver = false;
   baseUrl = environment.apiUrl;
   memberChange = output<Member>();
    private cdr = inject(ChangeDetectorRef);


   ngOnInit(): void {
       this.initializeUploader();
   }

   fileOverBase(e : any){
    this.hasBaseDropZoneOver = e;
   }

   initializeUploader(){
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10*1024*1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
      console.log('File added:', file); 
      this.cdr.detectChanges();
    }

  //   this.uploader.onAfterAddingAll = (files) => {
  //   files.forEach(file => file.withCredentials = false);
  // };
    
    this.uploader.onSuccessItem = (item, response, status, headers) =>{
      const photo = JSON.parse(response)
      const updateMember = {...this.member()}
      updateMember.photos.push(photo);
      this.memberChange.emit(updateMember);
    }

     this.uploader.onSuccessItem = (item, response, status, headers) => {
    if (response) {
      const photo = JSON.parse(response);
      const updateMember = { ...this.member() };
      updateMember.photos.push(photo);
      this.memberChange.emit(updateMember);
    }
  };

   }


   deletePhoto(photo: Photo){
    this.memberService.deletePhoto(photo).subscribe({
      next: _ =>{
        const updateMember = {...this.member()};
        updateMember.photos = updateMember.photos.filter(x => x.id !== photo.id);
        this.memberChange.emit(updateMember);
      }
    })
   }

   setMainPhoto(photo: Photo){
     this.memberService.setMainPhoto(photo).subscribe({
      next:_ =>{
        const user = this.accountService.currentUser();
        if(user){
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user)
        }
        const updateMember = {...this.member()}
        updateMember.photoUrl = photo.url;
        updateMember.photos.forEach(p =>{
          if(p.isMain) p.isMain= false
          if(p.id === photo.id)p.isMain = true
        });
        this.memberChange.emit(updateMember);
      }
     })
   }

}
