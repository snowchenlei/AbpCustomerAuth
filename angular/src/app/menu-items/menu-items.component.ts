import { Component, OnInit, Injector } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  MenuItemListDto,
  MenuItemServiceProxy,
  PagedResultDtoOfMenuItemListDto,
  MenuItemEditDto
} from "@shared/service-proxies/service-proxies";
import {
  PagedListingComponentBase,
  PagedRequestDto
} from "@shared/paged-listing-component-base";
import { MatDialog } from "@angular/material";
import { finalize } from "rxjs/operators";
import { CreateMenuItemDialogComponent } from "./create-menu-item/create-menu-item-dialog.component";
import { EditMenuItemDialogComponent } from "./edit-menu-item/edit-menu-item-dialog.component";

@Component({
  selector: "app-menu-items",
  templateUrl: "./menu-items.component.html",
  animations: [appModuleAnimation()],
  styleUrls: ["./menu-items.component.css"]
})
export class MenuItemsComponent extends PagedListingComponentBase<
  MenuItemListDto
> {
  menuItems: MenuItemListDto[] = [];
  keyword = "";
  ng;
  isActive: boolean | null;

  constructor(
    injector: Injector,
    private _menuItemService: MenuItemServiceProxy,
    private _dialog: MatDialog
  ) {
    super(injector);
  }

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this._menuItemService
      .getAll("", request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: PagedResultDtoOfMenuItemListDto) => {
        this.menuItems = result.items;
        this.showPaging(result, pageNumber);
      });
  }
  protected delete(entity: MenuItemListDto): void {
    abp.message.confirm(
      this.l("MenuItemDeleteWarningMessage", entity.creationTime),
      (result: boolean) => {
        if (result) {
          this._menuItemService.delete(entity.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }

  createMenuItem(): void {
    this.showCreateOrEditMenuItemDialog();
  }

  editMenuItem(user: MenuItemEditDto): void {
    this.showCreateOrEditMenuItemDialog(user.id);
  }

  private showCreateOrEditMenuItemDialog(id?: number): void {
    let createOrEditMenuItemDialog;
    if (id === undefined || id <= 0) {
      createOrEditMenuItemDialog = this._dialog.open(
        CreateMenuItemDialogComponent
      );
    } else {
      createOrEditMenuItemDialog = this._dialog.open(
        EditMenuItemDialogComponent,
        {
          data: id
        }
      );
    }

    createOrEditMenuItemDialog.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
}
