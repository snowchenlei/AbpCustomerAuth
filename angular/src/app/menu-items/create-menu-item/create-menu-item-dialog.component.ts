import { Component, OnInit, Injector } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { MenuItemServiceProxy } from "@shared/service-proxies/service-proxies";
import { MatDialogRef } from "@angular/material";

@Component({
  //selector: "app-create-menu-item",
  templateUrl: "./create-menu-item-dialog.component.html",
  styleUrls: []
})
export class CreateMenuItemDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  //user: CreateUserDto = new CreateUserDto();
  checkedRolesMap: { [key: string]: boolean } = {};
  defaultRoleCheckedStatus = false;

  constructor(
    injector: Injector,
    public _menuItemService: MenuItemServiceProxy,
    private _dialogRef: MatDialogRef<CreateMenuItemDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // this.user.isActive = true;
    // this._menuItemService..getRoles().subscribe(result => {
    //   this.roles = result.items;
    //   this.setInitialRolesStatus();
    // });
  }
}
